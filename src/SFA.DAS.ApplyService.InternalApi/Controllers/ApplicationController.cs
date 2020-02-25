using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.CheckOrganisationStandardStatus;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Application.Apply.GetOrganisationForApplication;
using SFA.DAS.ApplyService.Application.Apply.GetSequence;
using SFA.DAS.ApplyService.Application.Apply.Start;
using SFA.DAS.ApplyService.Application.Apply.Submit;
using SFA.DAS.ApplyService.Application.Apply.UpdatePageAnswers;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Roatp;

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

        [HttpGet("Application/{applicationId}")]
        public async Task<ActionResult<Domain.Entities.Apply>> GetApplication(Guid applicationId)
        {
            return await _mediator.Send(new GetApplicationRequest(applicationId));
        }

        [HttpGet("Applications/{userId}")]
        public async Task<ActionResult<List<Domain.Entities.Apply>>> GetApplications(string userId)
        {
            return await _mediator.Send(new GetApplicationsRequest(Guid.Parse(userId), true));
        }

        [HttpGet("Applications/{userId}/Organisation")]
        public async Task<ActionResult<List<Domain.Entities.Apply>>> GetOrganisationApplications(string userId)
        {
            return await _mediator.Send(new GetApplicationsRequest(Guid.Parse(userId), false));
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


        // NOTE: This is old stuff or things which are not migrated over yet
       
        [HttpGet("Application/{applicationId}/Sequences")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IEnumerable<ApplicationSequence>> GetSequences(string applicationId)
        {
            return await _mediator.Send(new GetSequencesRequest(Guid.Parse(applicationId)));
        }

        [HttpPost("Application/{applicationId}/User/{userId}/Sequence/{sequenceId}/Sections/{sectionId}/Pages/{pageId}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<SetPageAnswersResponse>> Page(string applicationId, string userId, int sequenceId, int sectionId, string pageId, [FromBody] PageApplyRequest request)
        {
            var updatedPage = await _mediator.Send(
                new UpdatePageAnswersRequest(Guid.Parse(applicationId), Guid.Parse(userId), sequenceId, sectionId, pageId, request.Answers, request.SaveNewAnswers));
            return updatedPage;
        }

        [HttpGet("/Application/{applicationId}/Organisation")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<Organisation> GetOrganisationForApplication(Guid applicationId)
        {
            return await _mediator.Send(new GetOrganisationForApplicationRequest(applicationId));
        }

        [HttpGet("/Application/{applicationId}/standard/{standardId}/check-status")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<String> GetApplicationStatusForStandard(Guid applicationId, int standardId)
        {
            return await _mediator.Send(new CheckOrganisationStandardStatusRequest(applicationId, standardId));
        }

    }
}