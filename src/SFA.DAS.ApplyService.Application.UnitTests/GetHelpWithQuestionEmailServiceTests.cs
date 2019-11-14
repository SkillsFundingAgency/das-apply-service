using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.EmailService;
using SFA.DAS.ApplyService.Application.Email;
using SFA.DAS.Notifications.Api.Client;
using SFA.DAS.ApplyService.Configuration;

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
    }
}
