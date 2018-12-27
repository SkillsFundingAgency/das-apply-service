using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.GetSection;
using SFA.DAS.ApplyService.Application.Apply.Review;
using SFA.DAS.ApplyService.Application.Apply.Review.Feedback;
using SFA.DAS.ApplyService.Application.Apply.Review.Return;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IMediator _mediator;

        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Review/NewApplications")]
        public async Task<ActionResult> NewApplications()
        {
            var applications = await _mediator.Send(new NewApplicationsRequest());
            return Ok(applications);
        }

        [HttpGet("Review/Applications/{applicationId}")]
        public async Task<ActionResult> ReviewApplications(Guid applicationId)
        {
            var activeSequence = await _mediator.Send(new GetActiveSequenceRequest(applicationId));
            return Ok(activeSequence);
        }

        [HttpPost("Review/Applications/{applicationId}/Sequences/{sequenceId}/Sections/{sectionId}/Pages/{pageId}/AddFeedback")]
        public async Task AddFeedback(Guid applicationId, int sequenceId, int sectionId, string pageId,
            [FromBody] Feedback feedback)
        {
            await _mediator.Send(new AddFeedbackRequest(applicationId, sequenceId, sectionId, pageId, feedback));
        }
        
        [HttpPost("Review/Applications/{applicationId}/Sequences/{sequenceId}/Return")]
        public async Task Return(Guid applicationId, int sequenceId, [FromBody] ReturnApplicationRequest request)
        {
            await _mediator.Send(new ReturnRequest(applicationId, sequenceId, request.ReturnType));
        }

        [HttpPost("Review/Applications/{applicationId}/Sequences/{sequenceId}/StartReview")]
        public async Task StartReview(Guid applicationId, int sequenceId)
        {
            await _mediator.Send(new StartApplicationReviewRequest(applicationId, sequenceId));
        }
    }

    public class ReturnApplicationRequest
    {
        public string ReturnType { get; set; }
    }
}