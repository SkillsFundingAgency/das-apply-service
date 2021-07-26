using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class GatewayApiChecksController : Controller
    {
        private readonly IMediator _mediator;

        public GatewayApiChecksController(IMediator mediator)
        {
            _mediator = mediator;
        }        

        [HttpGet]
        [Route("Gateway/UkrlpData/{applicationId}")]
        public async Task<IActionResult> GetUkrlpData(Guid applicationId)
        {
            var applyGatewayDetails = await GetApplyGatewayDetails(applicationId);

            if (applyGatewayDetails?.UkrlpDetails == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(applyGatewayDetails.UkrlpDetails);
            }
        }

        [HttpGet]
        [Route("Gateway/CompaniesHouseData/{applicationId}")]
        public async Task<IActionResult> GetCompaniesHouseData(Guid applicationId)
        {
            var applyGatewayDetails = await GetApplyGatewayDetails(applicationId);

            return Ok(applyGatewayDetails?.CompaniesHouseDetails);
        }

        [HttpGet]
        [Route("Gateway/CharityCommissionData/{applicationId}")]
        public async Task<IActionResult> GetCharityCommissionData(Guid applicationId)
        {
            var applyGatewayDetails = await GetApplyGatewayDetails(applicationId);

            return Ok(applyGatewayDetails?.CharityCommissionDetails);
        }

        [HttpGet]
        [Route("Gateway/RoatpRegisterData/{applicationId}")]
        public async Task<IActionResult> GetRoatpRegisterData(Guid applicationId)
        {
            var applyGatewayDetails = await GetApplyGatewayDetails(applicationId);

            if (applyGatewayDetails?.RoatpRegisterDetails == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(applyGatewayDetails.RoatpRegisterDetails);
            }
        }

        [HttpGet]
        [Route("Gateway/SourcesCheckedOn/{applicationId}")]
        public async Task<IActionResult> GetSourcesCheckedOnDate(Guid applicationId)
        {
            var applyGatewayDetails = await GetApplyGatewayDetails(applicationId);

            if (applyGatewayDetails?.SourcesCheckedOn == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(applyGatewayDetails.SourcesCheckedOn);
            }
        }


        private async Task<ApplyGatewayDetails> GetApplyGatewayDetails(Guid applicationId)
        {
            var application = await _mediator.Send(new GetApplicationRequest(applicationId));

            return application?.ApplyData?.GatewayReviewDetails;
        }
    }
}
