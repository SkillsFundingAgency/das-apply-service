using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ApplyService.Application.Apply.Gateway.Applications;
using SFA.DAS.ApplyService.Application.Apply.Gateway;
using SFA.DAS.ApplyService.Application.Apply.Gateway.ApplicationActions;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.EmailService.Interfaces;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.InternalApi.Types.Requests;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class GatewayReviewController : Controller
    {
        private readonly IMediator _mediator;

        public GatewayReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GatewayReview/Counts")]
        public async Task<GetGatewayApplicationCountsResponse> GetApplicationCounts()
        {
            var applicationCounts = await _mediator.Send(new GetGatewayApplicationCountsRequest());
            return applicationCounts;
        }

        [HttpGet("GatewayReview/NewApplications")]
        public async Task<ActionResult> NewApplications()
        {
            var applications = await _mediator.Send(new NewGatewayApplicationsRequest());
            return Ok(applications);
        }

        [HttpGet("GatewayReview/InProgressApplications")]
        public async Task<ActionResult> InProgressApplications()
        {
            var applications = await _mediator.Send(new InProgressGatewayApplicationsRequest());
            return Ok(applications);
        }

        [HttpGet("GatewayReview/ClosedApplications")]
        public async Task<ActionResult> ClosedApplications()
        {
            var applications = await _mediator.Send(new ClosedGatewayApplicationsRequest());
            return Ok(applications);
        }

        [HttpPost("GatewayReview/{applicationId}/Evaluate")]
        public async Task EvaluateGateway(Guid applicationId, [FromBody] EvaluateGatewayApplicationRequest request)
        {
            await _mediator.Send(new EvaluateGatewayRequest(applicationId, request.IsGatewayApproved, request.EvaluatedBy));
        }

        [HttpPost("GatewayReview/{applicationId}/Withdraw")]
        public async Task<bool> WithdrawApplication(Guid applicationId, [FromBody] GatewayWithdrawApplicationRequest request)
        {
            var withdrawResult = await _mediator.Send(new WithdrawApplicationRequest(applicationId, request.Comments, request.UserId, request.UserName));
            return withdrawResult;
        }

        [HttpPost("GatewayReview/{applicationId}/Remove")]
        public async Task<bool> RemoveApplication(Guid applicationId, [FromBody] GatewayRemoveApplicationRequest request)
        {
            return await _mediator.Send(new RemoveApplicationRequest(applicationId, request.Comments, request.ExternalComments, request.UserId, request.UserName));
        }
    }
}
