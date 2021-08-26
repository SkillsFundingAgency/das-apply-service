using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Appeals.Commands.MakeAppeal;
using SFA.DAS.ApplyService.Application.Apply.Appeals.Queries.GetAppeal;
using SFA.DAS.ApplyService.InternalApi.Types.Requests.Appeals;
using SFA.DAS.ApplyService.InternalApi.Types.Responses.Appeals;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class AppealsController : Controller
    {
        private readonly IMediator _mediator;

        public AppealsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("Appeals/{applicationId}")]
        public async Task<ActionResult<GetAppealResponse>> GetAppeal([FromRoute] GetAppealRequest request)
        {
            var query = new GetAppealQuery
            {
                ApplicationId = request.ApplicationId,
            };

            var result = await _mediator.Send(query);

            return result == null ? null : new GetAppealResponse
            {
                Id = result.Id,
                ApplicationId = result.ApplicationId,
                Status = result.Status,
                HowFailedOnPolicyOrProcesses = result.HowFailedOnPolicyOrProcesses,
                HowFailedOnEvidenceSubmitted = result.HowFailedOnEvidenceSubmitted,
                AppealSubmittedDate = result.AppealSubmittedDate,
                AppealDeterminedDate = result.AppealDeterminedDate,
                InternalComments = result.InternalComments,
                ExternalComments = result.ExternalComments,
                UserId = result.UserId,
                UserName = result.UserName,
                InProgressDate = result.InProgressDate,
                InProgressUserId = result.InProgressUserId,
                InProgressUserName = result.InProgressUserName,
                InProgressInternalComments = result.InProgressInternalComments,
                InProgressExternalComments = result.InProgressExternalComments,
                CreatedOn = result.CreatedOn,
                UpdatedOn = result.UpdatedOn,
                Uploads = result.Uploads.Select(upload => new GetAppealResponse.AppealUpload
                {
                    Id = upload.Id,
                    Filename = upload.Filename,
                    ContentType = upload.ContentType
                }).ToList()
            };
        }

        [HttpPost]
        [Route("Appeals/{applicationId}")]
        public async Task<IActionResult> MakeAppeal(Guid applicationId, [FromBody] MakeAppealRequest request)
        {
            var command = new MakeAppealCommand
            {
                ApplicationId = applicationId,
                HowFailedOnPolicyOrProcesses = request.HowFailedOnPolicyOrProcesses,
                HowFailedOnEvidenceSubmitted = request.HowFailedOnEvidenceSubmitted,
                UserId = request.UserId,
                UserName = request.UserName
            };

            await _mediator.Send(command);
            return new OkResult();
        }
    }
}
