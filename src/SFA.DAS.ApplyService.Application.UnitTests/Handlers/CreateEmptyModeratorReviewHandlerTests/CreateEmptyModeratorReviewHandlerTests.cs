using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Moderator;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.CreateEmptyModeratorReviewHandlerTests
{
    [TestFixture]
    public class CreateEmptyModeratorReviewHandlerTests
    {
        private Mock<IModeratorRepository> _repository;
        private CreateEmptyModeratorReviewHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repository = new Mock<IModeratorRepository>();
            _handler = new CreateEmptyModeratorReviewHandler(_repository.Object, Mock.Of<ILogger<CreateEmptyModeratorReviewHandler>>());
        }

        [Test]
        public async Task When_creating_empty_review_ModeratorPageOutcomes_are_stored()
        {
            var applicationId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var userName = Guid.NewGuid().ToString();

            var pageReviewOutcomes = new List<ModeratorPageReviewOutcome>
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

            var request = new CreateEmptyModeratorReviewRequest(applicationId, userId, userName, pageReviewOutcomes);

            await _handler.Handle(request, new CancellationToken());

            _repository.Verify(x => x.CreateEmptyModeratorReview(request.ApplicationId, request.ModeratorUserId, request.ModeratorUserName, request.PageReviewOutcomes), Times.Once);
        }
    }
}
