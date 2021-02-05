using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.EmailService.Consts;
using SFA.DAS.ApplyService.EmailService.Interfaces;
using SFA.DAS.Notifications.Api.Client;
using SFA.DAS.Notifications.Api.Types;

namespace SFA.DAS.ApplyService.EmailService.UnitTests
{
    public class ApplicationUpdatedEmailServiceTests
    {
        private ApplicationUpdatedEmailService _service;
        private readonly Guid _applicationId = Guid.NewGuid();

        private Mock<INotificationsApi> _notificationApi;
        private Mock<IApplyRepository> _applyRepository;
        private Mock<IConfigurationService> _configurationService;
        private Mock<IEmailTemplateClient> _emailTemplateClient;

        private readonly string _recipientEmail = "test@tester.com";
        private readonly string _recipientFirstName = "test-first-name";
        private readonly string _recipientLastName = "test-last-name";

        [SetUp]
        public void Setup()
        {
            _notificationApi = new Mock<INotificationsApi>();
            _notificationApi.Setup(x => x.SendEmail(It.IsAny<Email>())).Returns(Task.CompletedTask);

            _applyRepository = new Mock<IApplyRepository>();
            _applyRepository.Setup(x => x.GetContactForApplication(_applicationId)).ReturnsAsync(new Contact
            {
                Email = _recipientEmail,
                GivenNames = _recipientFirstName,
                FamilyName = _recipientLastName
            });

            _emailTemplateClient = new Mock<IEmailTemplateClient>();
            _emailTemplateClient.Setup(x => x.GetEmailTemplate(EmailTemplateName.ROATP_APPLICATION_UPDATED))
                .ReturnsAsync(new EmailTemplate());

            _configurationService = new Mock<IConfigurationService>();
            _configurationService.Setup(x => x.GetConfig()).ReturnsAsync(new ApplyConfig { SignInPage = "baseurl"});

            _service = new ApplicationUpdatedEmailService(
                Mock.Of<ILogger<ApplicationUpdatedEmailService>>(),
                _emailTemplateClient.Object,
                _notificationApi.Object,
                _applyRepository.Object,
                _configurationService.Object);
        }

        [Test]
        public async Task SendEmail_Sends_Email_To_Contact()
        {
            await _service.SendEmail(_applicationId);

            _notificationApi.Verify(x =>
                x.SendEmail(It.Is<Email>(email =>
                    email.RecipientsAddress == _recipientEmail
                    && email.Tokens["ApplicantFullName"] == $"{_recipientFirstName} {_recipientLastName}")));
        }
    }
}