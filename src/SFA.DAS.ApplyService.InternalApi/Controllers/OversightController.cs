using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Oversight;
using SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetOversightDetails;
using SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetOversightReview;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.QueryResults;
using SFA.DAS.ApplyService.InternalApi.Services;
using SFA.DAS.ApplyService.InternalApi.Types.Requests.Oversight;
using SFA.DAS.ApplyService.InternalApi.Types.Responses.Oversight;

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
        public async Task<ActionResult<PendingOversightReviews>> OversightsPending(string searchTerm, string sortColumn, string sortOrder)
        {
            return await _mediator.Send(new GetOversightsPendingRequest(searchTerm, sortColumn, sortOrder));
        }

        [HttpGet]
        [Route("Oversights/Completed")]
        public async Task<ActionResult<CompletedOversightReviews>> OversightsCompleted(string searchTerm, string sortColumn, string sortOrder)
        {
            return await _mediator.Send(new GetOversightsCompletedRequest(searchTerm, sortColumn, sortOrder));
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

        [HttpPost]
        [Route("Oversight/GatewayRemovedOutcome")]
        public async Task<ActionResult> RecordOversightGatewayRemovedOutcome([FromBody] RecordOversightGatewayRemovedOutcomeCommand command)
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
            return await _mediator.Send(new GetOversightApplicationDetailsRequest(applicationId));
        }

        [HttpGet]
        [Route("Oversight/{applicationId}/review")]
        public async Task<ActionResult<GetOversightReviewResponse>> OversightReview(GetOversightReviewRequest request)
        {
            var query = new GetOversightReviewQuery {ApplicationId = request.ApplicationId};

            var result = await _mediator.Send(query);

            return result == null ? null : new GetOversightReviewResponse
            {
                Id = result.Id,
                Status = result.Status,
                ApplicationDeterminedDate = result.ApplicationDeterminedDate,
                InProgressDate = result.InProgressDate,
                InProgressUserId = result.InProgressUserId,
                InProgressUserName = result.InProgressUserName,
                InProgressInternalComments = result.InProgressInternalComments,
                InProgressExternalComments = result.InProgressExternalComments,
                GatewayApproved = result.GatewayApproved,
                ModerationApproved = result.ModerationApproved,
                InternalComments = result.InternalComments,
                ExternalComments = result.ExternalComments,
                UserId = result.UserId,
                UserName = result.UserName
            };
        }
    }
}
