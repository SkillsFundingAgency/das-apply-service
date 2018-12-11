using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Financial;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    public class FinancialController : Controller
    {
        private readonly IMediator _mediator;

        public FinancialController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/Financial/New")]
        public async Task<ActionResult> NewApplications()
        {
            return Ok(await _mediator.Send(new NewApplicationsRequest()));
        }
        
        [HttpGet("/Financial/Previous")]
        public async Task<ActionResult> PreviousApplications()
        {
            return Ok(await _mediator.Send(new PreviousApplicationsRequest()));
        }

        [HttpPost("/Financial/{applicationId}/UpdateGrade")]
        public async Task<ActionResult> UpdateGrade(Guid applicationId, [FromBody] FinancialApplicationGrade updatedGrade)
        {
            return Ok(await _mediator.Send(new UpdateGradeRequest(applicationId, updatedGrade)));
        }

        [HttpPost("/Financial/{applicationId}/StartReview")]
        public async Task<ActionResult> StartReview(Guid applicationId)
        {
            await _mediator.Send(new StartReviewRequest(applicationId));
            return Ok();
        }
    }
}