using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
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
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Validation;
using SFA.DAS.ApplyService.Application.Email;
using SFA.DAS.ApplyService.Application.Organisations;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Application.Users;
using SFA.DAS.ApplyService.Application.Users.CreateAccount;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Data;
using SFA.DAS.ApplyService.DfeSignIn;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.Web.Infrastructure;
using CharityCommissionApiClient = SFA.DAS.ApplyService.InternalApi.Infrastructure.CharityCommissionApiClient;
using CompaniesHouseApiClient = SFA.DAS.ApplyService.InternalApi.Infrastructure.CompaniesHouseApiClient;
using IQnaTokenService = SFA.DAS.ApplyService.InternalApi.Infrastructure.IQnaTokenService;
using IRoatpApiClient = SFA.DAS.ApplyService.InternalApi.Infrastructure.IRoatpApiClient;
using QnaTokenService = SFA.DAS.ApplyService.InternalApi.Infrastructure.QnaTokenService;
using RoatpApiClient = SFA.DAS.ApplyService.InternalApi.Infrastructure.RoatpApiClient;
using SecurityHeadersExtensions = SFA.DAS.ApplyService.InternalApi.Infrastructure.SecurityHeadersExtensions;
using ServiceCollectionExtensions = SFA.DAS.ApplyService.InternalApi.Infrastructure.ServiceCollectionExtensions;

namespace SFA.DAS.ApplyService.InternalApi
{
    using SFA.DAS.ApplyService.Domain.Roatp;
    using SFA.DAS.ApplyService.InternalApi.Models.Roatp;
    using SFA.DAS.ApplyService.InternalApi.Services;
    using Swashbuckle.AspNetCore.Swagger;
    using System.IO;

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

        public void ConfigureServices(IServiceCollection services)
        {
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

            services.AddHttpClient<RoatpApiClient>("RoatpApiClient", config =>
            {
                config.BaseAddress = new Uri(_applyConfig.RoatpApiAuthentication.ApiBaseAddress);          // "https://providers-api.apprenticeships.education.gov.uk"
                config.DefaultRequestHeaders.Add("Accept", "Application/json");
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            services.AddHttpClient<RoatpApiClient>("InternalQnaApiClient", config =>
                {
                    config.BaseAddress = new Uri(_applyConfig.QnaApiAuthentication.ApiBaseAddress);          // "http://localhost:5554"
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

            services.AddOptions();

            services.Configure<List<RoatpSequences>>(_configuration.GetSection("RoatpSequences"));

            mvcBuilder.SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>());
            
            services.AddDistributedMemoryCache();

            services.AddHealthChecks();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "SFA.DAS.ApplyService.InternalApi", Version = "v1" });

                if (_env.IsDevelopment())
                {
                    var basePath = AppContext.BaseDirectory;
                    var xmlPath = Path.Combine(basePath, "SFA.DAS.ApplyService.InternalApi.xml");
                    c.IncludeXmlComments(xmlPath);
                }
            });

            ConfigureDependencyInjection(services);
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

            app.UseSwagger()
                    .UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SFA.DAS.ApplyService.InternalApi v1");
                    })
                    .UseAuthentication();

            app.UseRequestLocalization();
            SecurityHeadersExtensions.UseSecurityHeaders(app);

            app.UseAuthentication();
            app.UseHealthChecks("/health");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        
        private void ConfigureDependencyInjection(IServiceCollection services)
        {
            ServiceCollectionExtensions.RegisterAllTypes<IValidator>(services, new[] { typeof(IValidator).Assembly });
            
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
                
            services.AddSingleton<IConfigurationService>(sp => new ConfigurationService(
                 sp.GetService<IHostingEnvironment>(),
                 _configuration["EnvironmentName"],
                 _configuration["ConfigurationStorageConnectionString"],
                 _version,
                 _serviceName));

            services.AddTransient<IValidatorFactory, ValidatorFactory>();
            
            services.AddTransient<IContactRepository,ContactRepository>();
            services.AddTransient<IApplyRepository,ApplyRepository>();
            services.AddTransient<IOrganisationRepository,OrganisationRepository>();
            services.AddTransient<IDfeSignInService,DfeSignInService>();
            
            services.AddTransient<IEmailService, EmailService.EmailService>();
            services.AddTransient<IEmailTemplateRepository, EmailTemplateRepository>();

            // NOTE: These are SOAP Services. Their client interfaces are contained within the generated Proxy code.
            services.AddTransient<CharityCommissionService.ISearchCharitiesV1SoapClient,CharityCommissionService.SearchCharitiesV1SoapClient>();
            services.AddTransient<CharityCommissionApiClient,CharityCommissionApiClient>();
            // End of SOAP Services

            services.AddTransient<IRoatpApiClient, RoatpApiClient>();
            services.AddTransient<IInternalQnaApiClient, InternalQnaApiClient>();
            services.AddTransient<IQnaTokenService, QnaTokenService>();
            services.AddTransient<IRoatpTokenService, RoatpTokenService>();
            services.AddTransient<IGatewayApiChecksService, GatewayApiChecksService>();

            services.AddMediatR(typeof(CreateAccountHandler).GetTypeInfo().Assembly);
        }
    }
}