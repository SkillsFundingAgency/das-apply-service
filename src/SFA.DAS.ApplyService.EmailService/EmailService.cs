using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Email;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.Http;
using SFA.DAS.Http.TokenGenerators;
using SFA.DAS.Notifications.Api.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotificationsApiClientConfiguration = SFA.DAS.Notifications.Api.Client.Configuration.NotificationsApiClientConfiguration;

namespace SFA.DAS.ApplyService.EmailService
{
    public class EmailService : IEmailService
    {
        private const string SYSTEM_ID = "ApplyService";
        private const string REPLY_TO_ADDRESS = "digital.apprenticeship.service @notifications.service.gov.uk";
        private const string SUBJECT = "Update on your EPAO application";

        private readonly ILogger<EmailService> _logger;
        private readonly IConfigurationService _configurationService;
        private readonly INotificationsApi _notificationsApi;
        private readonly IEmailTemplateRepository _emailTemplateRepository;

        public EmailService(ILogger<EmailService> logger, IConfigurationService configurationService, IEmailTemplateRepository emailTemplateRepository)
        {
            _logger = logger;
            _configurationService = configurationService;
            _emailTemplateRepository = emailTemplateRepository;
            _notificationsApi = SetupNotificationApi(); // Consider injecting this in constructor ??? 
        }

        public async Task SendEmail(string templateName, string toAddress, dynamic replacements)
        {
            var emailTemplate = await _emailTemplateRepository.GetEmailTemplate(templateName);

            if (emailTemplate != null)
            {
                var recipients = new List<string>();

                if (!string.IsNullOrWhiteSpace(toAddress))
                {
                    recipients.Add(toAddress.Trim());
                }

                if (!string.IsNullOrWhiteSpace(emailTemplate.Recipients))
                {
                    recipients.AddRange(emailTemplate.Recipients.Split(';').Select(x => x.Trim()));
                }

                var personalisationTokens = new Dictionary<string, string>();

                foreach (var property in replacements.GetType().GetProperties())
                {
                    personalisationTokens[property.Name] = property.GetValue(replacements);
                }

                foreach (var recipient in recipients)
                {
                    _logger.LogInformation($"Sending {templateName} email to {recipient}");
                    await SendEmailViaNotificationsApi(recipient, emailTemplate.TemplateId, personalisationTokens);
                }
            }
        }

        private async Task SendEmailViaNotificationsApi(string toAddress, string templateId, Dictionary<string, string> personalisationTokens)
        {
            // Note: It appears that if anything is hard copied in the template it'll ignore any values below
            var email = new Notifications.Api.Types.Email
            {
                RecipientsAddress = toAddress,
                TemplateId = templateId,
                ReplyToAddress = REPLY_TO_ADDRESS,
                Subject = SUBJECT,
                SystemId = SYSTEM_ID,
                Tokens = personalisationTokens
            };

            try
            {
                await _notificationsApi.SendEmail(email);
            }
            catch(System.Exception ex)
            {
                _logger.LogError(ex, $"Error sending email template {templateId} to {toAddress}");
            }
        }

        private INotificationsApi SetupNotificationApi()
        {
            var config = _configurationService.GetConfig().GetAwaiter().GetResult();

            var apiConfiguration = new NotificationsApiClientConfiguration
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
    }
}