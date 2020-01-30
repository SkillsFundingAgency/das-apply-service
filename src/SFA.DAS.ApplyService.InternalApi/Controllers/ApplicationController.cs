using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.CheckOrganisationStandardStatus;
using SFA.DAS.ApplyService.Application.Apply.DeleteAnswer;
using SFA.DAS.ApplyService.Application.Apply.GetAnswers;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Application.Apply.GetOrganisationForApplication;
using SFA.DAS.ApplyService.Application.Apply.GetPage;
using SFA.DAS.ApplyService.Application.Apply.GetSection;
using SFA.DAS.ApplyService.Application.Apply.GetSequence;
using SFA.DAS.ApplyService.Application.Apply.Start;
using SFA.DAS.ApplyService.Application.Apply.Submit;
using SFA.DAS.ApplyService.Application.Apply.UpdatePageAnswers;
using SFA.DAS.ApplyService.Domain.Apply;
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

        [HttpGet("/Applications/Open")]
        public async Task<IEnumerable<Apply>> GetOpenApplications()
        {
            return await _mediator.Send(new GetOpenApplicationsRequest());
        }

        [HttpGet("/Applications/Closed")]
        public async Task<IEnumerable<Apply>> GetClosedApplications()
        {
            return await _mediator.Send(new GetClosedApplicationsRequest());
        }

        [HttpGet("/Applications/FeedbackAdded")]
        public async Task<IEnumerable<Apply>> GetFeedbackAddedApplications()
        {
            return await _mediator.Send(new GetFeedbackAddedApplicationsRequest());
        }
    }
}