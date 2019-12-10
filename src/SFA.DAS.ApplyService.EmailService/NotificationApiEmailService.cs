using Microsoft.Extensions.Logging;
using SFA.DAS.Notifications.Api.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.EmailService
{
    public abstract class NotificationApiEmailService
    {
        protected const string SYSTEM_ID = "RoatpApplyService";

        protected readonly ILogger<NotificationApiEmailService> _logger;
        protected readonly INotificationsApi _notificationsApi;

        protected NotificationApiEmailService(ILogger<NotificationApiEmailService> logger, INotificationsApi notificationsApi)
        {
            _logger = logger;
            _notificationsApi = notificationsApi;
        }

        protected async Task SendEmailViaNotificationsApi(string toAddress, string templateId, string templateName, string replyToAddress, 
                                                          string emailSubject, Dictionary<string, string> personalisationTokens)
        {
            var email = new Notifications.Api.Types.Email
            {
                RecipientsAddress = toAddress,
                TemplateId = templateId,
                ReplyToAddress = replyToAddress,
                SystemId = SYSTEM_ID,
                Subject = emailSubject,
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
