using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Clarification;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetClarificationPageReviewOutcomeHandlerTests
{
    [TestFixture]
    public class GetClarificationPageReviewOutcomeHandlerTests
    {
        protected Mock<IClarificationRepository> _repository;
        protected GetClarificationPageReviewOutcomeHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IClarificationRepository>();
            _handler = new GetClarificationPageReviewOutcomeHandler(_repository.Object, Mock.Of<ILogger<GetClarificationPageReviewOutcomeHandler>>());
        }

        [Test]
        public async Task GetClarificationPageReviewOutcomeHandler_returns__PageReviewOutcome()
        {
            var expectedApplicationId = Guid.NewGuid();
            var expectedSequenceNumber = 1;
            var expectedSectionNumber = 2;
            var expectedPageId = "30";
            var expectedUserId = "4fs7f-userId-7gfhh";
            var expectedUserName = "Joe Cool";
            var expectedStatus = "Fail";
            var expectedComment = "Very bad";
            var expectedResponse = "A good response";

            var expectedResult = new ClarificationPageReviewOutcome
            {
                ApplicationId = expectedApplicationId,
                SequenceNumber = expectedSequenceNumber,
                SectionNumber = expectedSectionNumber,
                PageId = expectedPageId,
                UserId = expectedUserId,
                UserName = expectedUserName,
                Status = expectedStatus,
                Comment = expectedComment,
                ClarificationResponse = expectedResponse
            };

            _repository.Setup(x => x.GetClarificationPageReviewOutcome(expectedApplicationId, expectedSequenceNumber, expectedSectionNumber, 
                                                          expectedPageId)).ReturnsAsync(expectedResult);

            var actualResult = await _handler.Handle(new GetClarificationPageReviewOutcomeRequest(expectedApplicationId, expectedSequenceNumber, expectedSectionNumber,
                                                          expectedPageId, expectedUserId), new CancellationToken());

            Assert.AreSame(expectedResult, actualResult);
        }
    }
}
