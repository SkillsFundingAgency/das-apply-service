using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Services;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    //MFCMFC coverage for 
    //GatewayPageSubmit	
    //GetGatewayPage
    //GetGatewayPageItemValue
    public class RoatpGatewayControllerTests
    {

        private Mock<IApplyRepository> _applyRepository;
        private Mock<ILogger<RoatpGatewayController>> _logger;
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private Mock<IGatewayApiChecksService> _gatewayApiChecksService;
        private Mock<CompaniesHouseApiClient> _companiesHouseApiClient;
        private Mock<CharityCommissionApiClient> _charityCommissionApiClient;
        private Mock<IRoatpApiClient> _roatpApiClient;
        private RoatpGatewayController _controller;


        [SetUp]
        public void Before_each_test()
        {
            _applyRepository = new Mock<IApplyRepository>();
            _logger = new Mock<ILogger<RoatpGatewayController>>();
            _companiesHouseApiClient = new Mock<CompaniesHouseApiClient>();
            _charityCommissionApiClient = new Mock<CharityCommissionApiClient>();
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _gatewayApiChecksService = new Mock<IGatewayApiChecksService>();

            _controller = new RoatpGatewayController(_applyRepository.Object, _logger.Object, _qnaApiClient.Object,_gatewayApiChecksService.Object);
        }
    }
    }
