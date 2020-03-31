using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.InternalApi.Controllers;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class GatewayRegisterChecksControllerTests
    {
        private readonly Guid _applicationId = Guid.NewGuid();

        private Mock<IMediator> _mediator;
        private Mock<ILogger<GatewayRegisterChecksController>> _logger;
        private GatewayRegisterChecksController _controller;

        [SetUp]
        public void Before_each_test()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<GatewayRegisterChecksController>>();
            _controller = new GatewayRegisterChecksController(_mediator.Object, _logger.Object);
        }

        [TestCase("Main")]
        [TestCase("Supporting")]
        [TestCase("Employer")]
        public async Task GetProviderRouteName_returns_expected_value(string providerRouteName)
        {
            var application = new Domain.Entities.Apply
            {
                ApplicationId = _applicationId,
                ApplyData = new Domain.Entities.ApplyData
                {
                    ApplyDetails = new Domain.Entities.ApplyDetails
                    {
                        ProviderRouteName = providerRouteName
                    }
                }
            };

            _mediator
               .Setup(x => x.Send(It.Is<GetApplicationRequest>(y => y.ApplicationId == _applicationId), It.IsAny<CancellationToken>()))
               .ReturnsAsync(application);

            var actualResult = await _controller.GetProviderRouteName(_applicationId);

            Assert.AreEqual(providerRouteName, actualResult);
        }
    }
}
