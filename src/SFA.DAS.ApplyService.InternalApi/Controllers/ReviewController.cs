using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.GetSection;
using SFA.DAS.ApplyService.Application.Apply.Review;
using SFA.DAS.ApplyService.Application.Apply.Review.Feedback;
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

        [HttpGet("Review/Applications")]
        public async Task<ActionResult> ReviewApplications()
        {
            var applications = await _mediator.Send(new ReviewRequest());
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
    }
}