using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Moderator;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.CreateModeratorPageReviewOutcomesHandlerTests
{
    [TestFixture]
    public class CreateModeratorPageReviewOutcomesHandlerTests
    {
        private Mock<IModeratorRepository> _repository;
        private CreateModeratorPageReviewOutcomesHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repository = new Mock<IModeratorRepository>();
            _handler = new CreateModeratorPageReviewOutcomesHandler(_repository.Object, Mock.Of<ILogger<CreateModeratorPageReviewOutcomesHandler>>());
        }

        [Test]
        public async Task CreateModeratorPageOutcomes_are_stored()
        {
            var request = new CreateModeratorPageReviewOutcomesRequest
            {
                ModeratorPageReviewOutcomes = new List<ModeratorPageReviewOutcome>
                {
                    new ModeratorPageReviewOutcome
                    {
                        ApplicationId = Guid.NewGuid(),
                        SequenceNumber = 1,
                        SectionNumber = 2,
                        Comment = string.Empty,
                        PageId = Guid.NewGuid().ToString(),
                        Status = string.Empty,
                        UserId = Guid.NewGuid().ToString()
                    },
                    new ModeratorPageReviewOutcome
                    {
                        ApplicationId = Guid.NewGuid(),
                        SequenceNumber = 3,
                        SectionNumber = 4,
                        Comment = string.Empty,
                        PageId = Guid.NewGuid().ToString(),
                        Status = string.Empty,
                        UserId = Guid.NewGuid().ToString()
                    }
                }
            };

            await _handler.Handle(request, new CancellationToken());

            _repository.Verify(x => x.CreateModeratorPageOutcomes(request.ModeratorPageReviewOutcomes), Times.Once);
        }
    }
}
