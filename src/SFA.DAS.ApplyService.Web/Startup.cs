using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
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
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Web.Infrastructure;

namespace SFA.DAS.ApplyService.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private IApplyConfig _applyConfig;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
//            services.Configure<CookiePolicyOptions>(options =>
//            {
//                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
//                options.CheckConsentNeeded = context => true;
//                options.MinimumSameSitePolicy = SameSiteMode.None;
//            });
            
            _applyConfig = ConfigurationService.GetConfig(_hostingEnvironment, _configuration["EnvironmentName"], _configuration["ConfigurationStorageConnectionString"], "1.0", "SFA.DAS.ApplyService").Result;
            
            services.AddDfeSignInAuthorization(_applyConfig);
            
            services.AddMvc()
                //.AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            services.AddSession(opt => { opt.IdleTimeout = TimeSpan.FromHours(1); });
            
            services.AddDistributedMemoryCache();
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
            //app.UseCookiePolicy();
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