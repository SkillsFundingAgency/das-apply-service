using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Email.Consts;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.Notifications.Api.Client;

namespace SFA.DAS.ApplyService.EmailService
{
    public class SubmitApplicationConfirmationEmailService : NotificationApiEmailService, ISubmitApplicationConfirmationEmailService
    {
        private const string EmailSubject = "Application submitted – RoATP service team";
        private const string ReplyToAddress = "the.apprenticeship.service@notifications.service.gov.uk";

        private readonly IEmailTemplateClient _emailTemplateClient;

        public SubmitApplicationConfirmationEmailService(ILogger<NotificationApiEmailService> logger, IEmailTemplateClient emailTemplateClient,
                                                         INotificationsApi notificationsApi)
            : base(logger, notificationsApi)
        {
            _emailTemplateClient = emailTemplateClient;
        }

        public async Task SendGetHelpWithQuestionEmail(ApplicationSubmitConfirmation applicationSubmitConfirmation)
        {
            var templateName = EmailTemplateName.ROATP_APPLICATION_SUBMITTED;
            if (applicationSubmitConfirmation.ApplicationRouteId == ApplicationRoute.MainProviderApplicationRoute.ToString())
            {
                templateName = EmailTemplateName.ROATP_APPLICATION_SUBMITTED_MAIN;
            }
            
            var emailTemplate = await _emailTemplateClient.GetEmailTemplate(templateName);

            var personalisationTokens = GetPersonalisationTokens(applicationSubmitConfirmation);

            await SendEmailViaNotificationsApi(applicationSubmitConfirmation.EmailAddress, emailTemplate.TemplateId, emailTemplate.TemplateName,
                                               ReplyToAddress, EmailSubject, personalisationTokens);
        }

        private Dictionary<string, string> GetPersonalisationTokens(ApplicationSubmitConfirmation applicationSubmitConfirmation)
        {
            var personalisationTokens = new Dictionary<string, string>
            {
                { "ApplicantEmail", applicationSubmitConfirmation.EmailAddress },
                { "ApplicantFullName", applicationSubmitConfirmation.ApplicantFullName }
            };

            return personalisationTokens;
        }
    }
}
