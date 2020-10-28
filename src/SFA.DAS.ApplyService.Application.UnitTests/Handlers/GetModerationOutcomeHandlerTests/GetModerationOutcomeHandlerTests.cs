using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Clarification;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetModerationOutcomeHandlerTests
{
    [TestFixture]
    public class GetModerationOutcomeHandlerTests
    {
        protected Mock<IClarificationRepository> _repository;
        protected GetModerationOutcomeHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IClarificationRepository>();
            _handler = new GetModerationOutcomeHandler(_repository.Object, Mock.Of<ILogger<GetModerationOutcomeHandler>>());
        }

        [Test]
        public async Task GetBlindAssessmentOutcomeHandler_returns__BlindAssessmentOutcome()
        {
            var applicationId = Guid.NewGuid();
            var sequenceNumber = 1;
            var sectionNumber = 2;
            var pageId = "30";

            var expectedResult = new ModerationOutcome
            {
                ApplicationId = applicationId,
                SequenceNumber = sectionNumber,
                SectionNumber = sectionNumber,
                PageId = pageId,
                ModeratorName = "Moderator",
                ModeratorUserId = "Moderator",
                ModeratorReviewStatus = "Fail",
                ModeratorReviewComment = "Not a good answer"
            };

            _repository.Setup(x => x.GetModerationOutcome(applicationId, sequenceNumber, sectionNumber, pageId)).ReturnsAsync(expectedResult);

            var actualResult = await _handler.Handle(new GetModerationOutcomeRequest(applicationId, sequenceNumber, sectionNumber, pageId), new CancellationToken());

            Assert.AreSame(expectedResult, actualResult);
        }
    }
}
