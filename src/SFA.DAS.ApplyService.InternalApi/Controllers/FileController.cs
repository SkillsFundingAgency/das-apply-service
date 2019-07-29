using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.DeleteFile;
using SFA.DAS.ApplyService.Application.Apply.Download;
using SFA.DAS.ApplyService.Application.Apply.Upload;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        private readonly IMediator _mediator;

        public FileController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost("Upload/Application/{applicationId}/User/{userId}/Sequence/{sequenceId}/Section/{sectionId}/Page/{pageId}")]
        public async Task<ActionResult<UploadResult>> Upload(string applicationId, string userId, int sequenceId, int sectionId, string pageId)
        {
            var uploadResult = await _mediator.Send(new UploadRequest(Guid.Parse(applicationId), sequenceId, sectionId, Guid.Parse(userId), pageId, HttpContext.Request.Form.Files));
            return uploadResult;
        }
        
        [HttpGet("Download/Application/{applicationId}/User/{userId}/Sequence/{sequenceId}/Section/{sectionId}/Page/{pageId}/Question/{questionId}/{filename}")]
        public async Task<FileStreamResult> Download(string applicationId, string userId, int sequenceId, int sectionId, string pageId, string questionId, string filename)
        {
            var downloadResponse = await _mediator.Send(new DownloadRequest(Guid.Parse(applicationId), Guid.Parse(userId), pageId, sequenceId, sectionId, questionId, filename));
            //return downloadResponse;
            return File(downloadResponse.FileStream, downloadResponse.ContentType, downloadResponse.Filename);
        }
        
        [HttpGet("FileInfo/Application/{applicationId}/User/{userId}/Sequence/{sequenceId}/Section/{sectionId}/Page/{pageId}/Question/{questionId}/{filename}")]
        public async Task<ActionResult<FileInfoResponse>> FileInfo(string applicationId, string userId, int sequenceId, int sectionId, string pageId, string questionId, string filename)
        {
            var fileInfoResponse = await _mediator.Send(new FileInfoRequest(Guid.Parse(applicationId), Guid.Parse(userId), pageId, sequenceId, sectionId, questionId, filename));
            
            return fileInfoResponse;
        }

        [HttpPost("/DeleteFile")]
        public async Task<IActionResult> DeleteFile([FromBody] DeleteFileRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}