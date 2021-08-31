using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Internal;
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
using SFA.DAS.ApplyService.InternalApi.Services.Files;
using SFA.DAS.ApplyService.Application.Apply.Appeals.Queries.GetAppealFileList;
using SFA.DAS.ApplyService.Application.Apply.Appeals.Commands.CancelAppeal;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class AppealsController : Controller
    {
        private readonly ILogger<AppealsController> _logger;
        private readonly IMediator _mediator;
        private readonly IFileStorageService _fileStorageService;

        public AppealsController(ILogger<AppealsController> logger, IMediator mediator, IFileStorageService fileStorageService)
        {
            _logger = logger;
            _mediator = mediator;
            _fileStorageService = fileStorageService;
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
                    FileName = file.FileName,
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

        [HttpPost]
        [Route("Appeals/{applicationId}/cancel")]
        public async Task<IActionResult> CancelAppeal(Guid applicationId, [FromBody] CancelAppealRequest request)
        {
            var deletedSuccessfully = await _fileStorageService.DeleteApplicationDirectory(applicationId, ContainerType.Appeals, new CancellationToken());

            if (!deletedSuccessfully)
            {
                _logger.LogError($"Unable to delete appeal files for application: {applicationId}");
            }

            var command = new CancelAppealCommand
            {
                ApplicationId = applicationId,
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
                    FileName = file.FileName,
                    ContentType = file.ContentType
                }).ToList()
            };
        }

        [HttpPost]
        [Route("Appeals/{applicationId}/files")]
        public async Task<IActionResult> UploadAppealFile([FromForm] UploadAppealFileRequest request)
        {
            if (request.AppealFile is null)
            {
                return BadRequest();
            }

            var fileCollection = new FormFileCollection { request.AppealFile };
            var uploadedSuccessfully = await _fileStorageService.UploadApplicationFiles(request.ApplicationId, fileCollection, ContainerType.Appeals, new CancellationToken());

            if (!uploadedSuccessfully)
            {
                _logger.LogError($"Unable to upload appeal file for application: {request.ApplicationId}");
                return BadRequest();
            }

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
        [Route("Appeals/{applicationId}/files/{fileName}")]
        public async Task<IActionResult> GetAppealFile([FromRoute] GetAppealFileRequest request)
        {
            var query = new GetAppealFileQuery
            {
                ApplicationId = request.ApplicationId,
                FileName = request.FileName
            };

            var appealFile = await _mediator.Send(query);
            if(appealFile is null)
            {
                _logger.LogError($"Unable to find appeal file for application: {request.ApplicationId} || fileName {request.FileName}");
                return NotFound();
            }

            var file = await _fileStorageService.DownloadApplicationFile(appealFile.ApplicationId, appealFile.FileName, ContainerType.Appeals, new CancellationToken());
            if (file is null)
            {
                _logger.LogError($"Unable to download appeal file for application: {request.ApplicationId} || fileName {request.FileName}");
                return NotFound();
            }

            return File(file.Stream, file.ContentType, file.FileName);
        }

        [HttpPost]
        [Route("Appeals/{applicationId}/files/delete/{fileName}")]
        public async Task<IActionResult> DeleteAppealFile(Guid applicationId, string fileName, [FromBody] DeleteAppealFileRequest request)
        {
            var command = new DeleteAppealFileCommand
            {
                ApplicationId = applicationId,
                FileName = fileName,
                UserId = request.UserId,
                UserName = request.UserName
            };

            await _mediator.Send(command);

            var deletedSuccessfully = await _fileStorageService.DeleteApplicationFile(applicationId, fileName, ContainerType.Appeals, new CancellationToken());

            if (!deletedSuccessfully)
            {
                _logger.LogError($"Unable to delete appeal file for application: {applicationId} ||  filename {fileName}");
            }

            return new OkResult();
        }
    }
}
