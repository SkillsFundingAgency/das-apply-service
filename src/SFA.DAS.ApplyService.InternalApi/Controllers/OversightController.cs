using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Oversight;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class OversightController : Controller
    {
        private readonly IMediator _mediator;

        public OversightController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("Oversights/Pending")]
        public async Task<ActionResult<List<ApplicationOversightDetails>>> OversightsPending()
        {
            return await _mediator.Send( new GetOversightsPendingRequest());
        }

        [HttpGet]
        [Route("Oversights/Completed")]
        public async Task<ActionResult<List<ApplicationOversightDetails>>> OversightsCompleted()
        {
            return await _mediator.Send(new GetOversightsCompletedRequest());
        }

        [HttpGet]
        [Route("Oversights/RegisterDetails/{applicationId}")]
        public async Task<ActionResult<RoatpRegistrationDetails>> GetRegisterDetails(Guid applicationId)
        {
            return await _mediator.Send(new GetRoatpRegistrationDetailsRequest(applicationId));
        }

        [HttpPost]
        [Route("Oversight/Outcome")]
        public async Task<ActionResult<bool>> RecordOversightOutcome([FromBody] RecordOversightOutcomeCommand command)
        {
            return await _mediator.Send(command);
        }
        
    }
}
