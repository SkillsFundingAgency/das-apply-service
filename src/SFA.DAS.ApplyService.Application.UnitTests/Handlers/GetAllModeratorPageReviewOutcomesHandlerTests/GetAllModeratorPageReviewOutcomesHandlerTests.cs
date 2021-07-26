using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Moderator;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetAllModeratorPageReviewOutcomesHandlerTests
{
    [TestFixture]
    public class GetAllModeratorPageReviewOutcomesHandlerTests
    {
        protected Mock<IModeratorRepository> _repository;
        protected GetAllModeratorPageReviewOutcomesHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IModeratorRepository>();
            _handler = new GetAllModeratorPageReviewOutcomesHandler(_repository.Object, Mock.Of<ILogger<GetAllModeratorPageReviewOutcomesHandler>>());
        }

        [Test]
        public async Task GetAllModeratorReviewOutcomesHandler_returns__List_of_PageReviewOutcome()
        {
            var expectedApplicationId = Guid.NewGuid();
            var expectedSequenceNumber = 1;
            var expectedSectionNumber = 2;
            var expectedPageId = "30";
            var expectedUserId = "4fs7f-userId-7gfhh";
            var expectedStatus = "Fail";
            var expectedComment = "Very bad";

            var expectedResult = new List<ModeratorPageReviewOutcome>
            {
                new ModeratorPageReviewOutcome
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

            _repository.Setup(x => x.GetAllModeratorPageReviewOutcomes(expectedApplicationId)).ReturnsAsync(expectedResult);

            var actualResult = await _handler.Handle(new GetAllModeratorPageReviewOutcomesRequest(expectedApplicationId, expectedUserId), new CancellationToken());

            Assert.AreSame(expectedResult, actualResult);
        }
    }
}
