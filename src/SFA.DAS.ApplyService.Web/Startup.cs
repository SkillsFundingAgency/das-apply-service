using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using SFA.DAS.ApplyService.Application.Apply.Validation;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.DfeSignIn;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Validators;
using StackExchange.Redis;

namespace SFA.DAS.ApplyService.Web
{
    using Controllers;
    using SFA.DAS.ApplyService.Web.Configuration;

    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Startup> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHostingEnvironment _env;
        private readonly IApplyConfig _configService;
        private const string ServiceName = "SFA.DAS.ApplyService";
        private const string Version = "1.0";

        public Startup(IConfiguration configuration, ILogger<Startup> logger, IHostingEnvironment hostingEnvironment, IHostingEnvironment env)
        {
            _configuration = configuration;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _env = env;
            _configService =  new ConfigurationService(env, _configuration["EnvironmentName"], _configuration["ConfigurationStorageConnectionString"], Version, ServiceName).GetConfig().GetAwaiter().GetResult();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureAuth(services);
            
            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-GB");
                options.SupportedCultures = new List<CultureInfo> { new CultureInfo("en-GB") };
                options.SupportedUICultures = new List<CultureInfo> { new CultureInfo("en-GB") };
                options.RequestCultureProviders.Clear();
            });
            
            services.AddMvc(options => { options.Filters.Add<PerformValidationFilter>(); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddOptions();

            services.Configure<List<TaskListConfiguration>>(_configuration.GetSection("TaskListSequences"));

            if (_env.IsDevelopment())
            {
                services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "keys")))
                    .SetApplicationName("AssessorApply");

                services.AddDistributedMemoryCache();
            }
            else
            {
                try
                {
                    var redis = ConnectionMultiplexer.Connect(
                        $"{_configService.SessionRedisConnectionString},DefaultDatabase=1");

                    services.AddDataProtection()
                        .PersistKeysToStackExchangeRedis(redis, "AssessorApply-DataProtectionKeys")
                        .SetApplicationName("AssessorApply");
                    services.AddDistributedRedisCache(options =>
                    {
                        options.Configuration = $"{_configService.SessionRedisConnectionString},DefaultDatabase=0";
                    });
                }
                catch (Exception e)
                {
                    _logger.LogError(e,
                        $"Error setting redis for session.  Conn: {_configService.SessionRedisConnectionString}");
                    throw;
                }
            }

            services.AddSession(opt =>
            {
                opt.IdleTimeout = TimeSpan.FromHours(1);
                opt.Cookie = new CookieBuilder()
                {
                    Name = ".Assessors.Session",
                    HttpOnly = true
                };
            });
            
            services.AddSingleton<Microsoft.AspNetCore.Mvc.ViewFeatures.IHtmlGenerator,CacheOverrideHtmlGenerator>();
            
            services.AddAntiforgery(options => options.Cookie = new CookieBuilder() { Name = ".Apply.AntiForgery", HttpOnly = true });

            services.AddHealthChecks();

            ConfigureDependencyInjection(services);
        }
        
        private void ConfigureDependencyInjection(IServiceCollection services)
        {

            services.RegisterAllTypes<IValidator>(new[] { typeof(IValidator).Assembly });
            
            services.AddTransient<ITokenService, TokenService>();
            
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IConfigurationService>(sp => new ConfigurationService(
                sp.GetService<IHostingEnvironment>(),
                _configuration["EnvironmentName"],
                _configuration["ConfigurationStorageConnectionString"],
                "1.0",
                "SFA.DAS.ApplyService"));

            services.AddTransient<ISessionService>(s => new SessionService(
                s.GetService<IHttpContextAccessor>(), 
                _configuration["EnvironmentName"]));

            services.AddTransient<IDfeSignInService,DfeSignInService>();

            services.AddTransient<IUsersApiClient,UsersApiClient>();
            services.AddTransient<UsersApiClient, UsersApiClient>();
            services.AddTransient<IApplicationApiClient,ApplicationApiClient>();
            services.AddTransient<OrganisationApiClient,OrganisationApiClient>();
            services.AddTransient<OrganisationSearchApiClient,OrganisationSearchApiClient>();
            services.AddTransient<CreateAccountValidator,CreateAccountValidator>();
            services.AddTransient<IUserService,UserService>();
            services.AddTransient<IQnaTokenService, QnaTokenService>();
            services.AddTransient<IQnaApiClient, QnaApiClient>();
            services.AddTransient<IOrganisationApiClient, OrganisationApiClient>();
            services.AddTransient<IRoatpApiClient, RoatpApiClient>();
            services.AddTransient<IUkrlpApiClient, UkrlpApiClient>();
            services.AddTransient<IRoatpStatusValidator, RoatpStatusValidator>();
            services.AddTransient<ICompaniesHouseApiClient, CompaniesHouseApiClient>();
            services.AddTransient<ICharityCommissionApiClient, CharityCommissionApiClient>();
        }

        protected virtual void ConfigureAuth(IServiceCollection services)
        {

            var configService = new ConfigurationService(_hostingEnvironment, _configuration["EnvironmentName"],
                _configuration["ConfigurationStorageConnectionString"], "1.0", "SFA.DAS.ApplyService");
            
            services.AddDfeSignInAuthorization(configService.GetConfig().Result, _logger, _hostingEnvironment);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
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
            
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseRequestLocalization();
            app.UseSecurityHeaders();
            app.UseHealthChecks("/health");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}