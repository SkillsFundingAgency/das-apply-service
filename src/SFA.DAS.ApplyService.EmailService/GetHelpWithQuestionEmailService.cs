using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Email;
using SFA.DAS.ApplyService.Application.Email.Consts;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.Http;
using SFA.DAS.Http.TokenGenerators;
using SFA.DAS.Notifications.Api.Client;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class GetHelpWithQuestionEmailService : IGetHelpWithQuestionEmailService
    {
        private readonly ILogger<GetHelpWithQuestionEmailService> _logger;
        private readonly IConfigurationService _configurationService;
        private readonly INotificationsApi _notificationsApi;
        private readonly IEmailTemplateRepository _emailTemplateRepository;

        private const string SYSTEM_ID = "ApplyService";

        public GetHelpWithQuestionEmailService(ILogger<GetHelpWithQuestionEmailService> logger, IConfigurationService configurationService, IEmailTemplateRepository emailTemplateRepository)
        {
            _logger = logger;
            _configurationService = configurationService;
            _emailTemplateRepository = emailTemplateRepository;
            _notificationsApi = SetupNotificationApi();
        }

        public async Task SendGetHelpWithQuestionEmail(GetHelpWithQuestion getHelpWithQuestion)
        {
            var emailTemplate = await _emailTemplateRepository.GetEmailTemplate(EmailTemplateName.ROATP_GET_HELP_WITH_QUESTION);

            var personalisationTokens = GetPersonalisationTokens(getHelpWithQuestion);

            await SendEmailViaNotificationsApi(emailTemplate.Recipients, emailTemplate.TemplateId, emailTemplate.TemplateName, getHelpWithQuestion.EmailAddress, personalisationTokens);
        }

        private Dictionary<string, string> GetPersonalisationTokens(GetHelpWithQuestion getHelpWithQuestion)
        {
            var personalisationTokens = new Dictionary<string, string>
            {
                { "email address", getHelpWithQuestion.EmailAddress },
                { "ApplicantFullName", getHelpWithQuestion.ApplicantFullName },
                { "UKPRN", getHelpWithQuestion.UKPRN },
                { "OrganisationName", getHelpWithQuestion.OrganisationName },
                { "ApplicationSequence", getHelpWithQuestion.ApplicationSequence },
                { "ApplicationSection", getHelpWithQuestion.ApplicationSection },
                { "PageTitle", getHelpWithQuestion.PageTitle },
                { "GetHelpQuery", getHelpWithQuestion.GetHelpQuery }
            };

            return personalisationTokens;
        }

        private INotificationsApi SetupNotificationApi()
        {
            var config = _configurationService.GetConfig().GetAwaiter().GetResult();

            var apiConfiguration = new Notifications.Api.Client.Configuration.NotificationsApiClientConfiguration
            {
                ApiBaseUrl = config.NotificationsApiClientConfiguration.ApiBaseUrl,
                ClientToken = config.NotificationsApiClientConfiguration.ClientToken,
                ClientId = config.NotificationsApiClientConfiguration.ClientId,
                ClientSecret = config.NotificationsApiClientConfiguration.ClientSecret,
                IdentifierUri = config.NotificationsApiClientConfiguration.IdentifierUri,
                Tenant = config.NotificationsApiClientConfiguration.Tenant
            };

            var httpClient = string.IsNullOrWhiteSpace(apiConfiguration.ClientId)
                ? new HttpClientBuilder().WithBearerAuthorisationHeader(new JwtBearerTokenGenerator(apiConfiguration)).Build()
                : new HttpClientBuilder().WithBearerAuthorisationHeader(new AzureADBearerTokenGenerator(apiConfiguration)).Build();

            return new NotificationsApi(httpClient, apiConfiguration);
        }

        private async Task SendEmailViaNotificationsApi(string toAddress, string templateId, string templateName, string replyToAddress, Dictionary<string, string> personalisationTokens)
        {
            var email = new Notifications.Api.Types.Email
            {
                RecipientsAddress = toAddress,
                TemplateId = templateId,
                ReplyToAddress = replyToAddress,
                SystemId = SYSTEM_ID,
                Tokens = personalisationTokens
            };

            try
            {
                _logger.LogInformation($"Sending {templateName} email ({templateId}) to {toAddress}");
                await _notificationsApi.SendEmail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending {templateName} email ({templateId}) to {toAddress}");
            }
        }
    }
}
