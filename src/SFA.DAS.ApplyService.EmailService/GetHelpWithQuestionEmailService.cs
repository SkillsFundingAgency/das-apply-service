﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.EmailService.Consts;
using SFA.DAS.ApplyService.EmailService.Interfaces;
using SFA.DAS.Notifications.Api.Client;

namespace SFA.DAS.ApplyService.EmailService
{
    public class GetHelpWithQuestionEmailService : NotificationApiEmailService, IGetHelpWithQuestionEmailService
    {
        protected override string SUBJECT => "RoATP – Get help with this question";
        private string SUBJECT_CONFIRMATION => "Get help with this question – RoATP service team";

        public GetHelpWithQuestionEmailService(ILogger<NotificationApiEmailService> logger, IConfigurationService configurationService,
                                               IEmailTemplateClient emailTemplateClient, INotificationsApi notificationsApi)
            : base(logger, emailTemplateClient, notificationsApi) { }

        public async Task SendGetHelpWithQuestionEmail(GetHelpWithQuestion getHelpWithQuestion)
        {
            var emailTemplate = await _emailTemplateClient.GetEmailTemplate(EmailTemplateName.ROATP_GET_HELP_WITH_QUESTION);

            var personalisationTokens = GetPersonalisationTokens(getHelpWithQuestion);

            await SendEmail(emailTemplate.TemplateName, emailTemplate.Recipients, getHelpWithQuestion.EmailAddress, SUBJECT, personalisationTokens);
            await SendGetHelpWithQuestionConfirmationEmail(getHelpWithQuestion);
        }

        private async Task SendGetHelpWithQuestionConfirmationEmail(GetHelpWithQuestion getHelpWithQuestion)
        {
            var emailTemplate = await _emailTemplateClient.GetEmailTemplate(EmailTemplateName.ROATP_GET_HELP_WITH_QUESTION_CONFIRMATION);

            var personalisationTokens = GetPersonalisationTokens(getHelpWithQuestion);

            await SendEmail(emailTemplate.TemplateName, getHelpWithQuestion.EmailAddress, REPLY_TO_ADDRESS, SUBJECT_CONFIRMATION, personalisationTokens);
        }

        private Dictionary<string, string> GetPersonalisationTokens(GetHelpWithQuestion getHelpWithQuestion)
        {
            var personalisationTokens = new Dictionary<string, string>
            {
                { "ApplicantEmail", getHelpWithQuestion.EmailAddress },
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

    }
}
