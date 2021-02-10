using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Oversight;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.QueryResults;
using SFA.DAS.ApplyService.InternalApi.Services;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class OversightController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IRegistrationDetailsService _registrationDetailsService;

        public OversightController(IMediator mediator, IRegistrationDetailsService registrationDetailsService)
        {
            _mediator = mediator;
            _registrationDetailsService = registrationDetailsService;
        }

        [HttpGet]
        [Route("Oversights/Pending")]
        public async Task<ActionResult<PendingOversightReviews>> OversightsPending()
        {
            return await _mediator.Send(new GetOversightsPendingRequest());
        }

        [HttpGet]
        [Route("Oversights/Completed")]
        public async Task<ActionResult<CompletedOversightReviews>> OversightsCompleted()
        {
            return await _mediator.Send(new GetOversightsCompletedRequest());
        }


        [HttpGet]
        [Route("Oversights/Download")]
        public async Task<ActionResult<List<ApplicationOversightDownloadDetails>>> OversightDownload(DateTime dateFrom, DateTime dateTo)
        {
            return await _mediator.Send(new GetOversightDownloadRequest{ DateFrom = dateFrom, DateTo = dateTo });
        }


        [HttpPost]
        [Route("Oversight/Outcome")]
        public async Task<ActionResult<bool>> RecordOversightOutcome([FromBody] RecordOversightOutcomeCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost]
        [Route("Oversight/GatewayFailOutcome")]
        public async Task<ActionResult> RecordOversightGatewayFailOutcome([FromBody] RecordOversightGatewayFailOutcomeCommand command)
        {
            await _mediator.Send(command);
            return new OkResult();
        }

        [HttpGet]
        [Route("Oversight/RegistrationDetails/{applicationId}")]
        public async Task<ActionResult<RoatpRegistrationDetails>> GetRegistrationDetails(Guid applicationId)
        {
            return await _registrationDetailsService.GetRegistrationDetails(applicationId);
        }
        
        [HttpGet]
        [Route("Oversights/{applicationId}")]
        public async Task<ActionResult<ApplicationOversightDetails>> OversightDetails(Guid applicationId)
        {
            return await _mediator.Send(new GetOversightDetailsRequest(applicationId));
        }
    }
}
