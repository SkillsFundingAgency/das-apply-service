using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.EmailService.Consts;
using SFA.DAS.ApplyService.EmailService.Interfaces;
using SFA.DAS.Notifications.Api.Client;

namespace SFA.DAS.ApplyService.EmailService
{
    public class RequestInvitationToReapplyEmailService : NotificationApiEmailService, IRequestInvitationToReapplyEmailService
    {
        protected override string SUBJECT => "Applicant request for invitation to reapply";

        public RequestInvitationToReapplyEmailService(ILogger<NotificationApiEmailService> logger, IEmailTemplateClient emailTemplateClient,
                                                         INotificationsApi notificationsApi)
            : base(logger, emailTemplateClient, notificationsApi) { }

        public async Task SendRequestToReapplyEmail(RequestInvitationToReapply requestInvitationToReapply)
        {
            var templateName = EmailTemplateName.ROATP_REQUEST_INVITATION_TO_REAPPLY;
            var emailTemplate = await _emailTemplateClient.GetEmailTemplate(templateName);

            var personalisationTokens = GetPersonalisationTokens(requestInvitationToReapply);

            await SendEmail(emailTemplate.TemplateName, emailTemplate.Recipients, requestInvitationToReapply.EmailAddress, SUBJECT, personalisationTokens);
        }

        private static Dictionary<string, string> GetPersonalisationTokens(RequestInvitationToReapply requestInvitationToReapply)
        {
            var personalisationTokens = new Dictionary<string, string>
            {
                { "ApplicantEmail", requestInvitationToReapply.EmailAddress },
                { "UKPRN", requestInvitationToReapply.UKPRN },
                { "OrganisationName", requestInvitationToReapply.OrganisationName },

            };

            return personalisationTokens;
        }
    }
}
