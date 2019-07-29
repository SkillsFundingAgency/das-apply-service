using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using SFA.DAS.ApplyService.Application;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Validation;
using SFA.DAS.ApplyService.Application.Organisations;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Application.Users;
using SFA.DAS.ApplyService.Application.Users.CreateAccount;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Data;
using SFA.DAS.ApplyService.DfeSignIn;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.Encryption;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Storage;
using StructureMap;

namespace SFA.DAS.ApplyService.InternalApi
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _configuration;
        private const string _serviceName = "SFA.DAS.ApplyService";
        private const string _version = "1.0";

        private readonly IApplyConfig _applyConfig;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _env = env;
            _configuration = configuration;
            
            _applyConfig = new ConfigurationService(_env, _configuration["EnvironmentName"], _configuration["ConfigurationStorageConnectionString"], _version, _serviceName).GetConfig().GetAwaiter().GetResult();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            IdentityModelEventSource.ShowPII = true;
            
        
            services.AddAuthentication(o =>
                {
                    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.Authority = $"https://login.microsoftonline.com/{_applyConfig.ApiAuthentication.TenantId}"; 
                    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                        ValidAudiences = new List<string>
                        {
                            _applyConfig.ApiAuthentication.Audience,
                            _applyConfig.ApiAuthentication.ClientId
                        }
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = c =>
                        {
                            
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = c =>
                        {
                            return Task.CompletedTask;
                        }, OnAuthenticationFailed = c =>
                        {
                            return Task.CompletedTask;
                        }
                    };
                });    
            
            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

            services.AddHttpClient<AssessorServiceApiClient>("AssessorServiceApiClient", config =>
            {
                config.BaseAddress = new Uri(_applyConfig.AssessorServiceApiAuthentication.ApiBaseAddress); //  "http://localhost:59022"
                config.DefaultRequestHeaders.Add("Accept", "Application/json");
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            services.AddHttpClient<ProviderRegisterApiClient>("ProviderRegisterApiClient", config =>
            {
                config.BaseAddress = new Uri(_applyConfig.ProviderRegisterApiAuthentication.ApiBaseAddress); //  "https://findapprenticeshiptraining-api.sfa.bis.gov.uk"
                config.DefaultRequestHeaders.Add("Accept", "Application/json");
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            services.AddHttpClient<ReferenceDataApiClient>("ReferenceDataApiClient", config =>
            {
                config.BaseAddress = new Uri(_applyConfig.ReferenceDataApiAuthentication.ApiBaseAddress); //  "https://at-refdata.apprenticeships.sfa.bis.gov.uk/api"
                config.DefaultRequestHeaders.Add("Accept", "Application/json");
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            services.AddHttpClient<CompaniesHouseApiClient>("CompaniesHouseApiClient", config =>
            {
                config.BaseAddress = new Uri(_applyConfig.CompaniesHouseApiAuthentication.ApiBaseAddress); //  "https://api.companieshouse.gov.uk"
                config.DefaultRequestHeaders.Add("Accept", "Application/json");
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-GB");
                options.SupportedCultures = new List<CultureInfo> { new CultureInfo("en-GB") };
                options.SupportedUICultures = new List<CultureInfo> { new CultureInfo("en-GB") };
                options.RequestCultureProviders.Clear();
            });
            
            
            IMvcBuilder mvcBuilder;
            if (_env.IsDevelopment())
                mvcBuilder = services.AddMvc(opt => { opt.Filters.Add(new AllowAnonymousFilter()); });
            else
                mvcBuilder = services.AddMvc();

            mvcBuilder.SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>());
            
            services.AddDistributedMemoryCache();

            return ConfigureIOC(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            MappingStartup.AddMappings();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
                app.UseHttpsRedirection();
            }
            app.UseRequestLocalization();
            app.UseSecurityHeaders();

            app.UseAuthentication();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        
        private IServiceProvider ConfigureIOC(IServiceCollection services)
        {
            var container = new Container();

            container.Configure(config =>
            {
                config.Scan(_ =>
                {
                    //_.AssemblyContainingType(typeof(Startup));
                    _.AssembliesFromApplicationBaseDirectory(c => c.FullName.StartsWith("SFA"));
                    _.WithDefaultConventions();

                    _.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<>)); // Handlers with no response
                    _.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>)); // Handlers with a response
                    _.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));

                    _.AddAllTypesOf<IValidator>();
                });

                config.For<IMediator>().Use<Mediator>();

                config.For<IHttpContextAccessor>().Use<HttpContextAccessor>();
                
                config.For<IConfigurationService>()
                    .Use<ConfigurationService>().Singleton()
                    .Ctor<string>("environment").Is(_configuration["EnvironmentName"])
                    .Ctor<string>("storageConnectionString").Is(_configuration["ConfigurationStorageConnectionString"])
                    .Ctor<string>("version").Is(_version)
                    .Ctor<string>("serviceName").Is(_serviceName);

                config.For<IContactRepository>().Use<ContactRepository>();
                config.For<IApplyRepository>().Use<ApplyRepository>();
                config.For<IOrganisationRepository>().Use<OrganisationRepository>();
                config.For<IDfeSignInService>().Use<DfeSignInService>();
                config.For<IEmailService>().Use<EmailService.EmailService>();

                // NOTE: These are SOAP Services. Their client interfaces are contained within the generated Proxy code.
                config.For<CharityCommissionService.ISearchCharitiesV1SoapClient>().Use<CharityCommissionService.SearchCharitiesV1SoapClient>();
                config.For<CharityCommissionApiClient>().Use<CharityCommissionApiClient>();
                // End of SOAP Services

                config.For<IKeyProvider>().Use<PlaceholderKeyProvider>();
                config.For<IStorageService>().Use<StorageService>();
                
                services.AddMediatR(typeof(CreateAccountHandler).GetTypeInfo().Assembly);
                
                config.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }
    }
}