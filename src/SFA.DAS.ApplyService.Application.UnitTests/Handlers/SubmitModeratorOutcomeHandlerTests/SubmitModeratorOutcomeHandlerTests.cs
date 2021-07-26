using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Moderator;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.SubmitModeratorOutcomeHandlerTests
{
    [TestFixture]
    public class SubmitModeratorOutcomeHandlerTests
    {
        private Guid _applicationId;
        private string _userId;
        private string _userName;
        private string _status;
        private string _comment;

        private Mock<IApplyRepository> _applyRepository;
        private Mock<IModeratorRepository> _moderatorRepository;
        private SubmitModeratorOutcomeHandler _handler;

        [SetUp]
        public void Setup()
        {
            _applicationId = Guid.NewGuid();
            _userId = "user_id_1";
            _userName = "user name";
            _status = "Approved";
            _comment = "comment goes here";

            var logger = Mock.Of<ILogger<SubmitModeratorOutcomeHandler>>();
            _applyRepository = new Mock<IApplyRepository>();
            _moderatorRepository = new Mock<IModeratorRepository>();

            _handler = new SubmitModeratorOutcomeHandler(logger, _applyRepository.Object, _moderatorRepository.Object);
        }

        [Test]
        public async Task SubmitModeratorOutcome_is_stored()
        {
            var applyData = new ApplyData { ModeratorReviewDetails = new ModeratorReviewDetails() };
            _applyRepository.Setup(x => x.GetApplyData(_applicationId)).ReturnsAsync(applyData);

            _moderatorRepository.Setup(x => x.UpdateModerationStatus(_applicationId, It.IsAny<ApplyData>(), _status, _userId))
                .ReturnsAsync(true);

            var request = new SubmitModeratorOutcomeRequest(_applicationId, _userId, _userName, _status, _comment);
            var successfulSave = await _handler.Handle(request, new CancellationToken());

            Assert.IsTrue(successfulSave);
            _moderatorRepository.Verify(x => x.UpdateModerationStatus(_applicationId, It.IsAny<ApplyData>(), _status, _userId), Times.Once);
        }
    }
}
