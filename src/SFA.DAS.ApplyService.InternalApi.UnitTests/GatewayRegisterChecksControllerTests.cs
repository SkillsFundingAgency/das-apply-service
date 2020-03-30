using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class GatewayRegisterChecksControllerTests
    {
        private readonly Guid _applicationId = Guid.NewGuid();

        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private Mock<IMediator> _mediator;
        private Mock<ILogger<GatewayRegisterChecksController>> _logger;
        private GatewayRegisterChecksController _controller;

        [SetUp]
        public void Before_each_test()
        {
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<GatewayRegisterChecksController>>();
            _controller = new GatewayRegisterChecksController(_qnaApiClient.Object, _mediator.Object, _logger.Object);
        }

        [TestCase("1")]
        [TestCase("2")]
        [TestCase("3")]
        public async Task GetProviderRoute_returns_expected_value(string providerRoute)
        {
            _qnaApiClient
               .Setup(x => x.GetAnswerValue(_applicationId,
                                           RoatpWorkflowSequenceIds.Preamble,
                                           RoatpWorkflowSectionIds.Preamble,
                                           RoatpWorkflowPageIds.ProviderRoute,
                                           RoatpPreambleQuestionIdConstants.ApplyProviderRoute)).ReturnsAsync(providerRoute);

            var actualResult = await _controller.GetProviderRoute(_applicationId);
            Assert.AreEqual(providerRoute, actualResult);
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
