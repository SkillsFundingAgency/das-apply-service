using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Gateway;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.AutoMapper;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class RoatpGatewayControllerTests
    {
        private Mock<IApplyRepository> _applyRepository;
        private Mock<ILogger<RoatpGatewayController>> _logger;
        private Mock<ILogger<GatewayApiChecksService>> _serviceLogger;
        private Mock<CompaniesHouseApiClient> _companiesHouseApiClient;
        private Mock<CharityCommissionApiClient> _charityCommissionApiClient;
        private Mock<IRoatpApiClient> _roatpApiClient;
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private IGatewayApiChecksService _gatewayApiChecksService;
        private RoatpGatewayController _controller;

        [OneTimeSetUp]
        public void Before_all_tests()
        {
            Mapper.Reset();

            Mapper.Initialize(cfg =>
            {

                cfg.AddProfile<UkrlpCharityCommissionProfile>();
                cfg.AddProfile<UkrlpCompaniesHouseProfile>();
            });

            Mapper.AssertConfigurationIsValid();
        }

        [SetUp]
        public void Before_each_test()
        {
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.GetConfig()).ReturnsAsync(new ApplyConfig { RoatpApiAuthentication = new RoatpApiAuthentication { ApiBaseAddress = "https://localhost" } });

            _applyRepository = new Mock<IApplyRepository>();
            _logger = new Mock<ILogger<RoatpGatewayController>>();
            _serviceLogger = new Mock<ILogger<GatewayApiChecksService>>();
            _companiesHouseApiClient = new Mock<CompaniesHouseApiClient>();
            _charityCommissionApiClient = new Mock<CharityCommissionApiClient>();
            _roatpApiClient = new Mock<IRoatpApiClient>();
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _gatewayApiChecksService = new GatewayApiChecksService(_companiesHouseApiClient.Object, _charityCommissionApiClient.Object,
                                                                   _roatpApiClient.Object, _qnaApiClient.Object, _serviceLogger.Object);

            _controller = new RoatpGatewayController(_applyRepository.Object, _logger.Object, _gatewayApiChecksService);
        }

        [Test]
        public void UpdateGatewayReviewStatusAndComment_executes()
        {
            var applicationId = Guid.NewGuid();
            var gatewayReviewStatus = GatewayReviewStatus.Pass;
            var gatewayReviewComment = "Some commment";
            var userName = "John Smith";
           
            _applyRepository.Setup(x => x.UpdateGatewayReviewStatusAndComment(applicationId, gatewayReviewStatus, gatewayReviewComment, userName)).ReturnsAsync(true); 

            var request = new UpdateGatewayReviewStatusAndCommentRequest(applicationId, gatewayReviewStatus, gatewayReviewComment, userName);
            _controller.UpdateGatewayReviewStatusAndComment(request).GetAwaiter().GetResult();

            _applyRepository.Verify(x => x.UpdateGatewayReviewStatusAndComment(applicationId, gatewayReviewStatus, gatewayReviewComment, userName), Times.Once);
        }
    }
}
