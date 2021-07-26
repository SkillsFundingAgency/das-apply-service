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
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetAssessorPageReviewOutcomesForSectionHandlerTests
{
    [TestFixture]
    public class GetAssessorPageReviewOutcomesForSectionHandlerTests
    {
        protected Mock<IAssessorRepository> _repository;
        protected GetAssessorPageReviewOutcomesForSectionHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IAssessorRepository>();
            _handler = new GetAssessorPageReviewOutcomesForSectionHandler(_repository.Object, Mock.Of<ILogger<GetAssessorPageReviewOutcomesForSectionHandler>>());
        }

        [Test]
        public async Task GetAssessorPageReviewOutcomesForSectionHandler_returns__List_of_PageReviewOutcome()
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

            _repository.Setup(x => x.GetAssessorPageReviewOutcomesForSection(expectedApplicationId, expectedSequenceNumber, expectedSectionNumber,
                                                          expectedUserId)).ReturnsAsync(expectedResult);

            var actualResult = await _handler.Handle(new GetAssessorPageReviewOutcomesForSectionRequest(expectedApplicationId, expectedSequenceNumber, expectedSectionNumber,
                                                          expectedUserId), new CancellationToken());

            Assert.AreSame(expectedResult, actualResult);
        }
    }
}
