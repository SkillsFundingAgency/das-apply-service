using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.CheckOrganisationStandardStatus;
using SFA.DAS.ApplyService.Application.Apply.DeleteAnswer;
using SFA.DAS.ApplyService.Application.Apply.Download;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Application.Apply.GetOrganisationForApplication;
using SFA.DAS.ApplyService.Application.Apply.GetPage;
using SFA.DAS.ApplyService.Application.Apply.GetSection;
using SFA.DAS.ApplyService.Application.Apply.Submit;
using SFA.DAS.ApplyService.Application.Apply.UpdateApplicationData;
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

        [HttpGet("Application/{applicationId}")]
        public async Task<ActionResult<Domain.Entities.Application>> GetApplication(Guid applicationId)
        {
            return await _mediator.Send(new GetApplicationRequest(applicationId));
        }
        
        [HttpGet("Applications/{userId}")]
        public async Task<ActionResult<List<Domain.Entities.Application>>> GetApplications(string userId)
        {
            return await _mediator.Send(new GetApplicationsRequest(Guid.Parse(userId)));
        }
        
        [HttpGet("Application/{applicationId}/User/{userId}/Sections")]
        public async Task<ActionResult<ApplicationSequence>> GetSequence(string applicationId, string userId)
        {
            return await _mediator.Send(new GetActiveSequenceRequest(Guid.Parse(applicationId)));
        }
        
        [HttpGet("Application/{applicationId}/User/{userId}/Sequences/{sequenceId}/Sections/{sectionId}")]
        public async Task<ActionResult<ApplicationSection>> GetSection(string applicationId, string userId, int sequenceId, int sectionId)
        {
            var uid = new Guid?();
            var goodUserId = Guid.TryParse(userId, out var parsedUserId);
            if (goodUserId)
            {
                uid = parsedUserId;
            }
            
            return await _mediator.Send(new GetSectionRequest(Guid.Parse(applicationId), uid, sequenceId, sectionId));
        }

        [HttpGet("Application/{applicationId}/User/{userId}/Sequence/{sequenceId}/Sections/{sectionId}/Pages/{pageId}")]
        public async Task<ActionResult<Page>> GetPage(string applicationId, string userId, int sequenceId, int sectionId, string pageId)
        {
            var uid = new Guid?();
            var goodUserId = Guid.TryParse(userId, out var parsedUserId);
            if (goodUserId)
            {
                uid = parsedUserId;
            }
            
            try
            {
                return await _mediator.Send(new GetPageRequest(Guid.Parse(applicationId), sequenceId, sectionId, pageId, uid));
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
        
       

        [HttpPost("/Applications/Submit")]
        public async Task<ActionResult> Submit([FromBody] ApplicationSubmitRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }
        
        [HttpPost("Application/{applicationId}/User/{userId}/Sequence/{sequenceId}/Sections/{sectionId}/Pages/{pageId}/DeleteAnswer/{answerId}")]
        public async Task<IActionResult> DeleteAnswer(string applicationId, string userId, int sequenceId, int sectionId, string pageId, Guid answerId)
        {
            await _mediator.Send(new DeletePageAnswerRequest(Guid.Parse(applicationId), Guid.Parse(userId),sequenceId,sectionId, pageId, answerId));
            return Ok();
        }

        [HttpPost("/Application/{applicationId}/UpdateApplicationData")]
        public async Task<ActionResult> UpdateApplicationData(Guid applicationId, [FromBody] object applicationData)
        {
            await _mediator.Send(new UpdateApplicationDataRequest(applicationId, applicationData));
            return Ok();
        }

        [HttpGet("/Application/{applicationId}/Organisation")]
        public async Task<Organisation> GetOrganisationForApplication(Guid applicationId)
        {
            return await _mediator.Send(new GetOrganisationForApplicationRequest(applicationId));
        }

        [HttpGet("/Application/{applicationId}/standard/{standardId}/check-status")]
        public async Task<String> GetApplicationStatusForStandard(Guid applicationId, int standardId)
        { 
            return await _mediator.Send(new CheckOrganisationStandardStatusRequest(applicationId, standardId));
        }

    }
}