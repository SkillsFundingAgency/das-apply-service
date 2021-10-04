using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.EmailService;
using SFA.DAS.ApplyService.EmailService.Interfaces;
using SFA.DAS.Notifications.Api.Client;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using SFA.DAS.ApplyService.Domain.Roatp;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests
{

    [TestFixture]
    public class RequestInvitationToReapplyEmailServiceTests
    {
        private Mock<ILogger<RequestInvitationToReapplyEmailService>> _logger;
        private Mock<INotificationsApi> _notificationsApi;
        private Mock<IEmailTemplateClient> _emailTemplateClient;
        private RequestInvitationToReapplyEmailService _service;

        [SetUp]
        public void Before_each_test()
        {
            _logger = new Mock<ILogger<RequestInvitationToReapplyEmailService>>();
            _notificationsApi = new Mock<INotificationsApi>();
            _emailTemplateClient = new Mock<IEmailTemplateClient>();        

            _service = new RequestInvitationToReapplyEmailService(_logger.Object, _emailTemplateClient.Object, _notificationsApi.Object);
        }

        [Test]
        public async Task Service_sends_email_to_notifications_api_with_tokens_for_template()
        {
            var emailTemplate = new EmailTemplate
            {
                TemplateId = Guid.NewGuid().ToString(),
                TemplateName = "Template Name",
                Recipients = "recipients@test.com"
            };

            _emailTemplateClient.Setup(x => x.GetEmailTemplate(It.IsAny<string>())).ReturnsAsync(emailTemplate).Verifiable();

            _notificationsApi.Setup(x => x.SendEmail(It.IsAny<Notifications.Api.Types.Email>())).Verifiable();

            var requestInvitationToReapply = new RequestInvitationToReapply()
            {
                EmailAddress = "test@test.com",
                OrganisationName = "Test Org",
                UKPRN = "10002000"
            };

            await _service.SendRequestToReapplyEmail(requestInvitationToReapply);

            _emailTemplateClient.VerifyAll();
            _notificationsApi.VerifyAll();
        }
    }
}
