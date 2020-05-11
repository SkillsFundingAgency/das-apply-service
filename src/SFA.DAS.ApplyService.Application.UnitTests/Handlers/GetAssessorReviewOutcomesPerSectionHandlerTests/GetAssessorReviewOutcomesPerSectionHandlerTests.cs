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

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetAssessorReviewOutcomesPerSectionHandlerTests
{
    [TestFixture]
    public class GetAssessorReviewOutcomesPerSectionHandlerTests
    {
        protected Mock<IAssessorRepository> _repository;
        protected GetAssessorReviewOutcomesPerSectionHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IAssessorRepository>();
            _handler = new GetAssessorReviewOutcomesPerSectionHandler(_repository.Object, Mock.Of<ILogger<GetAssessorReviewOutcomesPerSectionHandler>>());
        }

        [Test]
        public async Task GetAssessorReviewOutcomesPerSectionHandler_returns__List_of_PageReviewOutcome()
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

            _repository.Setup(x => x.GetAssessorReviewOutcomesPerSection(expectedApplicationId, expectedSequenceNumber, expectedSectionNumber,
                                                          expectedAssessorType, expectedUserId)).ReturnsAsync(expectedResult);

            var actualResult = await _handler.Handle(new GetAssessorReviewOutcomesPerSectionRequest(expectedApplicationId, expectedSequenceNumber, expectedSectionNumber,
                                                          expectedAssessorType, expectedUserId), new CancellationToken());

            Assert.AreSame(expectedResult, actualResult);
        }
    }
}
