using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Oversight;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Data.UnitOfWork;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.EmailService.Interfaces;
using SFA.DAS.ApplyService.Types;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.OversightHandlerTests
{
    [TestFixture]
    public class RecordOversightGatewayRemovedOutcomeCommandHandlerTests
    {
        private RecordOversightGatewayRemovedOutcomeCommandHandler _handler;
        private Mock<IApplyRepository> _applyRepository;
        private Mock<IOversightReviewRepository> _oversightReviewRepository;
        private Mock<IAuditService> _auditService;
        private Mock<IApplicationUpdatedEmailService> _applicationUpdatedEmailService;
        private Guid _applicationId;

        [SetUp]
        public void SetUp()
        {
            _applicationId = Guid.NewGuid();
            _applyRepository = new Mock<IApplyRepository>();
            _oversightReviewRepository = new Mock<IOversightReviewRepository>();
            _auditService = new Mock<IAuditService>();
            _applicationUpdatedEmailService = new Mock<IApplicationUpdatedEmailService>();

            _applyRepository.Setup(x => x.GetApplication(_applicationId)).ReturnsAsync(() => new Domain.Entities.Apply
                { ApplicationId = _applicationId, Status = ApplicationStatus.Submitted });

            _applyRepository.Setup(x => x.UpdateApplication(It.IsAny<Domain.Entities.Apply>())).Returns(Task.CompletedTask);
            _oversightReviewRepository.Setup(x => x.Add(It.IsAny<OversightReview>()));

            _auditService.Setup(x => x.AuditInsert(It.IsAny<OversightReview>()));

            _applicationUpdatedEmailService.Setup(x => x.SendEmail(_applicationId)).Returns(Task.CompletedTask);

            _handler = new RecordOversightGatewayRemovedOutcomeCommandHandler(_applyRepository.Object,
                _oversightReviewRepository.Object,
                Mock.Of<ILogger<RecordOversightGatewayRemovedOutcomeCommandHandler>>(),
                _auditService.Object,
                _applicationUpdatedEmailService.Object,
                Mock.Of<IUnitOfWork>());
        }

        [Test]
        public async Task Handle_Oversight_Outcome_Is_Added()
        {
            var request = new RecordOversightGatewayRemovedOutcomeCommand
            {
                ApplicationId = _applicationId,
                UserId = "test user id",
                UserName = "test user name"
            };

            await _handler.Handle(request, new CancellationToken());

            _oversightReviewRepository.Verify(x => x.Add(It.Is<OversightReview>(r =>
                r.ApplicationId == request.ApplicationId &&
                r.Status == OversightReviewStatus.Removed &&
                r.UserId == request.UserId &&
                r.UserName == request.UserName)));
        }

        [Test]
        public async Task Handle_Oversight_Outcome_Insert_Is_Audited()
        {
            var request = new RecordOversightGatewayRemovedOutcomeCommand
            {
                ApplicationId = _applicationId,
                UserId = "test user id",
                UserName = "test user name"
            };

            await _handler.Handle(request, new CancellationToken());

            _auditService.Verify(x => x.AuditInsert(It.Is<OversightReview>(r =>
                r.ApplicationId == request.ApplicationId &&
                r.Status == OversightReviewStatus.Removed &&
                r.UserId == request.UserId &&
                r.UserName == request.UserName)));
        }

        [Test]
        public async Task Handle_User_Is_Notified_Of_Update_To_Application()
        {
            var request = new RecordOversightGatewayRemovedOutcomeCommand
            {
                ApplicationId = _applicationId,
                UserId = "test user id",
                UserName = "test user name"
            };

            await _handler.Handle(request, new CancellationToken());

            _applicationUpdatedEmailService.Verify(x => x.SendEmail(_applicationId), Times.Once);
        }
    }
}
