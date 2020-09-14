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

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.CreateAssessorPageReviewOutcomesHandlerTests
{
    [TestFixture]
    public class CreateAssessorPageReviewOutcomesHandlerTests
    {
        private Mock<IAssessorRepository> _repository;
        private CreateAssessorPageReviewOutcomesHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repository = new Mock<IAssessorRepository>();
            _handler = new CreateAssessorPageReviewOutcomesHandler(_repository.Object, Mock.Of<ILogger<CreateAssessorPageReviewOutcomesHandler>>());
        }


        [Test]
        public async Task CreateAssessorPageOutcomes_are_stored()
        {
            var request = new CreateAssessorPageReviewOutcomesRequest
            {
                AssessorPageReviewOutcomes = new List<AssessorPageReviewOutcome>
                {
                    new AssessorPageReviewOutcome
                    {
                        ApplicationId = Guid.NewGuid(),
                        SequenceNumber = 1,
                        SectionNumber = 2,
                        AssessorNumber = 1,
                        Comment = string.Empty,
                        PageId = Guid.NewGuid().ToString(),
                        Status = string.Empty,
                        UserId = Guid.NewGuid().ToString()
                    },
                    new AssessorPageReviewOutcome
                    {
                        ApplicationId = Guid.NewGuid(),
                        SequenceNumber = 3,
                        SectionNumber = 4,
                        AssessorNumber = 2,
                        Comment = string.Empty,
                        PageId = Guid.NewGuid().ToString(),
                        Status = string.Empty,
                        UserId = Guid.NewGuid().ToString()
                    }
                }
            };

            await _handler.Handle(request, new CancellationToken());

            _repository.Verify(x => x.CreateAssessorPageOutcomes(request.AssessorPageReviewOutcomes), Times.Once);
        }
    }
}
