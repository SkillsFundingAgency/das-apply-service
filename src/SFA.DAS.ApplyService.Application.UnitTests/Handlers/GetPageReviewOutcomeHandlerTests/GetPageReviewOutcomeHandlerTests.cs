using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetPageReviewOutcomeHandlerTests
{
    [TestFixture]
    public class GetPageReviewOutcomeHandlerTests
    {
        protected Mock<IAssessorRepository> _repository;
        protected GetPageReviewOutcomeHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IAssessorRepository>();
            _handler = new GetPageReviewOutcomeHandler(_repository.Object, Mock.Of<ILogger<GetPageReviewOutcomeHandler>>());
        }

        [Test]
        public async Task GetPageReviewOutcomeHandler_returns__PageReviewOutcome()
        {
            var expectedApplicationId = Guid.NewGuid();
            var expectedSequenceNumber = 1;
            var expectedSectionNumber = 2;
            var expectedPageId = "30";
            var expectedAssessorType = 2;
            var expectedUserId = "4fs7f-userId-7gfhh";

            var expectedStatus = "Fail";
            var expectedComment = "Very bad";
  
            var expectedResult = new AssessorPageReviewOutcome 
            {
                ApplicationId = expectedApplicationId,
                SequenceNumber = expectedSequenceNumber,
                SectionNumber = expectedSectionNumber,
                PageId = expectedPageId,
                AssessorType = expectedAssessorType,
                UserId = expectedUserId,
                Status = expectedStatus,
                Comment = expectedComment
            };

            _repository.Setup(x => x.GetAssessorPageReviewOutcome(expectedApplicationId, expectedSequenceNumber, expectedSectionNumber, 
                                                          expectedPageId, expectedAssessorType, expectedUserId)).ReturnsAsync(expectedResult);

            var actualResult = await _handler.Handle(new GetPageReviewOutcomeRequest(expectedApplicationId, expectedSequenceNumber, expectedSectionNumber,
                                                          expectedPageId, expectedAssessorType, expectedUserId), new CancellationToken());

            Assert.AreSame(expectedResult, actualResult);
        }
    }
}
