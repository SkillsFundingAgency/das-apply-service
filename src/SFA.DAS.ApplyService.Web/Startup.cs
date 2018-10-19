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
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SFA.DAS.ApplyService.Application;
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

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<UsersApiClient>(c => { c.BaseAddress = new Uri("https://localhost:6000"); });
            
            services.AddTransient<ISessionService>(p =>
                new SessionService(p.GetService<IHttpContextAccessor>(), _configuration["EnvironmentName"]));
            services.AddSingleton<IConfigurationService>(p => new ConfigurationService(
                p.GetService<IHostingEnvironment>(), _configuration["EnvironmentName"],
                _configuration["ConfigurationStorageConnectionString"], "1.0", "SFA.DAS.ApplyService"));
            services.AddTransient<IDfeSignInService, DfeSignInService>();
            
            ConfigureAuth(services);
            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });
            ConfigureMvc(services);

            services.AddSession(opt => { opt.IdleTimeout = TimeSpan.FromHours(1); });
            
            services.AddDistributedMemoryCache();
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