using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Gateway.ApplicationActions;
using SFA.DAS.ApplyService.Application.Apply.Gateway.Applications;
using SFA.DAS.ApplyService.InternalApi.Controllers;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Controllers
{
    [TestFixture]
    public class GatewayReviewControllerTests
    {
        private GatewayReviewController _controller;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void TestSetup()
        {
            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.IsAny<GetGatewayApplicationCountsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new GetGatewayApplicationCountsResponse
                {
                    NewApplicationsCount = 1,
                    InProgressApplicationsCount = 2,
                     ClosedApplicationsCount = 3
                });

            _controller = new GatewayReviewController(_mediator.Object);
        }

        [Test]
        public async Task Get_ApplicationCounts_returns_expected_counts_for_the_user()
        {
            var result = await _controller.GetApplicationCounts();

            Assert.AreEqual(1, result.NewApplicationsCount);
            Assert.AreEqual(2, result.InProgressApplicationsCount);
            Assert.AreEqual(3, result.ClosedApplicationsCount);
        }

        [Test]
        public async Task WithdrawApplication_calls_mediator()
        {
            var applicationId = Guid.NewGuid();

            var request = new GatewayWithdrawApplicationRequest
            {
                UserId = "userId",
                UserName = "userName",
                Comments = "comments"
            };

            await _controller.WithdrawApplication(applicationId, request);

            _mediator.Verify(x => x.Send(It.Is<WithdrawApplicationRequest>(y => y.ApplicationId == applicationId), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
