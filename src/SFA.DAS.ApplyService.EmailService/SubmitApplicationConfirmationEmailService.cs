using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.EmailService.Consts;
using SFA.DAS.ApplyService.EmailService.Interfaces;
using SFA.DAS.Notifications.Api.Client;

namespace SFA.DAS.ApplyService.EmailService
{
    public class SubmitApplicationConfirmationEmailService : NotificationApiEmailService, ISubmitApplicationConfirmationEmailService
    {
        protected override string SUBJECT => "Application submitted – APAR service team";

        public SubmitApplicationConfirmationEmailService(ILogger<NotificationApiEmailService> logger, IEmailTemplateClient emailTemplateClient,
                                                         INotificationsApi notificationsApi)
            : base(logger, emailTemplateClient, notificationsApi) { }

        public async Task SendSubmitConfirmationEmail(ApplicationSubmitConfirmation applicationSubmitConfirmation)
        {
            var templateName = EmailTemplateName.ROATP_APPLICATION_SUBMITTED;

            var personalisationTokens = GetPersonalisationTokens(applicationSubmitConfirmation);

            await SendEmail(templateName, applicationSubmitConfirmation.EmailAddress, REPLY_TO_ADDRESS, SUBJECT, personalisationTokens);
        }

        private static Dictionary<string, string> GetPersonalisationTokens(ApplicationSubmitConfirmation applicationSubmitConfirmation)
        {
            var personalisationTokens = new Dictionary<string, string>
            {
                { "ApplicantEmail", applicationSubmitConfirmation.EmailAddress },
                { "ApplicantFullName", applicationSubmitConfirmation.ApplicantFullName },
                { "ApplicationReferenceNumber", applicationSubmitConfirmation.ApplicationReferenceNumber }
            };

            return personalisationTokens;
        }
    }
}
