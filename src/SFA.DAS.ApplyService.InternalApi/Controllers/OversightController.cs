using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Oversight;
using SFA.DAS.ApplyService.Application.Apply.Oversight.Commands.UploadAppealFile;
using SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetOversightDetails;
using SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetStagedFiles;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.QueryResults;
using SFA.DAS.ApplyService.InternalApi.Extensions;
using SFA.DAS.ApplyService.InternalApi.Services;
using SFA.DAS.ApplyService.InternalApi.Types.Requests.Oversight;

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
            return await _mediator.Send(new GetOversightDetailsRequest(applicationId));
        }

        [HttpPost]
        [Route("Oversight/{applicationId}/uploads")]
        public async Task<IActionResult> UploadAppealFile([FromForm] UploadAppealFileRequest request)
        {
            var command = new UploadAppealFileCommand
            {
                ApplicationId = request.ApplicationId,
                File = await request.File.ToFileUpload(),
                UserId = request.UserId,
                UserName = request.UserName
            };

            await _mediator.Send(command);
            return new OkResult();
        }

        [HttpPost]
        [Route("Oversight/{applicationId}/uploads/{fileId}/remove")]
        public async Task<IActionResult> RemoveAppealFile(Guid applicationId, Guid fileId, [FromBody] RemoveAppealFileRequest request)
        {
            var command = new RemoveAppealFileCommand
            {
                ApplicationId = applicationId,
                FileId = fileId,
                UserId = request.UserId,
                UserName = request.UserName
            };

            await _mediator.Send(command);
            return new OkResult();
        }
        
        [HttpGet]
        [Route("Oversight/{applicationId}/uploads")]
        public async Task<ActionResult<AppealFiles>> StagedUploads([FromRoute] GetStagedFilesRequest request)
        {
            return await _mediator.Send(request);
        }
    }
}
