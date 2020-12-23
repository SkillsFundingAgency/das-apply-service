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
    }

    public class EvaluateGatewayApplicationRequest
    {
        public bool IsGatewayApproved { get; set; }
        public string EvaluatedBy { get; set; }
    }


    public class SubcontractorDeclarationClarificationFileCommand
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FileName { get; set; }
    }
}
