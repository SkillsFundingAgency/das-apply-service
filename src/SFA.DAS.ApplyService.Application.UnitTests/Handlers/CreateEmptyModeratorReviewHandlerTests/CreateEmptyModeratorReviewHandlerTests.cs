using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Moderator;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.CreateEmptyModeratorReviewHandlerTests
{
    [TestFixture]
    public class CreateEmptyModeratorReviewHandlerTests
    {
        private Guid _applicationId;
        private string _userId;
        private string _userName;
        private List<ModeratorPageReviewOutcome> _pageReviewOutcomes;

        private Mock<IApplyRepository> _applyRepository;
        private Mock<IModeratorRepository> _moderatorRepository;
        private CreateEmptyModeratorReviewHandler _handler;

        [SetUp]
        public void Setup()
        {
            _applicationId = Guid.NewGuid();
            _userId = Guid.NewGuid().ToString();
            _userName = Guid.NewGuid().ToString();
            _pageReviewOutcomes = new List<ModeratorPageReviewOutcome>
                {
                    new ModeratorPageReviewOutcome
                    {
                        ApplicationId = Guid.NewGuid(),
                        SequenceNumber = 1,
                        SectionNumber = 2,
                        PageId = Guid.NewGuid().ToString(),
                        UserId = Guid.NewGuid().ToString()
                    },
                    new ModeratorPageReviewOutcome
                    {
                        ApplicationId = Guid.NewGuid(),
                        SequenceNumber = 3,
                        SectionNumber = 4,
                        PageId = Guid.NewGuid().ToString(),
                        UserId = Guid.NewGuid().ToString()
                    }
                };

            var logger = Mock.Of<ILogger<CreateEmptyModeratorReviewHandler>>();
            _applyRepository = new Mock<IApplyRepository>();
            _moderatorRepository = new Mock<IModeratorRepository>();

            var applyData = new ApplyData { ModeratorReviewDetails = new ModeratorReviewDetails() };
            _applyRepository.Setup(x => x.GetApplyData(_applicationId)).ReturnsAsync(applyData);

            _handler = new CreateEmptyModeratorReviewHandler(logger, _applyRepository.Object, _moderatorRepository.Object);
        }

        [Test]
        public async Task When_creating_empty_review_ModeratorPageOutcomes_are_stored()
        {
            var request = new CreateEmptyModeratorReviewRequest(_applicationId, _userId, _userName, _pageReviewOutcomes);
            await _handler.Handle(request, new CancellationToken());

            _moderatorRepository.Verify(x => x.CreateEmptyModeratorReview(request.ApplicationId, request.ModeratorUserId, request.ModeratorUserName, request.PageReviewOutcomes), Times.Once);
        }

        [Test]
        public async Task When_creating_empty_review_ModeratorReviewDetails_are_updated()
        {
            var request = new CreateEmptyModeratorReviewRequest(_applicationId, _userId, _userName, _pageReviewOutcomes);
            await _handler.Handle(request, new CancellationToken());

            _moderatorRepository.Verify(x => x.UpdateModerationStatus(request.ApplicationId, It.IsAny<ApplyData>(), ModerationStatus.New, request.ModeratorUserId), Times.Once);
        }
    }
}
