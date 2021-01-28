using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Oversight;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Types;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.OversightHandlerTests
{
    [TestFixture]
    public class RecordOversightGatewayFailOutcomeCommandHandlerTests
    {
        private RecordOversightGatewayFailOutcomeCommandHandler _handler;
        private Mock<IApplyRepository> _applyRepository;
        private Mock<IOversightReviewRepository> _oversightReviewRepository;
        private Mock<IAuditService> _auditService;

        [SetUp]
        public void SetUp()
        {
            _applyRepository = new Mock<IApplyRepository>();
            _oversightReviewRepository = new Mock<IOversightReviewRepository>();
            _auditService = new Mock<IAuditService>();

            _applyRepository.Setup(x => x.UpdateApplicationStatus(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            _oversightReviewRepository.Setup(x => x.Add(It.IsAny<OversightReview>())).Returns(Task.CompletedTask);

            _auditService.Setup(x => x.AuditInsert(It.IsAny<OversightReview>()));

            _handler = new RecordOversightGatewayFailOutcomeCommandHandler(_applyRepository.Object,
                _oversightReviewRepository.Object,
                Mock.Of<ILogger<RecordOversightGatewayFailOutcomeCommandHandler>>(),
                _auditService.Object);
        }

        [Test]
        public async Task Handle_Oversight_Outcome_Is_Added()
        {
            var request = new RecordOversightGatewayFailOutcomeCommand
            {
                ApplicationId = Guid.NewGuid(),
                UserId = "test user id",
                UserName = "test user name"
            };

            await _handler.Handle(request, new CancellationToken());

            _oversightReviewRepository.Verify(x => x.Add(It.Is<OversightReview>(r =>
                r.ApplicationId == request.ApplicationId &&
                r.Status == OversightReviewStatus.Unsuccessful &&
                r.UserId == request.UserId &&
                r.UserName == request.UserName)));
        }

        [Test]
        public async Task Handle_Oversight_Outcome_Insert_Is_Audited()
        {
            var request = new RecordOversightGatewayFailOutcomeCommand
            {
                ApplicationId = Guid.NewGuid(),
                UserId = "test user id",
                UserName = "test user name"
            };

            await _handler.Handle(request, new CancellationToken());

            _auditService.Verify(x => x.AuditInsert(It.Is<OversightReview>(r =>
                r.ApplicationId == request.ApplicationId &&
                r.Status == OversightReviewStatus.Unsuccessful &&
                r.UserId == request.UserId &&
                r.UserName == request.UserName)));
        }

        [Test]
        public async Task Handle_Application_Status_Is_Set_To_Rejected()
        {
            var request = new RecordOversightGatewayFailOutcomeCommand
            {
                ApplicationId = Guid.NewGuid(),
                UserId = "test user id",
                UserName = "test user name"
            };

            await _handler.Handle(request, new CancellationToken());

            _applyRepository.Verify(x => x.UpdateApplicationStatus(It.Is<Guid>(id => id == request.ApplicationId),
                It.Is<string>(status => status == ApplicationStatus.Rejected)));
        }
    }
}
