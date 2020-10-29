using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.CreateEmptyAssessorReviewHandlerTests
{
    [TestFixture]
    public class CreateEmptyAssessorReviewHandlerTests
    {
        private Mock<IAssessorRepository> _repository;
        private CreateEmptyAssessorReviewHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repository = new Mock<IAssessorRepository>();
            _handler = new CreateEmptyAssessorReviewHandler(_repository.Object, Mock.Of<ILogger<CreateEmptyAssessorReviewHandler>>());
        }


        [Test]
        public async Task When_creating_empty_review_AssessorPageOutcomes_are_stored()
        {
            var applicationId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var userName = Guid.NewGuid().ToString();

            var pageReviewOutcomes = new List<AssessorPageReviewOutcome>
                {
                    new AssessorPageReviewOutcome
                    {
                        ApplicationId = Guid.NewGuid(),
                        SequenceNumber = 1,
                        SectionNumber = 2,
                        AssessorNumber = 1,
                        PageId = Guid.NewGuid().ToString(),
                        UserId = Guid.NewGuid().ToString()
                    },
                    new AssessorPageReviewOutcome
                    {
                        ApplicationId = Guid.NewGuid(),
                        SequenceNumber = 3,
                        SectionNumber = 4,
                        AssessorNumber = 2,
                        PageId = Guid.NewGuid().ToString(),
                        UserId = Guid.NewGuid().ToString()
                    }
                };

            var request = new CreateEmptyAssessorReviewRequest(applicationId, userId, userName, pageReviewOutcomes);

            await _handler.Handle(request, new CancellationToken());

            _repository.Verify(x => x.CreateEmptyAssessorReview(request.ApplicationId, request.AssessorUserId, request.AssessorUserName, request.PageReviewOutcomes), Times.Once);
        }
    }
}
