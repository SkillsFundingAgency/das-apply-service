using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Financial;
using SFA.DAS.ApplyService.Application.Apply.Financial.Applications;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class FinancialController : Controller
    {
        private readonly IMediator _mediator;

        public FinancialController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/Financial/OpenApplications")]
        public async Task<ActionResult> OpenApplications()
        {
            var applications = await _mediator.Send(new OpenFinancialApplicationsRequest());
            return Ok(applications);
        }

        [HttpGet("/Financial/FeedbackAddedApplications")]
        public async Task<ActionResult> FeedbackAddedApplications()
        {
            var applications = await _mediator.Send(new FeedbackAddedFinancialApplicationsRequest());
            return Ok(applications);
        }

        [HttpGet("/Financial/ClosedApplications")]
        public async Task<ActionResult> ClosedApplications()
        {
            var applications = await _mediator.Send(new ClosedFinancialApplicationsRequest());
            return Ok(applications);
        }

        [HttpPost("/Financial/{applicationId}/Grade")]
        public async Task<IActionResult> RecordGrade(Guid applicationId, [FromBody] FinancialReviewDetails financialReviewDetails)
        {
            await _mediator.Send(new RecordGradeRequest(applicationId, financialReviewDetails));
            return Ok();
        }

        [HttpPost("/Financial/{applicationId}/UpdateGrade")]
        public async Task<ActionResult> UpdateGrade(Guid applicationId, [FromBody] FinancialApplicationGrade updatedGrade)
        {
            await _mediator.Send(new UpdateGradeRequest(applicationId, updatedGrade));
            return Ok();
        }

        [HttpPost("/Financial/{applicationId}/StartReview")]
        public async Task<ActionResult> StartReview(Guid applicationId, [FromBody] StartFinancialReviewApplicationRequest request)
        {
            await _mediator.Send(new StartFinancialReviewRequest(applicationId, request.Reviewer));
            return Ok();
        }
    }

    public class StartFinancialReviewApplicationRequest
    {
        public string Reviewer { get; set; }
    }
}