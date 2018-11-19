using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SFA.DAS.ApplyService.Application;
using SFA.DAS.ApplyService.Application.Apply.Validation;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.DfeSignIn;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Controllers;
using SFA.DAS.ApplyService.Web.Infrastructure;
using StructureMap;

namespace SFA.DAS.ApplyService.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Startup> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, ILogger<Startup> logger, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            
            
            ConfigureAuth(services);
            
            
            
            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });
            
         
            
            services.AddMvc(options => { options.Filters.Add<PerformValidationFilter>(); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            _logger.LogInformation("Passed Configure Mvc");
            
            services.AddSession(opt => { opt.IdleTimeout = TimeSpan.FromHours(1); });
            
            _logger.LogInformation("Passed Add Session");
            
            services.AddDistributedMemoryCache();

            return ConfigureIOC(services).Result;
        }
        
        private async Task<IServiceProvider> ConfigureIOC(IServiceCollection services)
        {
            var container = new Container();

            container.Configure(config =>
            {
                config.Scan(_ =>
                {
                    //_.AssemblyContainingType(typeof(Startup));
                    _.AssembliesFromApplicationBaseDirectory(c => c.FullName.StartsWith("SFA"));
                    _.WithDefaultConventions();

                    _.AddAllTypesOf<IValidator>();
                });
                

                config.For<IHttpContextAccessor>().Use<HttpContextAccessor>();
                
                config.For<IConfigurationService>()
                    .Use<ConfigurationService>().Singleton()
                    .Ctor<string>("environment").Is(_configuration["EnvironmentName"])
                    .Ctor<string>("storageConnectionString").Is(_configuration["ConfigurationStorageConnectionString"])
                    .Ctor<string>("version").Is("1.0")
                    .Ctor<string>("serviceName").Is("SFA.DAS.ApplyService");
                
                config.For<ISessionService>().Use<SessionService>().Ctor<string>("environment")
                    .Is(_configuration["EnvironmentName"]);
                config.For<IDfeSignInService>().Use<DfeSignInService>();

                config.For<IUsersApiClient>().Use<UsersApiClient>();
                config.For<IApplicationApiClient>().Use<ApplicationApiClient>();
                config.For<OrganisationApiClient>().Use<OrganisationApiClient>();
                config.For<OrganisationSearchApiClient>().Use<OrganisationSearchApiClient>();

                config.Populate(services);
            });

            var applyConfig = await container.GetInstance<IConfigurationService>().GetConfig();

            //services.AddHttpClient<UsersApiClient>(c => { c.BaseAddress = new Uri(applyConfig.InternalApi.Uri); });
            //services.AddHttpClient<ApplicationApiClient>(c => { c.BaseAddress = new Uri(applyConfig.InternalApi.Uri); });
            //services.AddHttpClient<OrganisationSearchApiClient>(c => { c.BaseAddress = new Uri(applyConfig.InternalApi.Uri); });
            //services.AddHttpClient<OrganisationApiClient>(c => { c.BaseAddress = new Uri(applyConfig.InternalApi.Uri); });

            return container.GetInstance<IServiceProvider>();
        }

        protected virtual void ConfigureAuth(IServiceCollection services)
        {

            var configService = new ConfigurationService(_hostingEnvironment, _configuration["EnvironmentName"],
                _configuration["ConfigurationStorageConnectionString"], "1.0", "SFA.DAS.ApplyService");
            
            services.AddDfeSignInAuthorization(configService.GetConfig().Result);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation("Configure");
            
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
            
            logger.LogInformation("Before UseStaticFiles");
            app.UseStaticFiles();
            logger.LogInformation("Before UseSession");
            app.UseSession();
            logger.LogInformation("Before UseAuthentication");
            app.UseAuthentication();
            logger.LogInformation("Before UseMvc");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}