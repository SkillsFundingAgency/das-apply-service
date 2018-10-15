using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ApplyService.Application;
using SFA.DAS.ApplyService.Application.Users;
using SFA.DAS.ApplyService.Application.Users.CreateAccount;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Data;
using SFA.DAS.ApplyService.DfeSignIn;
using SFA.DAS.ApplyService.Session;
using StructureMap;

namespace SFA.DAS.ApplyService.InternalApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            services.AddDistributedMemoryCache();

            return ConfigureIOC(services);
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
                });
                
                config.For<IMediator>().Use<Mediator>();

                config.For<IHttpContextAccessor>().Use<HttpContextAccessor>();
                
//                config.For<ISessionService>().Use<SessionService>().Ctor<string>()
//                    .Is(_configuration["EnvironmentName"]);
//                
//                services.AddSingleton<ISessionService>(p =>
//                    new SessionService(p.GetService<IHttpContextAccessor>(), _configuration["EnvironmentName"]));

                config.For<IConfigurationService>()
                    .Use<ConfigurationService>().Singleton()
                    .Ctor<string>("environment").Is(_configuration["EnvironmentName"])
                    .Ctor<string>("storageConnectionString").Is(_configuration["ConfigurationStorageConnectionString"])
                    .Ctor<string>("version").Is("1.0")
                    .Ctor<string>("serviceName").Is("SFA.DAS.ApplyService");
                
//                services.AddSingleton<IConfigurationService>(p => new ConfigurationService(p.GetService<ISessionService>(),
//                    p.GetService<IHostingEnvironment>(), _configuration["EnvironmentName"],
//                    _configuration["ConfigurationStorageConnectionString"], "1.0", "SFA.DAS.ApplyService"));

                config.For<IContactRepository>().Use<ContactRepository>();
                config.For<IDfeSignInService>().Use<DfeSignInService>();
                config.For<IEmailService>().Use<EmailService.EmailService>();

                services.AddMediatR(typeof(CreateAccountHandler).GetTypeInfo().Assembly);
                
                config.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }
    }
}