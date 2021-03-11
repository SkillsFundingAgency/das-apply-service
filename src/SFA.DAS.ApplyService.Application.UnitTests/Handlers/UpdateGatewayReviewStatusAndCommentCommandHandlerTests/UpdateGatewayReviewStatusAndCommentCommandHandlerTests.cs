using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Gateway;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Data.UnitOfWork;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.EmailService.Interfaces;
using SFA.DAS.ApplyService.Types;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.UpdateGatewayReviewStatusAndCommentCommandHandlerTests
{
    [TestFixture]
    public class UpdateGatewayReviewStatusAndCommentCommandHandlerTests
    {
        private UpdateGatewayReviewStatusAndCommentCommandHandler _handler;
        private Mock<IApplyRepository> _applyRepository;
        private Mock<IOversightReviewRepository> _oversightReviewRepository;
        private Mock<IApplicationUpdatedEmailService> _applicationUpdatedEmailService;
        private const string _userName = "John Smith";
        private const string _userId = "user id 123";
        private Guid _applicationId;
        private Domain.Entities.Apply _application;

        [SetUp]
        public void SetUp()
        {
            _applicationId = Guid.NewGuid();

            _application = new Domain.Entities.Apply
            {
                ApplicationId = _applicationId,
                GatewayReviewStatus = GatewayReviewStatus.New,
                ApplyData = new ApplyData()
            };

            _applyRepository = new Mock<IApplyRepository>();
            _applyRepository.Setup(x => x.GetApplication(_applicationId)).ReturnsAsync(() => _application);

            _oversightReviewRepository = new Mock<IOversightReviewRepository>();
            _oversightReviewRepository.Setup(x => x.Add(It.IsAny<OversightReview>()));

            _applicationUpdatedEmailService = new Mock<IApplicationUpdatedEmailService>();

            _handler = new UpdateGatewayReviewStatusAndCommentCommandHandler(_applyRepository.Object,
                _oversightReviewRepository.Object,
                Mock.Of<IAuditService>(),
                _applicationUpdatedEmailService.Object,
                Mock.Of<IUnitOfWork>());
        }

        [Test]
        public async Task UpdateGatewayReviewStatusAndComment_updates_reviewstatus()
        {
            var gatewayReviewStatus = GatewayReviewStatus.Pass;
            var gatewayReviewComment = "Some comment";
            var gatewayReviewExternalComment = "Some external comment";

            _applyRepository.Setup(x => x.UpdateGatewayReviewStatusAndComment(_applicationId, It.IsAny<ApplyData>(), gatewayReviewStatus, _userId, _userName)).ReturnsAsync(true);

            await _handler.Handle(new UpdateGatewayReviewStatusAndCommentCommand(_applicationId, gatewayReviewStatus,
                gatewayReviewComment, gatewayReviewExternalComment, _userId, _userName), CancellationToken.None);

            _applyRepository.Verify(x => x.UpdateGatewayReviewStatusAndComment(_applicationId, It.IsAny<ApplyData>(), gatewayReviewStatus, _userId, _userName), Times.Once);
        }

        [Test]
        public async Task UpdateGatewayReviewStatusAndComment_when_rejected_creates_oversight_review()
        {
            var gatewayReviewStatus = GatewayReviewStatus.Reject;
            var gatewayReviewComment = "Some comment";
            var gatewayReviewExternalComment = "Some external comment";

            _applyRepository.Setup(x => x.UpdateGatewayReviewStatusAndComment(_applicationId, It.IsAny<ApplyData>(), gatewayReviewStatus, _userId, _userName)).ReturnsAsync(true);

            var request = new UpdateGatewayReviewStatusAndCommentCommand(_applicationId, gatewayReviewStatus, gatewayReviewComment, gatewayReviewExternalComment, _userId, _userName);

            await _handler.Handle(request, CancellationToken.None);

            _oversightReviewRepository.Verify(x => x.Add(It.Is<OversightReview>(r => r.ApplicationId == _applicationId && r.Status == OversightReviewStatus.Rejected)));
        }

        [Test]
        public async Task Handler_sends_email_when_application_rejected()
        {
            var gatewayReviewStatus = GatewayReviewStatus.Reject;
            var gatewayReviewComment = "Some comment";
            var gatewayReviewExternalComment = "Some external comment";

            _applyRepository.Setup(x => x.UpdateGatewayReviewStatusAndComment(_applicationId, It.IsAny<ApplyData>(), gatewayReviewStatus, _userId, _userName)).ReturnsAsync(true);

            var request = new UpdateGatewayReviewStatusAndCommentCommand(_applicationId, gatewayReviewStatus, gatewayReviewComment, gatewayReviewExternalComment, _userId, _userName);

            await _handler.Handle(request, CancellationToken.None);

            _applicationUpdatedEmailService.Verify(x => x.SendEmail(It.Is<Guid>(id => id == _applicationId)), Times.Once);
        }

        [TestCase(GatewayReviewStatus.Pass)]
        [TestCase(GatewayReviewStatus.Fail)]
        [TestCase(GatewayReviewStatus.InProgress)]
        [TestCase(GatewayReviewStatus.New)]
        [TestCase(GatewayReviewStatus.Resubmitted)]
        [TestCase(GatewayReviewStatus.ClarificationSent)]
        [TestCase(GatewayReviewStatus.Draft)]
        public async Task Handler_does_not_send_email_when_application_not_rejected(string status)
        {
            var gatewayReviewStatus = status;
            var gatewayReviewComment = "Some comment";
            var gatewayReviewExternalComment = "Some external comment";

            _applyRepository.Setup(x => x.UpdateGatewayReviewStatusAndComment(_applicationId, It.IsAny<ApplyData>(), gatewayReviewStatus, _userId, _userName)).ReturnsAsync(true);

            var request = new UpdateGatewayReviewStatusAndCommentCommand(_applicationId, gatewayReviewStatus, gatewayReviewComment, gatewayReviewExternalComment, _userId, _userName);

            await _handler.Handle(request, CancellationToken.None);

            _applicationUpdatedEmailService.Verify(x => x.SendEmail(It.Is<Guid>(id => id == _applicationId)), Times.Never);
        }
    }
}
