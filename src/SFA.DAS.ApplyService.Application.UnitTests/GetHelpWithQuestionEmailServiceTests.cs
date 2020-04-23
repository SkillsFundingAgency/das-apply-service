using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.EmailService;
using SFA.DAS.Notifications.Api.Client;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using SFA.DAS.ApplyService.Domain.Roatp;

namespace SFA.DAS.ApplyService.Application.UnitTests
{

    [TestFixture]
    public class GetHelpWithQuestionEmailServiceTests
    {
        private Mock<ILogger<GetHelpWithQuestionEmailService>> _logger;
        private Mock<IConfigurationService> _config;
        private Mock<INotificationsApi> _notificationsApi;
        private Mock<IEmailTemplateClient> _emailTemplateClient;
        private GetHelpWithQuestionEmailService _service;

        [SetUp]
        public void Before_each_test()
        {
            _logger = new Mock<ILogger<GetHelpWithQuestionEmailService>>();
            _config = new Mock<IConfigurationService>();
            _notificationsApi = new Mock<INotificationsApi>();
            _emailTemplateClient = new Mock<IEmailTemplateClient>();        

            _service = new GetHelpWithQuestionEmailService(_logger.Object, _config.Object, _emailTemplateClient.Object, _notificationsApi.Object);
        }

        [Test]
        public void Service_sends_email_to_notifications_api_with_tokens_for_template()
        {
            var emailTemplate = new EmailTemplate
            {
                TemplateId = Guid.NewGuid().ToString(),
                TemplateName = "Template Name"
            };

            _emailTemplateClient.Setup(x => x.GetEmailTemplate(It.IsAny<string>())).ReturnsAsync(emailTemplate).Verifiable();

            _notificationsApi.Setup(x => x.SendEmail(It.IsAny<Notifications.Api.Types.Email>())).Verifiable();

            var getHelpWithQuestion = new GetHelpWithQuestion
            {
                ApplicantFullName = "Mrs Test Person",
                ApplicationSection = "Your org",
                ApplicationSequence = "Preamble",
                EmailAddress = "test@test.com",
                GetHelpQuery = "Help! I need somebody",
                OrganisationName = "Test Org",
                PageTitle = "Provider route",
                UKPRN = "10002000"
            };

            _service.SendGetHelpWithQuestionEmail(getHelpWithQuestion).GetAwaiter().GetResult();

            _emailTemplateClient.VerifyAll();
            _notificationsApi.VerifyAll();
        }
    }
}
