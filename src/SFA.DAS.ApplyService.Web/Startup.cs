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
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.DfeSignIn;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Controllers;
using SFA.DAS.ApplyService.Web.Infrastructure;

namespace SFA.DAS.ApplyService.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            
            _logger.LogInformation("ConfigureServices");
            _logger.LogInformation(_configuration["EnvironmentName"]);
            _logger.LogInformation(_configuration["ConfigurationStorageConnectionString"]);
            
            services.AddTransient<ISessionService>(p =>
                new SessionService(p.GetService<IHttpContextAccessor>(), _configuration["EnvironmentName"]));
            services.AddSingleton<IConfigurationService>(p => new ConfigurationService(
                p.GetService<IHostingEnvironment>(), _configuration["EnvironmentName"],
                _configuration["ConfigurationStorageConnectionString"], "1.0", "SFA.DAS.ApplyService"));
            services.AddTransient<IDfeSignInService, DfeSignInService>();

            _logger.LogInformation("Passed registering services");
            
            
            AddApiClients(services, services.BuildServiceProvider());
            
            _logger.LogInformation("Passed Api Clients");
            
            ConfigureAuth(services);
            
            _logger.LogInformation("Passed Config auth");
            
            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });
            
            _logger.LogInformation("Passed Localization");
            
            ConfigureMvc(services);

            _logger.LogInformation("Passed Configure Mvc");
            
            services.AddSession(opt => { opt.IdleTimeout = TimeSpan.FromHours(1); });
            
            _logger.LogInformation("Passed Add Session");
            
            services.AddDistributedMemoryCache();
            
            _logger.LogInformation("Passed Memory Cache");
        }

        private static async void AddApiClients(IServiceCollection services, IServiceProvider serviceProvider)
        {
            var config = await serviceProvider.GetRequiredService<IConfigurationService>().GetConfig();
            services.AddHttpClient<UsersApiClient>(c => { c.BaseAddress = new Uri(config.InternalApi.Uri); });
            services.AddHttpClient<ApplicationApiClient>(c => { c.BaseAddress = new Uri(config.InternalApi.Uri); });
        }
        
        protected virtual void ConfigureMvc(IServiceCollection services)
        {
            services.AddMvc(options => { options.Filters.Add<PerformValidationFilter>(); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //.AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>());
        }

        protected virtual void ConfigureAuth(IServiceCollection services)
        {
            services.AddDfeSignInAuthorization();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
            
            app.UseStaticFiles();
            app.UseSession();

            app.UseAuthentication();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}