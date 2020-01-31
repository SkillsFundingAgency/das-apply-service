using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ApplyService.Application.Apply.Gateway.Applications;
using SFA.DAS.ApplyService.Application.Apply.Gateway;

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

        [HttpPost("GatewayReview/Applications/{applicationId}/Evaluate")]
        public async Task EvaluateGateway(Guid applicationId, [FromBody] EvaluateGatewayApplicationRequest request)
        {
            await _mediator.Send(new EvaluateGatewayRequest(applicationId, request.IsGatewayApproved, request.EvaluatedBy));
        }

        [HttpPost("GatewayReview/Applications/{applicationId}/StartReview")]
        public async Task StartGatewayReview(Guid applicationId, [FromBody] StartGatewayReviewApplicationRequest request)
        {
            await _mediator.Send(new StartGatewayReviewRequest(applicationId, request.Reviewer));
        }
    }

    public class StartGatewayReviewApplicationRequest
    {
        public string Reviewer { get; set; }
    }

    public class EvaluateGatewayApplicationRequest
    {
        public bool IsGatewayApproved { get; set; }
        public string EvaluatedBy { get; set; }
    }
}
