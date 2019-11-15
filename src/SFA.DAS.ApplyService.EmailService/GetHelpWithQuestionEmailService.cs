using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Email.Consts;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.Notifications.Api.Client;

namespace SFA.DAS.ApplyService.EmailService
{
    public class GetHelpWithQuestionEmailService : IGetHelpWithQuestionEmailService
    {
        private readonly ILogger<GetHelpWithQuestionEmailService> _logger;
        private readonly IConfigurationService _configurationService;
        private readonly INotificationsApi _notificationsApi;
        private readonly IEmailTemplateClient _emailTemplateClient;

        private const string SYSTEM_ID = "RoatpApplyService";
        private const string EmailSubject = "RoATP – Get help with this question";

        public GetHelpWithQuestionEmailService(ILogger<GetHelpWithQuestionEmailService> logger, IConfigurationService configurationService,
                                               IEmailTemplateClient emailTemplateClient, INotificationsApi notificationsApi)
        {
            _logger = logger;
            _configurationService = configurationService;
            _emailTemplateClient = emailTemplateClient;
            _notificationsApi = notificationsApi;
        }

        public async Task SendGetHelpWithQuestionEmail(GetHelpWithQuestion getHelpWithQuestion)
        {
            var emailTemplate = await _emailTemplateClient.GetEmailTemplate(EmailTemplateName.ROATP_GET_HELP_WITH_QUESTION);

            var personalisationTokens = GetPersonalisationTokens(getHelpWithQuestion);

            await SendEmailViaNotificationsApi(emailTemplate.Recipients, emailTemplate.TemplateId, emailTemplate.TemplateName, getHelpWithQuestion.EmailAddress, personalisationTokens);
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

        private async Task SendEmailViaNotificationsApi(string toAddress, string templateId, string templateName, string replyToAddress, Dictionary<string, string> personalisationTokens)
        {
            var email = new Notifications.Api.Types.Email
            {
                RecipientsAddress = toAddress,
                TemplateId = templateId,
                ReplyToAddress = replyToAddress,
                SystemId = SYSTEM_ID,
                Subject = EmailSubject,
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
