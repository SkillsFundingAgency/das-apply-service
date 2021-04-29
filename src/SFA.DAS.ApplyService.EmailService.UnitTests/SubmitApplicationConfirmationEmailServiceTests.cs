using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.EmailService.Consts;
using SFA.DAS.ApplyService.EmailService.Interfaces;
using SFA.DAS.Notifications.Api.Client;
using SFA.DAS.Notifications.Api.Types;

namespace SFA.DAS.ApplyService.EmailService.UnitTests
{
    public class SubmitApplicationConfirmationEmailServiceTests
    {
        private SubmitApplicationConfirmationEmailService _service;

        private Mock<INotificationsApi> _notificationApi;
        private Mock<IEmailTemplateClient> _emailTemplateClient;

        private readonly string _recipientEmail = "test@tester.com";
        private readonly string _recipientFirstName = "test-first-name";
        private readonly string _recipientLastName = "test-last-name";
        private readonly string _applicationReferenceNumber = "APR-123456789";

        [SetUp]
        public void Setup()
        {
            _notificationApi = new Mock<INotificationsApi>();
            _notificationApi.Setup(x => x.SendEmail(It.IsAny<Email>())).Returns(Task.CompletedTask);

            _emailTemplateClient = new Mock<IEmailTemplateClient>();
            _emailTemplateClient.Setup(x => x.GetEmailTemplate(EmailTemplateName.ROATP_APPLICATION_SUBMITTED))
                .ReturnsAsync(new EmailTemplate());

            _service = new SubmitApplicationConfirmationEmailService(
                Mock.Of<ILogger<SubmitApplicationConfirmationEmailService>>(),
                _emailTemplateClient.Object,
                _notificationApi.Object);
        }

        [Test]
        public async Task SendSubmitConfirmationEmail_Sends_Email_To_Contact()
        {
            var confirmationRequest = new ApplicationSubmitConfirmation
            {
                ApplicantFullName = $"{_recipientFirstName} {_recipientLastName}",
                EmailAddress = _recipientEmail,
                ApplicationReferenceNumber = _applicationReferenceNumber,
            };

            await _service.SendSubmitConfirmationEmail(confirmationRequest);

            _notificationApi.Verify(x =>
                x.SendEmail(It.Is<Email>(email =>
                    email.RecipientsAddress == _recipientEmail
                    && email.Tokens["ApplicantFullName"] == $"{_recipientFirstName} {_recipientLastName}"
                    && email.Tokens["ApplicationReferenceNumber"] == _applicationReferenceNumber
                    )));
        }
    }
}