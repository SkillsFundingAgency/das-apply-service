using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.EmailService.Interfaces;
using SFA.DAS.Notifications.Api.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.EmailService
{
    public abstract class NotificationApiEmailService
    {
        private const string SYSTEM_ID = "RoatpApplyService";
        protected virtual string REPLY_TO_ADDRESS => "the.apprenticeship.service@notifications.service.gov.uk";
        protected virtual string SUBJECT => "Update on your RoATP application";

        protected readonly ILogger<NotificationApiEmailService> _logger;
        protected readonly IEmailTemplateClient _emailTemplateClient;
        protected readonly INotificationsApi _notificationsApi;

        protected NotificationApiEmailService(ILogger<NotificationApiEmailService> logger, IEmailTemplateClient emailTemplateClient, INotificationsApi notificationsApi)
        {
            _logger = logger;
            _emailTemplateClient = emailTemplateClient;
            _notificationsApi = notificationsApi;  
        }

        public async Task SendEmail(string templateName, string toAddress, string replyToAddress,
                                    string subject, Dictionary<string, string> personalisationTokens)
        {
            var emailTemplate = await _emailTemplateClient.GetEmailTemplate(templateName);

            if (emailTemplate != null && !string.IsNullOrWhiteSpace(toAddress))
            {
                await SendEmailViaNotificationsApi(emailTemplate, toAddress, replyToAddress, subject, personalisationTokens);

                if (!string.IsNullOrWhiteSpace(emailTemplate.RecipientTemplate))
                {
                    await SendEmailToRecipients(emailTemplate.RecipientTemplate, personalisationTokens);
                }
            }
            else if (emailTemplate is null)
            {
                _logger.LogError($"Cannot find email template: {templateName}");
            }
            else
            {
                _logger.LogError($"Cannot send email template: {templateName} to: '{toAddress}'");
            }
        }

        private async Task SendEmailToRecipients(string recipientTemplateName, Dictionary<string, string> personalisationTokens)
        {
            var emailTemplate = await _emailTemplateClient.GetEmailTemplate(recipientTemplateName);

            if (emailTemplate != null && emailTemplate.Recipients != null)
            {
                var recipients = emailTemplate.Recipients.Split(';').Select(x => x.Trim());
                foreach (var recipientAddress in recipients)
                {
                    await SendEmailViaNotificationsApi(emailTemplate, emailTemplate.Recipients, null, null, personalisationTokens);
                }

                if (!string.IsNullOrWhiteSpace(emailTemplate.RecipientTemplate))
                {
                    await SendEmailToRecipients(emailTemplate.RecipientTemplate, personalisationTokens);
                }
            }
        }

        private async Task SendEmailViaNotificationsApi(EmailTemplate emailTemplate, string toAddress, string replyToAddress, 
                                                          string subject, Dictionary<string, string> personalisationTokens)
        {
            if (emailTemplate is null) return;

            try
            {
                // Note: If anything is hard copied in the template then it will ignore the respected value(s) below
                var email = new Notifications.Api.Types.Email
                {
                    RecipientsAddress = toAddress,
                    TemplateId = emailTemplate.TemplateId,
                    ReplyToAddress = replyToAddress ?? REPLY_TO_ADDRESS,
                    SystemId = SYSTEM_ID,
                    Subject = subject ?? SUBJECT,
                    Tokens = personalisationTokens
                };

                _logger.LogInformation($"Sending {emailTemplate.TemplateName} email ({emailTemplate.TemplateId}) to: '{toAddress}'");
                await _notificationsApi.SendEmail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending {emailTemplate.TemplateName} email ({emailTemplate.TemplateId}) to: '{toAddress}'");
            }
        }
    }
}
