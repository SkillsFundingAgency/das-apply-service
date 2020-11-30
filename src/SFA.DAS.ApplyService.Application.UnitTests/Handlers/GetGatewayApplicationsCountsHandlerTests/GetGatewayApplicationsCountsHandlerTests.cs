using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Gateway.Applications;
using SFA.DAS.ApplyService.Domain.Apply.Gateway;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetGatewayApplicationsCountsHandlerTests
{
    [TestFixture]
    public class GetGatewayApplicationsCountsHandlerTests
    {
        private GetGatewayApplicationsCountsHandler _handler;
        private Mock<IApplyRepository> _repository;

        [SetUp]
        public void TestSetup()
        {
            var data = new List<GatewayApplicationStatusCount>
            {
                new GatewayApplicationStatusCount
                    {GatewayApplicationStatus = GatewayReviewStatus.New, Count = 1},
                new GatewayApplicationStatusCount
                    {GatewayApplicationStatus = GatewayReviewStatus.InProgress, Count = 2},
                new GatewayApplicationStatusCount
                    {GatewayApplicationStatus = GatewayReviewStatus.ClarificationSent, Count = 4},
                new GatewayApplicationStatusCount
                    {GatewayApplicationStatus = GatewayReviewStatus.Fail, Count = 8},
                new GatewayApplicationStatusCount
                    {GatewayApplicationStatus = GatewayReviewStatus.Pass, Count = 16}
            };

            _repository = new Mock<IApplyRepository>();
            _repository.Setup(x => x.GetGatewayApplicationStatusCounts()).ReturnsAsync(data);

            _handler = new GetGatewayApplicationsCountsHandler(_repository.Object);
        }

        [Test]
        public async Task GetGatewayApplicationsCountsHandler_returns_correct_counts()
        {
            var result = await _handler.Handle(new GetGatewayApplicationCountsRequest(), new CancellationToken());

            Assert.AreEqual(1, result.NewApplicationsCount);
            Assert.AreEqual(6, result.InProgressApplicationsCount);
            Assert.AreEqual(24, result.ClosedApplicationsCount);
        }
    }
}
