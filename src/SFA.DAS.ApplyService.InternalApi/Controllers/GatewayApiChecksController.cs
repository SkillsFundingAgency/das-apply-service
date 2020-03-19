using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ApplyService.InternalApi.Services;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class GatewayApiChecksController : Controller
    {
        private readonly IApplyRepository _applyRepository;
        private readonly ILogger<GatewayApiChecksController> _logger;
        private readonly IGatewayApiChecksService _gatewayApiChecksService;

        public GatewayApiChecksController(IApplyRepository applyRepository, ILogger<GatewayApiChecksController> logger, 
                                          IGatewayApiChecksService gatewayApiChecksService)
        {
            _applyRepository = applyRepository;
            _logger = logger;
            _gatewayApiChecksService = gatewayApiChecksService;
        }

        [HttpGet]
        [Route("Gateway/ApiChecks/{applicationId}/{userRequestedChecks}")]
        public async Task<IActionResult> ExternalApiChecks(Guid applicationId, string userRequestedChecks)
        {
            var applyData = await GetApplyData(applicationId);

            if (applyData.GatewayReviewDetails == null)
            {
                _logger.LogInformation($"Getting external API checks data for application {applicationId}");
                applyData.GatewayReviewDetails = await _gatewayApiChecksService.GetExternalApiCheckDetails(applicationId, userRequestedChecks);

                await _applyRepository.UpdateApplyData(applicationId, applyData, userRequestedChecks);
            }
            return Ok(applyData.GatewayReviewDetails);
        }             

        private async Task<ApplyData> GetApplyData(Guid applicationId)
        {
            return await _applyRepository.GetApplyData(applicationId);
        }        

        [HttpGet]
        [Route("Gateway/UkrlpData/{applicationId}")]
        public async Task<IActionResult> GetUkrlpData(Guid applicationId)
        {
            var applyData = await GetApplyData(applicationId);

            if (applyData?.GatewayReviewDetails?.UkrlpDetails == null)
            {
                return NotFound();
            }

            return Ok(applyData.GatewayReviewDetails.UkrlpDetails);
        }

        [HttpGet]
        [Route("Gateway/CompaniesHouseData/{applicationId}")]
        public async Task<IActionResult> GetCompaniesHouseData(Guid applicationId)
        {
            var applyData = await GetApplyData(applicationId);

            if (applyData?.GatewayReviewDetails?.CompaniesHouseDetails == null)
            {
                return NotFound();
            }

            return Ok(applyData.GatewayReviewDetails.CompaniesHouseDetails);
        }

        [HttpGet]
        [Route("Gateway/CharityCommissionData/{applicationId}")]
        public async Task<IActionResult> GetCharityCommissionData(Guid applicationId)
        {
            var applyData = await GetApplyData(applicationId);

            if (applyData?.GatewayReviewDetails?.CharityCommissionDetails == null)
            {
                return NotFound();
            }

            return Ok(applyData.GatewayReviewDetails.CharityCommissionDetails);
        }

        [HttpGet]
        [Route("Gateway/RoatpRegisterData/{applicationId}")]
        public async Task<IActionResult> GetRoatpRegisterData(Guid applicationId)
        {
            var applyData = await GetApplyData(applicationId);

            if (applyData?.GatewayReviewDetails?.RoatpRegisterDetails == null)
            {
                return NotFound();
            }

            return Ok(applyData.GatewayReviewDetails.RoatpRegisterDetails);
        }

        [HttpGet]
        [Route("Gateway/SourcesCheckedOn/{applicationId}")]
        public async Task<IActionResult> GetSourcesCheckedOnDate(Guid applicationId)
        {
            var applyData = await GetApplyData(applicationId);

            if (applyData?.GatewayReviewDetails == null)
            {
                return NotFound();
            }

            return Ok(applyData.GatewayReviewDetails.SourcesCheckedOn);
        }
    }
}
