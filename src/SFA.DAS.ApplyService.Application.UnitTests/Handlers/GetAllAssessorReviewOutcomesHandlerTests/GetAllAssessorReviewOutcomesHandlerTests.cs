using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetAllAssessorReviewOutcomesHandlerTests
{
    [TestFixture]
    public class GetAllAssessorReviewOutcomesHandlerTests
    {
        protected Mock<IApplyRepository> _repository;
        protected GetAllAssessorReviewOutcomesHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IApplyRepository>();
            _handler = new GetAllAssessorReviewOutcomesHandler(_repository.Object, Mock.Of<ILogger<GetAllAssessorReviewOutcomesHandler>>());
        }

        [Test]
        public async Task GetAllAssessorReviewOutcomesHandler_returns__List_of_PageReviewOutcome()
        {
            var expectedApplicationId = Guid.NewGuid();
            var expectedSequenceNumber = 1;
            var expectedSectionNumber = 2;
            var expectedPageId = "30";
            var expectedAssessorType = 2;
            var expectedUserId = "4fs7f-userId-7gfhh";

            var expectedStatus = "Fail";
            var expectedComment = "Very bad";

            var expectedResult = new List<PageReviewOutcome>
            {
                new PageReviewOutcome
                {
                    ApplicationId = expectedApplicationId,
                    SequenceNumber = expectedSequenceNumber,
                    SectionNumber = expectedSectionNumber,
                    PageId = expectedPageId,
                    AssessorType = expectedAssessorType,
                    UserId = expectedUserId,
                    Status = expectedStatus,
                    Comment = expectedComment
                }
            };

            _repository.Setup(x => x.GetAllAssessorReviewOutcomes(expectedApplicationId, expectedAssessorType, expectedUserId)).ReturnsAsync(expectedResult);

            var actualResult = await _handler.Handle(new GetAllAssessorReviewOutcomesRequest(expectedApplicationId, 
                                                                                                    expectedAssessorType, expectedUserId), new CancellationToken());

            Assert.AreSame(expectedResult, actualResult);
        }
    }
}
