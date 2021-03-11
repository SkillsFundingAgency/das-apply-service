using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Oversight;
using SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetStagedFiles;
using SFA.DAS.ApplyService.Application.Email;
using SFA.DAS.ApplyService.Application.Organisations;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Application.Services;
using SFA.DAS.ApplyService.Application.Users;
using SFA.DAS.ApplyService.Application.Users.CreateAccount;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Data;
using SFA.DAS.ApplyService.Data.FileStorage;
using SFA.DAS.ApplyService.Data.Queries;
using SFA.DAS.ApplyService.Data.Repositories.UnitOfWorkRepositories;
using SFA.DAS.ApplyService.Data.UnitOfWork;
using SFA.DAS.ApplyService.DfeSignIn;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Services.Moderator;
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
    using SFA.DAS.ApplyService.Application.Behaviours;
    using SFA.DAS.ApplyService.Domain.Roatp;
    using SFA.DAS.ApplyService.EmailService;
    using SFA.DAS.ApplyService.EmailService.Infrastructure;
    using SFA.DAS.ApplyService.EmailService.Interfaces;
    using SFA.DAS.ApplyService.InternalApi.Models.Roatp;
    using SFA.DAS.ApplyService.InternalApi.Services;
    using SFA.DAS.ApplyService.InternalApi.Services.Assessor;
    using SFA.DAS.ApplyService.InternalApi.Services.Files;
    using SFA.DAS.Http;
    using SFA.DAS.Http.TokenGenerators;
    using SFA.DAS.Notifications.Api.Client;
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
                        },
                        OnAuthenticationFailed = c =>
                        {
                            return Task.CompletedTask;
                        }
                    };
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

            services.AddMvc(opt =>
            {
                if (_env.IsDevelopment())
                {
                    opt.Filters.Add(new AllowAnonymousFilter());
                }
            })
            .AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<GetStagedFilesQueryValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddOptions();

            services.Configure<List<RoatpSequences>>(_configuration.GetSection("RoatpSequences"));
            services.Configure<List<CriminalComplianceGatewayConfig>>(_configuration.GetSection("CriminalComplianceGatewayConfig"));
            services.Configure<List<CriminalComplianceGatewayOverrideConfig>>(_configuration.GetSection("SoleTraderCriminalComplianceGatewayOverrides"));

            services.AddDistributedMemoryCache();

            services.AddHealthChecks();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "SFA.DAS.ApplyService.InternalApi", Version = "v1" });
                c.CustomSchemaIds(x => x.FullName); // Fixes issue when the same type name appears twice
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

        private void ConfigHttpClients(IServiceCollection services)
        {
            var acceptHeaderName = "Accept";
            var acceptHeaderValue = "application/json";
            var handlerLifeTime = TimeSpan.FromMinutes(5);

            services.AddHttpClient<ProviderRegisterApiClient, ProviderRegisterApiClient>(config =>
            {
                config.BaseAddress = new Uri(_applyConfig.ProviderRegisterApiAuthentication.ApiBaseAddress);
                config.DefaultRequestHeaders.Add(acceptHeaderName, acceptHeaderValue);
            })
            .SetHandlerLifetime(handlerLifeTime);

            services.AddHttpClient<ReferenceDataApiClient, ReferenceDataApiClient>(config =>
            {
                config.BaseAddress = new Uri(_applyConfig.ReferenceDataApiAuthentication.ApiBaseAddress);
                config.DefaultRequestHeaders.Add(acceptHeaderName, acceptHeaderValue);
            })
            .SetHandlerLifetime(handlerLifeTime);

            services.AddHttpClient<CompaniesHouseApiClient, CompaniesHouseApiClient>(config =>
            {
                config.BaseAddress = new Uri(_applyConfig.CompaniesHouseApiAuthentication.ApiBaseAddress);
                config.DefaultRequestHeaders.Add(acceptHeaderName, acceptHeaderValue);
            })
            .SetHandlerLifetime(handlerLifeTime);

            services.AddHttpClient<IRoatpApiClient, RoatpApiClient>(config =>
            {
                config.BaseAddress = new Uri(_applyConfig.RoatpApiAuthentication.ApiBaseAddress);
                config.DefaultRequestHeaders.Add(acceptHeaderName, acceptHeaderValue);
            })
            .SetHandlerLifetime(handlerLifeTime);

            services.AddHttpClient<IInternalQnaApiClient, InternalQnaApiClient>(config =>
            {
                config.BaseAddress = new Uri(_applyConfig.QnaApiAuthentication.ApiBaseAddress);
                config.DefaultRequestHeaders.Add(acceptHeaderName, acceptHeaderValue);
            })
            .SetHandlerLifetime(handlerLifeTime);

            services.AddHttpClient<IEmailTemplateClient, EmailTemplateClient>(config =>
            {
                config.BaseAddress = new Uri(_applyConfig.InternalApi.Uri);
                config.DefaultRequestHeaders.Add(acceptHeaderName, acceptHeaderValue);
            })
            .SetHandlerLifetime(handlerLifeTime);
        }

        private void ConfigureDependencyInjection(IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IConfigurationService>(sp => new ConfigurationService(
                 sp.GetService<IHostingEnvironment>(),
                 _configuration["EnvironmentName"],
                 _configuration["ConfigurationStorageConnectionString"],
                 _version,
                 _serviceName));

            services.AddTransient<IContactRepository, ContactRepository>();
            services.AddTransient<IApplyRepository, ApplyRepository>();
            services.AddTransient<IAssessorRepository, AssessorRepository>();
            services.AddTransient<IOrganisationRepository, OrganisationRepository>();
            services.AddTransient<IModeratorRepository, ModeratorRepository>();
            services.AddTransient<IClarificationRepository, ClarificationRepository>();
            services.AddTransient<IDfeSignInService, DfeSignInService>();
            services.AddTransient<IOversightReviewRepository, OversightReviewRepository>();
            services.AddTransient<IOversightReviewQueries, OversightReviewQueries>();
            services.AddTransient<IAppealUploadRepository, AppealUploadRepository>();
            services.AddTransient<IAppealRepository, AppealRepository>();
            services.AddTransient<IAppealsQueries, AppealsQueries>();

            services.AddTransient<IEmailTemplateRepository, EmailTemplateRepository>();

            // NOTE: These are SOAP Services. Their client interfaces are contained within the generated Proxy code.
            services.AddTransient<CharityCommissionService.ISearchCharitiesV1SoapClient, CharityCommissionService.SearchCharitiesV1SoapClient>();
            services.AddTransient<CharityCommissionApiClient, CharityCommissionApiClient>();
            // End of SOAP Services

            services.AddTransient<IQnaTokenService, QnaTokenService>();
            services.AddTransient<IRoatpTokenService, RoatpTokenService>();
            services.AddTransient<IGatewayApiChecksService, GatewayApiChecksService>();
            services.AddTransient<ICriminalComplianceChecksQuestionLookupService, CriminalComplianceChecksQuestionLookupService>();
            services.AddTransient<IRegistrationDetailsService, RegistrationDetailsService>();
            services.AddTransient<IAssessorLookupService, AssessorLookupService>();
            services.AddTransient<IExtractAnswerValueService, ExtractAnswerValueService>();
            services.AddTransient<IAssessorSequenceService, AssessorSequenceService>();
            services.AddTransient<IAssessorPageService, AssessorPageService>();
            services.AddTransient<IAssessorSectorService, AssessorSectorService>();
            services.AddTransient<IAssessorSectorDetailsService, AssessorSectorDetailsService>();
            services.AddTransient<IAssessorReviewCreationService, AssessorReviewCreationService>();
            services.AddTransient<IModeratorReviewCreationService, ModeratorReviewCreationService>();

            services.AddTransient<IApplicationRepository, ApplicationRepository>();
            services.AddTransient<IDiffService, DiffService>();
            services.AddTransient<IStateService, StateService>();
            services.AddTransient<IAuditRepository, AuditRepository>();
            services.AddTransient<IAuditService, AuditService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IFileEncryptionService, FileEncryptionService>();
            services.AddTransient<IFileStorageService, FileStorageService>();
            services.AddTransient<IAppealsFileStorage, AppealsFileStorage>();
            services.AddTransient<IByteArrayEncryptionService, ByteArrayEncryptionService>();
            
            services.AddMediatR(typeof(CreateAccountHandler).GetTypeInfo().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            ConfigureNotificationApiEmailService(services);

            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(_applyConfig.FileStorage.StorageConnectionString);
            });
        }

        private void ConfigureNotificationApiEmailService(IServiceCollection services)
        {
            services.AddTransient<INotificationsApi>(x =>
            {
                var apiConfiguration = new Notifications.Api.Client.Configuration.NotificationsApiClientConfiguration
                {
                    ApiBaseUrl = _applyConfig.NotificationsApiClientConfiguration.ApiBaseUrl,
#pragma warning disable 618
                    ClientToken = _applyConfig.NotificationsApiClientConfiguration.ClientToken,
#pragma warning restore 618
                    ClientId = _applyConfig.NotificationsApiClientConfiguration.ClientId,
                    ClientSecret = _applyConfig.NotificationsApiClientConfiguration.ClientSecret,
                    IdentifierUri = _applyConfig.NotificationsApiClientConfiguration.IdentifierUri,
                    Tenant = _applyConfig.NotificationsApiClientConfiguration.Tenant
                };

                var httpClient = string.IsNullOrWhiteSpace(apiConfiguration.ClientId)
                    ? new HttpClientBuilder().WithBearerAuthorisationHeader(new JwtBearerTokenGenerator(apiConfiguration)).Build()
                    : new HttpClientBuilder().WithBearerAuthorisationHeader(new AzureADBearerTokenGenerator(apiConfiguration)).Build();

                return new NotificationsApi(httpClient, apiConfiguration);
            });

            services.AddTransient<IEmailTokenService, EmailTokenService>();
            services.AddTransient<IApplicationUpdatedEmailService, ApplicationUpdatedEmailService>();
        }
    }
}