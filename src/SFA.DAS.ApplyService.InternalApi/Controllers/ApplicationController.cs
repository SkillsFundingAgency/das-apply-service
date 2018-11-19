using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Download;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Application.Apply.GetPage;
using SFA.DAS.ApplyService.Application.Apply.GetSection;
using SFA.DAS.ApplyService.Application.Apply.UpdatePageAnswers;
using SFA.DAS.ApplyService.Application.Apply.Upload;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly IMediator _mediator;

        public ApplicationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Application/Start")]
        public async Task Start([FromBody] StartApplyRequest request)
        {
            await _mediator.Send(new StartApplicationRequest(request.UserId));
        }

        [HttpGet("Applications/{userId}")]
        public async Task<ActionResult<List<Domain.Entities.Application>>> GetApplications(string userId)
        {
            return await _mediator.Send(new GetApplicationsRequest(Guid.Parse(userId)));
        }
        
        [HttpGet("Application/{applicationId}/User/{userId}/Sequences/{sequenceId}/Sections")]
        public async Task<ActionResult<List<ApplicationSection>>> GetSections(string applicationId, string userId, int sequenceId)
        {
            return await _mediator.Send(new GetSectionsRequest(Guid.Parse(applicationId), sequenceId, Guid.Parse(userId)));
        }
        
        [HttpGet("Application/{applicationId}/User/{userId}/Sequences/{sequenceId}/Sections/{sectionId}")]
        public async Task<ActionResult<ApplicationSection>> GetSection(string applicationId, string userId, int sequenceId, int sectionId)
        {
            return await _mediator.Send(new GetSectionRequest(Guid.Parse(applicationId), Guid.Parse(userId),
                sequenceId, sectionId));
        }

        [HttpGet("Application/{applicationId}/User/{userId}/Sequence/{sequenceId}/Sections/{sectionId}/Pages/{pageId}")]
        public async Task<ActionResult<Page>> GetPage(string applicationId, string userId, int sequenceId, int sectionId, string pageId)
        {
            try
            {
                return await _mediator.Send(new GetPageRequest(Guid.Parse(applicationId), sequenceId, sectionId, pageId, Guid.Parse(userId)));
            }
            catch (Exception e)
            {
                if (e is BadRequestException)
                {
                    return BadRequest();
                }

                throw;
            }
        }
        
        [HttpPost("Application/{applicationId}/User/{userId}/Sequence/{sequenceId}/Sections/{sectionId}/Pages/{pageId}")]
        public async Task<ActionResult<UpdatePageAnswersResult>> Page(string applicationId, string userId, int sequenceId, int sectionId, string pageId, [FromBody] List<Answer> answers)
        {
            var updatedPage = await _mediator.Send(new UpdatePageAnswersRequest(Guid.Parse(applicationId), Guid.Parse(userId),sequenceId,sectionId, pageId, answers));
            return updatedPage;
        }
        
        [HttpPost("Application/{applicationId}/User/{userId}/Page/{pageId}/Upload")]
        public async Task<ActionResult<UploadResult>> Upload(string applicationId, string userId, string pageId)
        {
            var uploadResult = await _mediator.Send(new UploadRequest(Guid.Parse(applicationId), Guid.Parse(userId), pageId, HttpContext.Request.Form.Files));
            return uploadResult;
        }
        
        [HttpGet("Application/{applicationId}/User/{userId}/Page/{pageId}/Question/{questionId}/{filename}/Download")]
        public async Task<IActionResult> Download(string applicationId, string userId, string pageId, string questionId, string filename)
        {
            var downloadResponse = await _mediator.Send(new DownloadRequest(Guid.Parse(applicationId), Guid.Parse(userId), pageId, questionId, filename));
            return File(downloadResponse.FileStream, "image/png", downloadResponse.Filename);
        }
    }
}