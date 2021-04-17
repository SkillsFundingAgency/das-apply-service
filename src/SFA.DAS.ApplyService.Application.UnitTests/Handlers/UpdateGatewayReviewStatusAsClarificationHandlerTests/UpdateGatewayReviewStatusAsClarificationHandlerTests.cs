using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Application.Apply.Gateway;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.UpdateGatewayReviewStatusAsClarificationHandlerTests
{
    [TestFixture]
    public class UpdateGatewayReviewStatusAsClarificationHandlerTests
    {
        private Mock<IApplyRepository> _repository;
        private Mock<IGatewayRepository> _gatewayRepository;
        private UpdateGatewayReviewStatusAsClarificationHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IApplyRepository>();
            _gatewayRepository = new Mock<IGatewayRepository>();
            _handler = new UpdateGatewayReviewStatusAsClarificationHandler(_repository.Object, _gatewayRepository.Object);

            _gatewayRepository.Setup(x => x.UpdateGatewayReviewStatusAndComment(It.IsAny<Guid>(), It.IsAny<ApplyData>(), 
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
        }


        [Test]
        public async Task UpdateReviewStatus_to_clarification_where_no_application_returns_false()
        {
            var applicationId = Guid.NewGuid();
            var userId = "4fs7f-userId-7gfhh";
            var userName = "joe";

            _repository.Setup(x => x.GetApplication(applicationId)).ReturnsAsync((Domain.Entities.Apply)null);
            var result = await _handler.Handle(new UpdateGatewayReviewStatusAsClarificationRequest(applicationId, userId, userName), new CancellationToken());

            Assert.IsFalse(result);
            _gatewayRepository.Verify(x => x.UpdateGatewayReviewStatusAndComment(applicationId,It.IsAny<Domain.Entities.ApplyData>(), It.IsAny<string>(), It.IsAny<string>(),It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task UpdateReviewStatus_to_clarification_returns_true()
        {
            var applicationId = Guid.NewGuid();
            var userId = "4fs7f-userId-7gfhh";
            var userName = "janet";

            _repository.Setup(x => x.GetApplication(applicationId)).ReturnsAsync( new Domain.Entities.Apply {ApplicationId = applicationId});
            var result = await _handler.Handle(new UpdateGatewayReviewStatusAsClarificationRequest(applicationId, userId, userName), new CancellationToken());

            Assert.IsTrue(result);
            _gatewayRepository.Verify(x => x.UpdateGatewayReviewStatusAndComment(applicationId, It.IsAny<Domain.Entities.ApplyData>(), GatewayReviewStatus.ClarificationSent, userId, userName), Times.Once);
        }
    }
}
