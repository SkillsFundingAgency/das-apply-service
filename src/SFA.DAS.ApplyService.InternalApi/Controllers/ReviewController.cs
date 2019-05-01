using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.GetSection;
using SFA.DAS.ApplyService.Application.Apply.Review;
using SFA.DAS.ApplyService.Application.Apply.Review.Applications;
using SFA.DAS.ApplyService.Application.Apply.Review.Evaluate;
using SFA.DAS.ApplyService.Application.Apply.Review.Feedback;
using SFA.DAS.ApplyService.Application.Apply.Review.Return;
using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    //[Authorize(Roles = "ApplyServiceInternalAPI")]
    public class ReviewController : Controller
    {
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

        [HttpPost("Review/Applications/{applicationId}/Sequences/{sequenceId}/Sections/{sectionId}/Pages/{pageId}/AddFeedback")]
        public async Task AddFeedback(Guid applicationId, int sequenceId, int sectionId, string pageId,
            [FromBody] Feedback feedback)
        {
            await _mediator.Send(new AddFeedbackRequest(applicationId, sequenceId, sectionId, pageId, feedback));
        }

        [HttpPost("Review/Applications/{applicationId}/Sequences/{sequenceId}/Sections/{sectionId}/Pages/{pageId}/DeleteFeedback")]
        public async Task DeleteFeedback(Guid applicationId, int sequenceId, int sectionId, string pageId, [FromBody] Guid feedbackId)
        {
            await _mediator.Send(new DeleteFeedbackRequest(applicationId, sequenceId, sectionId, pageId, feedbackId));
        }

        [HttpPost("Review/Applications/{applicationId}/Sequences/{sequenceId}/Sections/{sectionId}/Evaluate")]
        public async Task EvaluateSection(Guid applicationId, int sequenceId, int sectionId, [FromBody] EvaluateSectionRequest request)
        {
            await _mediator.Send(new EvaluateRequest(applicationId, sequenceId, sectionId, request.IsSectionComplete));
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

    public class EvaluateSectionRequest
    {
        public bool IsSectionComplete { get; set; }
    }
}