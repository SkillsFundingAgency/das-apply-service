using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Financial;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    public class FinancialController : Controller
    {
        private readonly IMediator _mediator;
        private readonly AssessorServiceApiClient _assessorApiClient;

        public FinancialController(IMediator mediator, AssessorServiceApiClient assessorApiClient)
        {
            _mediator = mediator;
            _assessorApiClient = assessorApiClient;
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
            var organisation = await _mediator.Send(new UpdateGradeRequest(applicationId, updatedGrade));

            if (organisation.OrganisationDetails.OrganisationReferenceType == "RoEPAO")
            {
                // Call the assessor Api to update the org with the financial due date and exemption status. 
            }
            
            return Ok();
        }

        [HttpPost("/Financial/{applicationId}/StartReview")]
        public async Task<ActionResult> StartReview(Guid applicationId)
        {
            await _mediator.Send(new StartFinancialReviewRequest(applicationId));
            return Ok();
        }
    }
}