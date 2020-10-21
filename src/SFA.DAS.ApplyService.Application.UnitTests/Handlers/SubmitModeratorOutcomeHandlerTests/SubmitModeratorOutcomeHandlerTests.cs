using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Moderator;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.SubmitModeratorOutcomeHandlerTests
{
    [TestFixture]
    public class SubmitModeratorOutcomeHandlerTests
    {
        private Mock<IApplyRepository> _applyRepository;
        private Mock<IModeratorRepository> _moderatorRepository;
        private SubmitModeratorOutcomeHandler _handler;
        private Guid _applicationId;
        private ApplyData _applyData;
        private SubmitModeratorOutcomeRequest _request;
        private string UserId => "user_id_1";
        private string UserName => "user name";
        private string Status => "status to store";
        private string Comment => "comment goes here";

        [SetUp]
        public void Setup()
        {
            _applicationId = Guid.NewGuid();
            _applyRepository = new Mock<IApplyRepository>();
            _request = new SubmitModeratorOutcomeRequest(_applicationId, UserId, UserName, Status, Comment);
            _applyData = new ApplyData {ModeratorReviewDetails = new ModeratorReviewDetails()};

            _moderatorRepository = new Mock<IModeratorRepository>();
            _handler = new SubmitModeratorOutcomeHandler(_applyRepository.Object, _moderatorRepository.Object, Mock.Of<ILogger<SubmitModeratorOutcomeHandler>>());
        }

        [Test]
        public async Task Submit_Moderator_Outcome_is_stored()
        {
            _applyRepository.Setup(x => x.GetApplyData(_applicationId)).ReturnsAsync(_applyData);
            
            _moderatorRepository.Setup(x => x.SubmitModeratorOutcome(_applicationId, It.IsAny<ApplyData>(), UserId, Status))
                .ReturnsAsync(true);
            var successfulSave = await _handler.Handle(_request, new CancellationToken());

             Assert.IsTrue(successfulSave);
            _moderatorRepository.Verify(x => x.SubmitModeratorOutcome(_applicationId, It.IsAny<ApplyData>(), UserId, Status), Times.Once);
        }
    }
}
