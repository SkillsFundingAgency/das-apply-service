using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Gateway.ApplicationActions;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.EmailService.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.Gateway.ApplicationActions
{
    [TestFixture]
    public class WithdrawApplicationHandlerTests
    {
        private WithdrawApplicationHandler _handler;
        private Mock<IApplyRepository> _repository;
        private Mock<IApplicationUpdatedEmailService> _emailService;

        private Guid _applicationId;
        private const string _comments = "comments";
        private const string _userId = "userId";
        private const string _userName = "_userName";

        [SetUp]
        public void SetUp()
        {
            _applicationId = Guid.NewGuid();

            _repository = new Mock<IApplyRepository>();
            _emailService = new Mock<IApplicationUpdatedEmailService>();
            var logger = Mock.Of<ILogger<WithdrawApplicationHandler>>();
            
            _handler = new WithdrawApplicationHandler(_repository.Object, logger, _emailService.Object);
        }

        [Test]
        public async Task Handler_withdraws_application()
        {
            await _handler.Handle(new WithdrawApplicationRequest(_applicationId, _comments, _userId, _userName), CancellationToken.None);
            _repository.Verify(x => x.WithdrawApplication(_applicationId, _comments, _userId, _userName), Times.Once);
        }

        [Test]
        public async Task Handler_sends_updated_email()
        {
            _repository.Setup(x => x.WithdrawApplication(_applicationId, _comments, _userId, _userName)).Returns(Task.FromResult(true));

            await _handler.Handle(new WithdrawApplicationRequest(_applicationId, _comments, _userId, _userName), CancellationToken.None);

            _emailService.Verify(x => x.SendEmail(It.Is<Guid>(id => id == _applicationId)), Times.Once);
        }
    }
}
