using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.GetSection;
using SFA.DAS.ApplyService.Application.Apply.Review.Applications;
using SFA.DAS.ApplyService.Application.Apply.Review.Return;
using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
     public class ReviewController : Controller
    {
        ///////////////////////////////////////////////////////////
        // TODO: THIS WILL NEED RE-WRITING FOR NEW RoATP PROCESS
        ///////////////////////////////////////////////////////////

        private readonly IMediator _mediator;

        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Review/OpenApplications")]
        public async Task<ActionResult> OpenApplications(int sequenceId = 1)
        {
            var applications = await _mediator.Send(new OpenApplicationsRequest(sequenceId));
            return Ok(applications);
        }

        [HttpGet("Review/FeedbackAddedApplications")]
        public async Task<ActionResult> FeedbackAddedApplications()
        {
            var applications = await _mediator.Send(new FeedbackAddedApplicationsRequest());
            return Ok(applications);
        }

        [HttpGet("Review/ClosedApplications")]
        public async Task<ActionResult> ClosedApplications()
        {
            var applications = await _mediator.Send(new ClosedApplicationsRequest());
            return Ok(applications);
        }

        [HttpGet("Review/Applications/{applicationId}")]
        public async Task<ActionResult> ReviewApplications(Guid applicationId)
        {
            var activeSequence = await _mediator.Send(new GetActiveSequenceRequest(applicationId));
            return Ok(activeSequence);
        }

        [HttpPost("Review/Applications/{applicationId}/Sequences/{sequenceId}/Return")]
        public async Task Return(Guid applicationId, int sequenceId, [FromBody] ReturnApplicationRequest request)
        {
            await _mediator.Send(new ReturnRequest(applicationId, sequenceId, request.ReturnType));
        }
    }

    public class ReturnApplicationRequest
    {
        public string ReturnType { get; set; }
    }

    public class EvaluateSectionRequest
    {
        public bool IsSectionComplete { get; set; }
    }
}