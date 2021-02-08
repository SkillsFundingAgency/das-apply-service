using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Clarification;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetClarificationPageReviewOutcomesForSectionHandlerTests
{
    [TestFixture]
    public class GetClarificationPageReviewOutcomesForSectionHandlerTests
    {
        protected Mock<IClarificationRepository> _repository;
        protected GetClarificationPageReviewOutcomesForSectionHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IClarificationRepository>();
            _handler = new GetClarificationPageReviewOutcomesForSectionHandler(_repository.Object, Mock.Of<ILogger<GetClarificationPageReviewOutcomesForSectionHandler>>());
        }

        [Test]
        public async Task GetModeratorPageReviewOutcomesForSectionHandler_returns__List_of_PageReviewOutcome()
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

            var expectedResult = new List<ClarificationPageReviewOutcome> 
            { 
                new ClarificationPageReviewOutcome
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
                } 
            };

            _repository.Setup(x => x.GetClarificationPageReviewOutcomesForSection(expectedApplicationId, expectedSequenceNumber, expectedSectionNumber)).ReturnsAsync(expectedResult);

            var actualResult = await _handler.Handle(new GetClarificationPageReviewOutcomesForSectionRequest(expectedApplicationId, expectedSequenceNumber, expectedSectionNumber,
                                                          expectedUserId), new CancellationToken());

            Assert.AreSame(expectedResult, actualResult);
        }
    }
}
