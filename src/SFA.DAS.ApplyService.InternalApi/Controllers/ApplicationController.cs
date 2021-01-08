using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Application.Apply.Start;
using SFA.DAS.ApplyService.Application.Apply.Submit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Application.Apply.Snapshot;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class ApplicationController : Controller
    {
        private readonly IMediator _mediator;

        public ApplicationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Application/Start")]
        public async Task<ActionResult<Guid>> Start([FromBody] StartApplicationRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpPost("/Application/Submit")]
        public async Task<ActionResult<bool>> Submit([FromBody] SubmitApplicationRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpPost("/Application/Snapshot")]
        public async Task<ActionResult<Guid>> Snapshot([FromBody] SnapshotApplicationRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet("Application/{applicationId}")]
        public async Task<ActionResult<Domain.Entities.Apply>> GetApplication(Guid applicationId)
        {
            return await _mediator.Send(new GetApplicationRequest(applicationId));
        }

        [HttpGet("Application/{applicationId}/Contact")]
        public async Task<ActionResult<Domain.Entities.Contact>> GetContactForApplication(Guid applicationId)
        {
            return await _mediator.Send(new GetContactForApplicationRequest(applicationId));
        }

        [HttpGet("Applications/{signinId}")]
        public async Task<ActionResult<List<Domain.Entities.Apply>>> GetApplications(string signinId)
        {
            return await _mediator.Send(new GetApplicationsRequest(Guid.Parse(signinId), true));
        }

        [HttpGet("Applications/{signinId}/Organisation")]
        public async Task<ActionResult<List<Domain.Entities.Apply>>> GetOrganisationApplications(string signinId)
        {
            return await _mediator.Send(new GetApplicationsRequest(Guid.Parse(signinId), false));
        }

        [HttpGet("Application/{applicationId}/Contact/{signinId}")]
        public async Task<ActionResult<Domain.Entities.Apply>> GetApplicationByUserId(Guid applicationId, string signinId)
        {
            return await _mediator.Send(new GetApplicationByUserIdRequest(applicationId, Guid.Parse(signinId)));
        }

        [HttpGet("/Applications/Existing/{ukprn}")]
        public async Task<IEnumerable<RoatpApplicationStatus>> GetExistingApplicationStatus(string ukprn)
        {
            return await _mediator.Send(new GetExistingApplicationStatusRequest(ukprn));
        }

        [HttpPost("/Application/Status")]
        public async Task<ActionResult<bool>> UpdateApplicationStatus([FromBody] UpdateApplicationStatusRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpPost("/Application/ChangeProviderRoute")]
        public async Task<ActionResult<bool>> ChangeProviderRoute([FromBody] ChangeProviderRouteRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpPost("/Application/{applicationId}/StartAssessorReview")]
        public async Task<bool> StartAssessorReview(Guid applicationId,
            [FromBody] StartAssessorReviewApplicationRequest request)
        {
            return await _mediator.Send(new StartAssessorReviewRequest(applicationId, request.Reviewer));
        }

        [HttpPost("/Application/{applicationId}/StartAssessorSectionReview")]
        public async Task<bool> StartAssessorSectionReview(Guid applicationId,
            [FromBody] StartAssessorSectionReviewRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpPost("/Application/{applicationId}/AssessorEvaluateSection")]
        public async Task<bool> AssessorEvaluateSection(Guid applicationId,
            [FromBody] AssessorEvaluateSectionRequest request)
        {
            return await _mediator.Send(request);
        }
    }

    public class StartAssessorReviewApplicationRequest
    {
        public string Reviewer { get; set; }
    }
}