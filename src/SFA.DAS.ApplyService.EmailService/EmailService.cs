using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Email;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.Http;
using SFA.DAS.Http.TokenGenerators;
using SFA.DAS.Notifications.Api.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotificationsApiClientConfiguration = SFA.DAS.Notifications.Api.Client.Configuration.NotificationsApiClientConfiguration;

namespace SFA.DAS.ApplyService.EmailService
{
    public class EmailService : IEmailService
    {
        private const string SYSTEM_ID = "ApplyService";
        private const string REPLY_TO_ADDRESS = "digital.apprenticeship.service@notifications.service.gov.uk";
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
            _notificationsApi = SetupNotificationApi(); 
        }

        private Dictionary<string, string> GetPersonalisationTokens(dynamic tokens)
        {
            var personalisationTokens = new Dictionary<string, string>();

            if (tokens != null)
            {
                foreach (var property in tokens.GetType().GetProperties())
                {
                    personalisationTokens[property.Name] = property.GetValue(tokens);
                }
            }

            return personalisationTokens;
        }

        private async Task SendEmailToRecipients(IEnumerable<string> recipients, string templateName, dynamic tokens)
        {
            if (recipients != null)
            {
                var emailTemplate = await _emailTemplateRepository.GetEmailTemplate(templateName);

                if (emailTemplate != null)
                {
                    var personalisationTokens = GetPersonalisationTokens(tokens);

                    foreach (var recipient in recipients)
                    {
                        await SendEmailViaNotificationsApi(recipient, emailTemplate.TemplateId, emailTemplate.TemplateName, personalisationTokens);
                    }

                    var rcpList = string.IsNullOrWhiteSpace(emailTemplate.Recipients) ? null : emailTemplate.Recipients.Split(';').Select(x => x.Trim());
                    await SendEmailToRecipients(rcpList, emailTemplate.RecipientTemplate, tokens);
                }
            }
        }

        public async Task SendEmail(string templateName, string toAddress, dynamic tokens)
        {
            var emailTemplate = await _emailTemplateRepository.GetEmailTemplate(templateName);

            if (emailTemplate != null && !string.IsNullOrWhiteSpace(toAddress))
            {
                var personalisationTokens = GetPersonalisationTokens(tokens);

                await SendEmailViaNotificationsApi(toAddress, emailTemplate.TemplateId, emailTemplate.TemplateName, personalisationTokens);

                var rcpList = string.IsNullOrWhiteSpace(emailTemplate.Recipients) ? null : emailTemplate.Recipients.Split(';').Select(x => x.Trim());
                await SendEmailToRecipients(rcpList, emailTemplate.RecipientTemplate, tokens);
            }
            else if(emailTemplate is null)
            {
                _logger.LogError($"Cannot find email template {emailTemplate}");
            }
            else
            {
                _logger.LogError($"Cannot send email template {emailTemplate} to '{toAddress}'");
            }
        }

        public async Task SendEmailToContact(string templateName, Contact contact, dynamic tokens)
        {
            await SendEmailToContacts(templateName, new List<Contact> { contact }, tokens);
        }

        public async Task SendEmailToContacts(string templateName, IEnumerable<Contact> contacts, dynamic tokens)
        {
            var emailTemplate = await _emailTemplateRepository.GetEmailTemplate(templateName);

            if (emailTemplate != null)
            {
                foreach(var contact in contacts?.Where(c => !string.IsNullOrWhiteSpace(c.Email)))
                {
                    var personalisationTokens = GetPersonalisationTokens(tokens);
                    personalisationTokens["contactname"] = $"{contact.GivenNames} {contact.FamilyName}";

                    await SendEmailViaNotificationsApi(contact.Email, emailTemplate.TemplateId, emailTemplate.TemplateName, personalisationTokens);
                }

                var rcpList = string.IsNullOrWhiteSpace(emailTemplate.Recipients) ? null : emailTemplate.Recipients.Split(';').Select(x => x.Trim());
                await SendEmailToRecipients(rcpList, emailTemplate.RecipientTemplate, tokens);
            }
            else
            {
                _logger.LogError($"Cannot find email template {emailTemplate}");
            }
        }

        private async Task SendEmailViaNotificationsApi(string toAddress, string templateId, string templateName, Dictionary<string, string> personalisationTokens)
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
                _logger.LogInformation($"Sending {templateName} email ({templateId}) to {toAddress}");
                await _notificationsApi.SendEmail(email);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error sending {templateName} email ({templateId}) to {toAddress}");
            }
        }

        private INotificationsApi SetupNotificationApi()
        {
            // Note: This is placed here as the types used pollute the DI setup.
            // Also there wasn't any nice way to create this as an StructureMap Registry
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