using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.UpdateAssessorReviewStatusHandlerTests
{
    [TestFixture]
    public class UpdateAssessorReviewStatusHandlerTests
    {
        private Mock<IAssessorRepository> _repository;
        private UpdateAssessorReviewStatusHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IAssessorRepository>();
            _handler = new UpdateAssessorReviewStatusHandler(_repository.Object, Mock.Of<ILogger<UpdateAssessorReviewStatusHandler>>());
        }

        [Test]
        public async Task UpdateAssessorReviewStatus_is_stored()
        {
            var applicationId = Guid.NewGuid();
            var userId = "4fs7f-userId-7gfhh";
            var status = "Approved";

            await _handler.Handle(new UpdateAssessorReviewStatusRequest(applicationId, userId, status), new CancellationToken());

            _repository.Verify(x => x.UpdateAssessorReviewStatus(applicationId, userId, status), Times.Once);
        }
    }
}
