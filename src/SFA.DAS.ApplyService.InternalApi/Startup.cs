using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<Startup> _logger;
        private readonly IConfiguration _configuration;
        private const string _serviceName = "SFA.DAS.ApplyService";
        private const string _version = "1.0";

        private readonly IApplyConfig _applyConfig;

        public Startup(IConfiguration configuration, IHostingEnvironment env, ILogger<Startup> logger)
        {
            _env = env;
            _logger = logger;
            _configuration = configuration;

            _logger.LogInformation("In startup constructor.  Before GetConfig");
            _applyConfig = new ConfigurationService(_env, _configuration["EnvironmentName"], _configuration["ConfigurationStorageConnectionString"], _version, _serviceName).GetConfig().GetAwaiter().GetResult();
            _logger.LogInformation("In startup constructor.  After GetConfig");
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
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
//                app.UseHsts();
//                app.UseHttpsRedirection();
            }

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


                config.For<IKeyProvider>().Use<PlaceholderKeyProvider>();
                config.For<IStorageService>().Use<StorageService>();
                
                services.AddMediatR(typeof(CreateAccountHandler).GetTypeInfo().Assembly);
                
                config.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }
    }
}