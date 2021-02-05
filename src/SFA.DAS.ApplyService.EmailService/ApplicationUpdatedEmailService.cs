using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.EmailService.Consts;
using SFA.DAS.ApplyService.EmailService.Interfaces;
using SFA.DAS.Notifications.Api.Client;

namespace SFA.DAS.ApplyService.EmailService
{
    public class ApplicationUpdatedEmailService : NotificationApiEmailService, IApplicationUpdatedEmailService
    {
        protected override string SUBJECT => "Update to your application – RoATP service team";
        private readonly IApplyRepository _applyRepository;
        private readonly IConfigurationService _configurationService;

        public ApplicationUpdatedEmailService(ILogger<NotificationApiEmailService> logger,
            IEmailTemplateClient emailTemplateClient,
            INotificationsApi notificationsApi,
            IApplyRepository applyRepository,
            IConfigurationService configurationService)
            : base(logger, emailTemplateClient, notificationsApi)
        {
            _applyRepository = applyRepository;
            _configurationService = configurationService;
        }

        public async Task SendEmail(Guid applicationId)
        {
            var config = await _configurationService.GetConfig();
            var contact = await _applyRepository.GetContactForApplication(applicationId);

            var tokens = new Dictionary<string, string>
            {
                { "ApplicantEmail", contact.Email },
                { "ApplicantFullName", $"{contact.GivenNames} {contact.FamilyName}" },
                { "LoginLink", config.SignInPage },
            };

            await SendEmail(EmailTemplateName.ROATP_APPLICATION_UPDATED, contact.Email, REPLY_TO_ADDRESS, SUBJECT, tokens);
        }
    }
}

