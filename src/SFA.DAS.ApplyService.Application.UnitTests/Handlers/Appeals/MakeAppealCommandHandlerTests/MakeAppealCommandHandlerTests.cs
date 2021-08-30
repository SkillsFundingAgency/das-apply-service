using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Appeals.Commands.MakeAppeal;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Types;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.Appeals.MakeAppealCommandHandlerTests
{
    [TestFixture]
    public class MakeAppealCommandHandlerTests
    {
        private MakeAppealCommandHandler _handler;
        private Mock<IAppealRepository> _appealRepository;
        private Mock<IAuditService> _auditService;
        private Mock<IOversightReviewRepository> _oversightReviewRepository;

        private MakeAppealCommand _command;
        private readonly Guid _applicationId = Guid.NewGuid();
        private OversightReview _oversightReview;

        [SetUp]
        public void SetUp()
        {
            _oversightReview = new OversightReview
            {
                ApplicationId = _applicationId,
                Status = OversightReviewStatus.Unsuccessful
            };

            _oversightReviewRepository = new Mock<IOversightReviewRepository>();
            _oversightReviewRepository.Setup(x => x.GetByApplicationId(_applicationId)).ReturnsAsync(() => _oversightReview);

            _command = new MakeAppealCommand
            {
                ApplicationId = _applicationId,
                HowFailedOnPolicyOrProcesses = "howFailedOnPolicyOrProcesses",
                HowFailedOnEvidenceSubmitted = "howFailedOnEvidenceSubmitted",
                UserId = "userId",
                UserName = "userName"
            };

            _appealRepository = new Mock<IAppealRepository>();
            _appealRepository.Setup(x => x.Add(It.IsAny<Appeal>()));

            _auditService = new Mock<IAuditService>();
            _auditService.Setup(x => x.StartTracking(UserAction.UploadAppealFile, _command.UserId, _command.UserName));
            _auditService.Setup(x => x.AuditInsert(It.IsAny<AppealFile>()));

            _handler = new MakeAppealCommandHandler(_oversightReviewRepository.Object, _appealRepository.Object, _auditService.Object);
        }

        [Test]
        public async Task Handle_Adds_Appeal()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _appealRepository.Verify(
                x => x.Add(It.Is<Appeal>(appeal =>
                    appeal.ApplicationId == _applicationId &&
                    appeal.HowFailedOnPolicyOrProcesses == _command.HowFailedOnPolicyOrProcesses &&
                    appeal.HowFailedOnEvidenceSubmitted == _command.HowFailedOnEvidenceSubmitted &&
                    appeal.UserId == _command.UserId &&
                    appeal.UserName == _command.UserName)));
        }

        [Test]
        public async Task Handle_Audits_New_Appeal()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _auditService.Verify(x=> x.StartTracking(UserAction.MakeAppeal, _command.UserId, _command.UserName), Times.Once);

            _auditService.Verify(x => x.AuditInsert(It.Is<Appeal>(appeal =>
                appeal.ApplicationId == _applicationId &&
                appeal.HowFailedOnPolicyOrProcesses == _command.HowFailedOnPolicyOrProcesses &&
                appeal.HowFailedOnEvidenceSubmitted == _command.HowFailedOnEvidenceSubmitted &&
                appeal.UserId == _command.UserId &&
                appeal.UserName == _command.UserName)));
        }

        [TestCase(OversightReviewStatus.InProgress)]
        [TestCase(OversightReviewStatus.Rejected)]
        [TestCase(OversightReviewStatus.Successful)]
        [TestCase(OversightReviewStatus.SuccessfulAlreadyActive)]
        [TestCase(OversightReviewStatus.SuccessfulFitnessForFunding)]
        [TestCase(OversightReviewStatus.Withdrawn)]
        public void Handle_Throws_If_Oversight_Review_Is_Not_Unsuccessful_or_Removed(OversightReviewStatus status)
        {
            _oversightReview.Status = status;
            Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(_command, CancellationToken.None));
        }

        [TestCase(AppealStatus.Submitted)]
        [TestCase(AppealStatus.InProgressOutcome)]
        [TestCase(AppealStatus.Successful)]
        [TestCase(AppealStatus.SuccessfulAlreadyActive)]
        [TestCase(AppealStatus.SuccessfulFitnessForFunding)]
        [TestCase(AppealStatus.Unsuccessful)]
        public void Handle_Throws_If_Application_Already_Been_Appealed(AppealStatus status)
        {
            _appealRepository.Setup(x => x.GetByApplicationId(_applicationId))
                .ReturnsAsync(() => new Appeal { Status = status });

            Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(_command, CancellationToken.None));
        }
    }
}
