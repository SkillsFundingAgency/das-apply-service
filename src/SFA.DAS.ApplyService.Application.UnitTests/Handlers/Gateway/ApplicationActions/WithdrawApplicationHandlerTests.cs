using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Gateway.ApplicationActions;
using SFA.DAS.ApplyService.Application.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.Gateway.ApplicationActions
{
    public class WithdrawApplicationHandlerTests
    {
        private WithdrawApplicationHandler _handler;
        private Mock<IApplyRepository> _applyRepository;
        private Mock<IOversightReviewRepository> _oversightReviewRepository;
        private Mock<IAuditService> _auditService;

        private Guid _applicationId;
        private const string _comments = "comments";
        private const string _userId = "userId";
        private const string _userName = "_userName";

        [SetUp]
        public void SetUp()
        {
            _applicationId = Guid.NewGuid();

            _applyRepository = new Mock<IApplyRepository>();
            _oversightReviewRepository = new Mock<IOversightReviewRepository>();
            _auditService = new Mock<IAuditService>();
            var logger = Mock.Of<ILogger<WithdrawApplicationHandler>>();
            
            _handler = new WithdrawApplicationHandler(_applyRepository.Object, _oversightReviewRepository.Object, _auditService.Object, logger);
        }

        [Test]
        public async Task Handler_withdraws_application()
        {
            await _handler.Handle(new WithdrawApplicationRequest(_applicationId, _comments, _userId, _userName), CancellationToken.None);
            _applyRepository.Verify(x => x.WithdrawApplication(_applicationId, _comments, _userId, _userName), Times.Once);
        }
    }
}
