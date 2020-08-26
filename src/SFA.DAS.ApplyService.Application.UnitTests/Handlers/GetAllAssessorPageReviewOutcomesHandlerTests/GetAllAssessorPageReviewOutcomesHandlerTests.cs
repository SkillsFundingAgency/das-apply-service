using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetAllAssessorPageReviewOutcomesHandlerTests
{
    [TestFixture]
    public class GetAllAssessorPageReviewOutcomesHandlerTests
    {
        protected Mock<IAssessorRepository> _repository;
        protected GetAllAssessorPageReviewOutcomesHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IAssessorRepository>();
            _handler = new GetAllAssessorPageReviewOutcomesHandler(_repository.Object, Mock.Of<ILogger<GetAllAssessorPageReviewOutcomesHandler>>());
        }

        [Test]
        public async Task GetAllAssessorReviewOutcomesHandler_returns__List_of_PageReviewOutcome()
        {
            var expectedApplicationId = Guid.NewGuid();
            var expectedSequenceNumber = 1;
            var expectedSectionNumber = 2;
            var expectedPageId = "30";
            var expectedUserId = "4fs7f-userId-7gfhh";
            var expectedStatus = "Fail";
            var expectedComment = "Very bad";

            var expectedResult = new List<AssessorPageReviewOutcome>
            {
                new AssessorPageReviewOutcome
                {
                    ApplicationId = expectedApplicationId,
                    SequenceNumber = expectedSequenceNumber,
                    SectionNumber = expectedSectionNumber,
                    PageId = expectedPageId,
                    UserId = expectedUserId,
                    Status = expectedStatus,
                    Comment = expectedComment
                }
            };

            _repository.Setup(x => x.GetAllAssessorPageReviewOutcomes(expectedApplicationId, expectedUserId)).ReturnsAsync(expectedResult);

            var actualResult = await _handler.Handle(new GetAllAssessorPageReviewOutcomesRequest(expectedApplicationId, expectedUserId), new CancellationToken());

            Assert.AreSame(expectedResult, actualResult);
        }
    }
}
