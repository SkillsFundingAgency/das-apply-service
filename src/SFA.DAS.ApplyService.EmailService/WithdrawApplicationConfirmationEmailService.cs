using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.EmailService.Consts;
using SFA.DAS.ApplyService.EmailService.Interfaces;
using SFA.DAS.Notifications.Api.Client;

namespace SFA.DAS.ApplyService.EmailService
{
    public class WithdrawApplicationConfirmationEmailService : NotificationApiEmailService, IWithdrawApplicationConfirmationEmailService
    {
        protected override string SUBJECT => "Application withdrawn – RoATP service team";

        public WithdrawApplicationConfirmationEmailService(ILogger<NotificationApiEmailService> logger, IEmailTemplateClient emailTemplateClient,
                                                         INotificationsApi notificationsApi)
            : base(logger, emailTemplateClient, notificationsApi) { }

        public async Task SendWithdrawConfirmationEmail(ApplicationWithdrawConfirmation applicationWithdrawConfirmation)
        {
            var templateName = EmailTemplateName.ROATP_APPLICATION_WITHDRAWN;
            
            var personalisationTokens = GetPersonalisationTokens(applicationWithdrawConfirmation);

            await SendEmail(templateName, applicationWithdrawConfirmation.EmailAddress, REPLY_TO_ADDRESS, SUBJECT, personalisationTokens);
        }

        private static Dictionary<string, string> GetPersonalisationTokens(ApplicationWithdrawConfirmation applicationWithdrawConfirmation)
        {
            var personalisationTokens = new Dictionary<string, string>
            {
                { "ApplicantEmail", applicationWithdrawConfirmation.EmailAddress },
                { "ApplicantFullName", applicationWithdrawConfirmation.ApplicantFullName },
                { "LoginLink", applicationWithdrawConfirmation.LoginLink },
            };

            return personalisationTokens;
        }
    }
}
