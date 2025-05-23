﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.IdentityModel.Logging;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Application.Services.Assessor;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.DfeSignIn;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.EmailService;
using SFA.DAS.ApplyService.EmailService.Infrastructure;
using SFA.DAS.ApplyService.EmailService.Interfaces;
using SFA.DAS.ApplyService.Infrastructure.ApiClients;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Authorization;
using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Infrastructure.FeatureToggles;
using SFA.DAS.ApplyService.Web.Infrastructure.Interfaces;
using SFA.DAS.ApplyService.Web.Infrastructure.Services;
using SFA.DAS.ApplyService.Web.Infrastructure.Validations;
using SFA.DAS.ApplyService.Web.Orchestrators;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.StartupExtensions;
using SFA.DAS.ApplyService.Web.Validators;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.GovUK.Auth.AppStart;
using SFA.DAS.GovUK.Auth.Models;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.Http;
using SFA.DAS.Http.TokenGenerators;
using SFA.DAS.Notifications.Api.Client;

namespace SFA.DAS.ApplyService.Web
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Startup> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IApplyConfig _configService;

        public Startup(IConfiguration configuration, ILogger<Startup> logger, IWebHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;

            var config = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile("appsettings.Development.json", true);


            config.AddEnvironmentVariables();
            config.AddAzureTableStorage(options =>
                {
                    options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
                    options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                    options.EnvironmentName = configuration["EnvironmentName"];
                    options.PreFixConfigurationKeys = false;
                }
            );

            _configuration = config.Build();
            _configService = _configuration.GetSection(nameof(ApplyConfig)).Get<ApplyConfig>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = false;

            ConfigureAuth(services);

            services.AddTransient<IAuthorizationHandler, AuthorizationHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AccessAppeal", policy =>
                {
                    policy.Requirements.Add(new AccessApplicationRequirement());
                });
                options.AddPolicy("AccessAppealNotYetSubmitted", policy =>
                {
                    policy.Requirements.Add(new AccessApplicationRequirement());
                    policy.Requirements.Add(new AppealNotYetSubmittedRequirement());
                });
                options.AddPolicy("AccessInProgressApplication", policy =>
                {
                    policy.Requirements.Add(new AccessApplicationRequirement());
                    policy.Requirements.Add(new ApplicationStatusRequirement(ApplicationStatus.InProgress, ApplicationStatus.New));
                });
                options.AddPolicy("AccessApplication", policy =>
                {
                    policy.Requirements.Add(new AccessApplicationRequirement());
                });
            });

            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

            ConfigHttpClients(services);

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-GB");
                options.SupportedCultures = new List<CultureInfo> { new CultureInfo("en-GB") };
                options.SupportedUICultures = new List<CultureInfo> { new CultureInfo("en-GB") };
                options.RequestCultureProviders.Clear();
            });

            services.AddMvc(options =>
            {
                options.Filters.Add<FeatureToggleFilter>();
            })
            .AddSessionStateTempDataProvider();

            services.AddFluentValidationAutoValidation().AddValidatorsFromAssemblyContaining<ManagementHierarchyValidator>();

            services.AddLogging(builder =>
            {
                builder.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Information);
                builder.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Information);
            });

            services.AddApplicationInsightsTelemetry();

            services.AddOptions();

            services.Configure<List<TaskListConfiguration>>(_configuration.GetSection("TaskListSequences"));
            services.Configure<List<QnaPageOverrideConfiguration>>(_configuration.GetSection("QnaPageOverrides"));
            services.Configure<List<QnaLinksConfiguration>>(_configuration.GetSection("QnaLinks"));
            services.Configure<List<CustomValidationConfiguration>>(_configuration.GetSection("CustomValidations"));
            services.Configure<List<NotRequiredOverrideConfiguration>>(_configuration.GetSection("NotRequiredOverrides"));
            services.Configure<List<OuterApiConfiguration>>(_configuration.GetSection("OuterApiConfiguration"));

            services.AddCache(_configService, _hostingEnvironment);
            services.AddDataProtection(_configService, _hostingEnvironment);

            services.AddSession(opt =>
            {
                opt.IdleTimeout = TimeSpan.FromHours(1);
                opt.Cookie = new CookieBuilder()
                {
                    Name = ".Apply.Session",
                    HttpOnly = true
                };
            });

            services.AddSingleton<Microsoft.AspNetCore.Mvc.ViewFeatures.IHtmlGenerator, CacheOverrideHtmlGenerator>();

            services.AddAntiforgery(options => options.Cookie = new CookieBuilder() { Name = ".Apply.AntiForgery", HttpOnly = true });

            services.AddHealthChecks();

            ConfigureDependencyInjection(services);
        }

        private void ConfigHttpClients(IServiceCollection services)
        {
            var handlerLifeTime = TimeSpan.FromMinutes(5);

            services.AddHttpClient<IApplicationApiClient, ApplicationApiClient>(config =>
            {
                config.BaseAddress = new Uri(_configService.InternalApi.ApiBaseAddress);
            })
            .SetHandlerLifetime(handlerLifeTime);

            services.AddHttpClient<IQnaApiClient, QnaApiClient>(config =>
            {
                config.BaseAddress = new Uri(_configService.QnaApiAuthentication.ApiBaseAddress);
            })
            .SetHandlerLifetime(handlerLifeTime);

            services.AddHttpClient<IOuterApiClient, OuterApiClient>(config =>
            {
                config.BaseAddress = new Uri(_configService.OuterApiConfiguration.ApiBaseUrl);
                config.DefaultRequestHeaders.Add("Accept", "application/json");
                config.DefaultRequestHeaders.Add("X-Version", "1");
                config.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _configService.OuterApiConfiguration.SubscriptionKey);
            })
            .SetHandlerLifetime(handlerLifeTime);

            services.AddHttpClient<ICompaniesHouseApiClient, CompaniesHouseApiClient>(config =>
            {
                config.BaseAddress = new Uri(_configService.InternalApi.ApiBaseAddress);
            })
            .SetHandlerLifetime(handlerLifeTime);

            services.AddHttpClient<IEmailTemplateClient, EmailTemplateClient>(config =>
            {
                config.BaseAddress = new Uri(_configService.InternalApi.ApiBaseAddress);
            })
            .SetHandlerLifetime(handlerLifeTime);

            services.AddHttpClient<IOrganisationApiClient, OrganisationApiClient>(config =>
            {
                config.BaseAddress = new Uri(_configService.InternalApi.ApiBaseAddress);
            })
            .SetHandlerLifetime(handlerLifeTime);

            services.AddHttpClient<IRoatpApiClient, RoatpApiClient>(config =>
            {
                config.BaseAddress = new Uri(_configService.InternalApi.ApiBaseAddress);
            })
            .SetHandlerLifetime(handlerLifeTime);

            services.AddHttpClient<IUkrlpApiClient, UkrlpApiClient>(config =>
            {
                config.BaseAddress = new Uri(_configService.InternalApi.ApiBaseAddress);
            })
            .SetHandlerLifetime(handlerLifeTime);

            services.AddHttpClient<IUsersApiClient, UsersApiClient>(config =>
            {
                config.BaseAddress = new Uri(_configService.InternalApi.ApiBaseAddress);
            })
            .SetHandlerLifetime(handlerLifeTime);

            services.AddHttpClient<IAllowedProvidersApiClient, AllowedProvidersApiClient>(config =>
            {
                config.BaseAddress = new Uri(_configService.InternalApi.ApiBaseAddress);
            })
            .SetHandlerLifetime(handlerLifeTime);

            services.AddHttpClient<IOutcomeApiClient, OutcomeApiClient>(config =>
            {
                config.BaseAddress = new Uri(_configService.InternalApi.ApiBaseAddress);
            })
            .SetHandlerLifetime(handlerLifeTime);

            services.AddHttpClient<IAppealsApiClient, AppealsApiClient>(config =>
            {
                config.BaseAddress = new Uri(_configService.InternalApi.ApiBaseAddress);
            })
            .SetHandlerLifetime(handlerLifeTime);
        }

        private void ConfigureDependencyInjection(IServiceCollection services)
        {
            services.AddTransient<ITokenService, TokenService>();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IApplyConfig>(_ => _configService);

            // TODO: IConfigurationService could be remov
            services.AddSingleton<IConfigurationService>(sp => new ConfigurationService(
                sp.GetService<IWebHostEnvironment>(),
                _configuration["EnvironmentName"],
                _configuration["ConfigurationStorageConnectionString"],
                "1.0",
                "SFA.DAS.ApplyService"));

            services.AddTransient<ISessionService>(s => new SessionService(
                s.GetService<IHttpContextAccessor>(),
                _configuration["EnvironmentName"]));

            services.AddTransient<IFeatureToggles>(_ => _configService.FeatureToggles ?? new FeatureToggles());

            services.AddTransient<IDfeSignInService, DfeSignInService>();
            services.AddTransient<IQnaTokenService, QnaTokenService>();
            services.AddTransient<IProcessPageFlowService, ProcessPageFlowService>();
            services.AddTransient<IResetRouteQuestionsService, ResetRouteQuestionsService>();
            services.AddTransient<IPagesWithSectionsFlowService, PagesWithSectionsFlowService>();
            services.AddTransient<IRefreshTrusteesService, RefreshTrusteesService>();
            services.AddTransient<IQuestionPropertyTokeniser, QuestionPropertyTokeniser>();
            services.AddTransient<IPageNavigationTrackingService, PageNavigationTrackingService>();
            services.AddTransient<ICustomValidatorFactory, CustomValidatorFactory>();
            services.AddTransient<IAnswerFormService, AnswerFormService>();
            services.AddTransient<ITrusteeExemptionService, TrusteeExemptionService>();
            services.AddTransient<IEmailTokenService, EmailTokenService>();
            services.AddTransient<IAssessorLookupService, AssessorLookupService>();
            services.AddTransient<IGetHelpWithQuestionEmailService, GetHelpWithQuestionEmailService>();
            services.AddTransient<IReapplicationCheckService, ReapplicationCheckService>();
            services.AddTransient<IResetCompleteFlagService, ResetCompleteFlagService>();
            services.AddTransient<IRequestInvitationToReapplyEmailService, RequestInvitationToReapplyEmailService>();
            services.AddTransient<INotificationsApi>(x =>
            {
                var apiConfiguration = new Notifications.Api.Client.Configuration.NotificationsApiClientConfiguration
                {
                    ApiBaseUrl = _configService.NotificationsApiClientConfiguration.ApiBaseUrl,
#pragma warning disable 618
                    ClientToken = _configService.NotificationsApiClientConfiguration.ClientToken,
#pragma warning restore 618
                    ClientId = _configService.NotificationsApiClientConfiguration.ClientId,
                    ClientSecret = _configService.NotificationsApiClientConfiguration.ClientSecret,
                    IdentifierUri = _configService.NotificationsApiClientConfiguration.IdentifierUri,
                    Tenant = _configService.NotificationsApiClientConfiguration.Tenant
                };

                var httpClient = string.IsNullOrWhiteSpace(apiConfiguration.ClientId)
                    ? new HttpClientBuilder().WithBearerAuthorisationHeader(new JwtBearerTokenGenerator(apiConfiguration)).Build()
                    : new HttpClientBuilder().WithBearerAuthorisationHeader(new AzureADBearerTokenGenerator(apiConfiguration)).Build();

                return new NotificationsApi(httpClient, apiConfiguration);
            });

            services.AddTransient<ISubmitApplicationConfirmationEmailService, SubmitApplicationConfirmationEmailService>();
            services.AddTransient<ITabularDataService, TabularDataService>();
            services.AddTransient<ITabularDataRepository, TabularDataRepository>();
            services.AddTransient<IAllowedUkprnValidator, AllowedUkprnValidator>();
            services.AddTransient<IRoatpTaskListWorkflowService, RoatpTaskListWorkflowService>();
            services.AddTransient<IRoatpOrganisationVerificationService, RoatpOrganisationVerificationService>();
            services.AddTransient<INotRequiredOverridesService, NotRequiredOverridesService>();
            services.AddTransient<ITaskListOrchestrator, TaskListOrchestrator>();
            services.AddTransient<IOverallOutcomeService, OverallOutcomeService>();

            services.AddTransient<IStubAuthenticationService, StubAuthenticationService>();
            services.AddTransient<ICustomClaims, CustomClaims>();
        }

        protected virtual void ConfigureAuth(IServiceCollection services)
        {
            if (_configService.UseGovSignIn)
            {
                services.Configure<GovUkOidcConfiguration>(_configuration.GetSection("GovUkOidcConfiguration"));
                var cookieDomain = DomainExtensions.GetDomain(_configuration["ResourceEnvironmentName"]);
                var loginRedirect = string.IsNullOrEmpty(cookieDomain) ? "" : $"https://{cookieDomain}/account-details";
                services.AddAndConfigureGovUkAuthentication(_configuration, new AuthRedirects
                {
                    CookieDomain = cookieDomain,
                    LoginRedirect = loginRedirect,
                    SignedOutRedirectUrl = "/Users/SignedOut",
                    LocalStubLoginPath = "/account-details"
                },
                    typeof(CustomClaims));
            }
            else
            {
                services.AddDfeSignInAuthorization(_configService, _logger, _hostingEnvironment);
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            MappingStartup.AddMappings();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error/{0}");
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseRouting();
            app.UseSession();
            app.UseRequestLocalization();
            app.UseStatusCodePagesWithReExecute("/Home/error/{0}");
            app.UseSecurityHeaders();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHealthChecks("/health");
            app.UseEndpoints(routes =>
            {
                routes.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}