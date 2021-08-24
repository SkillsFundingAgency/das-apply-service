using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Appeals.Commands.DeleteAppealFile;
using SFA.DAS.ApplyService.Application.Appeals.Commands.UploadAppealFile;
using SFA.DAS.ApplyService.Application.Appeals.Queries.GetAppealFile;
using SFA.DAS.ApplyService.Application.Apply.Appeals.Commands.MakeAppeal;
using SFA.DAS.ApplyService.Application.Apply.Appeals.Queries.GetAppeal;
using SFA.DAS.ApplyService.InternalApi.Types.Requests.Appeals;
using SFA.DAS.ApplyService.InternalApi.Types.Responses.Appeals;
using SFA.DAS.ApplyService.InternalApi.Extensions;
using SFA.DAS.ApplyService.Application.Apply.Appeals.Queries.GetAppealFileList;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class AppealsController : Controller
    {
        private readonly ILogger<AppealsController> _logger;
        private readonly IMediator _mediator;

        public AppealsController(ILogger<AppealsController> logger, IMediator mediator)
        {
            _logger = logger;
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
                AppealFiles = result.AppealFiles.Select(file => new AppealFile
                {
                    Id = file.Id,
                    Filename = file.Filename,
                    ContentType = file.ContentType
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


        [HttpGet]
        [Route("Appeals/{applicationId}/files")]
        public async Task<ActionResult<GetAppealFileListResponse>> GetAppealFileList([FromRoute] GetAppealFileListRequest request)
        {
            var query = new GetAppealFileListQuery
            {
                ApplicationId = request.ApplicationId,
            };

            var result = await _mediator.Send(query);

            return result == null ? null : new GetAppealFileListResponse
            {
                AppealFiles = result.Select(file => new AppealFile
                {
                    Id = file.Id,
                    Filename = file.Filename,
                    ContentType = file.ContentType
                }).ToList()
            };
        }

        [HttpPost]
        [Route("Appeals/{applicationId}/files")]
         public async Task<IActionResult> UploadAppealFile([FromForm] UploadAppealFileRequest request)
        {
            var command = new UploadAppealFileCommand
            {
                ApplicationId = request.ApplicationId,
                AppealFile = await request.AppealFile.ToFileUpload(),
                UserId = request.UserId,
                UserName = request.UserName
            };

            await _mediator.Send(command);
            return new OkResult();
        }

        [HttpGet]
        [Route("Appeals/{applicationId}/files/{fileId}")]
        public async Task<IActionResult> GetAppealFile([FromRoute] GetAppealFileRequest request)
        {
            var query = new GetAppealFileQuery
            {
                ApplicationId = request.ApplicationId,
                FileId = request.FileId,
            };

            var file = await _mediator.Send(query);

            if (file is null)
            {
                _logger.LogError($"Unable to download file for application: {request.ApplicationId} || appealFileId {request.FileId}");
                return NotFound();
            }

            return File(file.Content, file.ContentType, file.Filename);
        }

        [HttpPost]
        [Route("Appeals/{applicationId}/files/{fileId}/delete")]
        public async Task<IActionResult> DeleteAppealFile(Guid applicationId, Guid fileId, [FromBody] DeleteAppealFileRequest request)
        {
            var command = new DeleteAppealFileCommand
            {
                ApplicationId = applicationId,
                FileId = fileId,
                UserId = request.UserId,
                UserName = request.UserName
            };

            await _mediator.Send(command);
            return new OkResult();
        }
    }
}
