using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Moderator;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.SubmitModeratorPageOutcomeHandlerTests
{
    [TestFixture]
    public class SubmitModeratorPageOutcomeHandlerTests
    {
        private Guid _applicationId;
        private int _sequenceNumber;
        private int _sectionNumber;
        private string _pageId;
        private string _userId;
        private string _userName;
        private string _status;
        private string _comment;

        private Mock<IApplyRepository> _applyRepository;
        private Mock<IModeratorRepository> _moderatorRepository;
        private SubmitModeratorPageOutcomeHandler _handler;

        [SetUp]
        public void Setup()
        {
            _applicationId = Guid.NewGuid();
            _sequenceNumber = 1;
            _sectionNumber = 2;
            _pageId = "30";
            _userId = "4fs7f-userId-7gfhh";
            _userName = "user-name";
            _status = "Fail";
            _comment = "Very bad";

            var logger = Mock.Of<ILogger<SubmitModeratorPageOutcomeHandler>>();
            _applyRepository = new Mock<IApplyRepository>();
            _moderatorRepository = new Mock<IModeratorRepository>();

            var applyData = new ApplyData { ModeratorReviewDetails = new ModeratorReviewDetails() };
            _applyRepository.Setup(x => x.GetApplyData(_applicationId)).ReturnsAsync(applyData);

            _handler = new SubmitModeratorPageOutcomeHandler(logger, _applyRepository.Object, _moderatorRepository.Object);
        }

        [Test]
        public async Task SubmitModeratorPageOutcome_is_stored()
        {
            var request = new SubmitModeratorPageOutcomeRequest(_applicationId, _sequenceNumber, _sectionNumber, _pageId, _userId, _userName, _status, _comment);
            await _handler.Handle(request, new CancellationToken());

            _moderatorRepository.Verify(x => x.SubmitModeratorPageOutcome(_applicationId, _sequenceNumber, _sectionNumber, _pageId, _userId, _userName, _status, _comment), Times.Once);
        }

        [Test]
        public async Task SubmitModeratorPageOutcome_ModeratorReviewDetails_are_updated()
        {
            var request = new SubmitModeratorPageOutcomeRequest(_applicationId, _sequenceNumber, _sectionNumber, _pageId, _userId, _userName, _status, _comment);
            await _handler.Handle(request, new CancellationToken());

            _moderatorRepository.Verify(x => x.UpdateModerationStatus(request.ApplicationId, It.IsAny<ApplyData>(), ModerationStatus.InProgress, request.UserId), Times.Once);
        }
    }
}
