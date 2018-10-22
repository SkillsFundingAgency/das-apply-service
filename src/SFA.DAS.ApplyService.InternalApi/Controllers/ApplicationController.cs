using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.GetPage;
using SFA.DAS.ApplyService.Application.Apply.UpdatePageAnswers;
using SFA.DAS.ApplyService.Domain.Apply;
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
            await _mediator.Send(new StartApplicationRequest(request.ApplicationType, request.ApplyingOrganisationId,
                request.Username));
        }

        [HttpGet("Application/{applicationId}/User/{userId}/Pages/{pageId}")]
        public async Task<ActionResult<Page>> GetPage(string applicationId, string userId, string pageId)
        {
            try
            {
                return await _mediator.Send(new GetPageRequest(Guid.Parse(applicationId), pageId, Guid.Parse(userId)));
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
        
        [HttpPost("Application/{applicationId}/User/{userId}/Pages/{pageId}")]
        public async Task<ActionResult<UpdatePageAnswersResult>> Page(string applicationId, string userId, string pageId, [FromBody] List<Answer> answers)
        {
            var updatedPage = await _mediator.Send(new UpdatePageAnswersRequest(Guid.Parse(applicationId), Guid.Parse(userId), pageId, answers));
            return updatedPage;
        }
    }
}