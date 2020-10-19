using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class NotRequiredOverridesController : Controller
    {
        private readonly IMediator _mediator;

        public NotRequiredOverridesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("NotRequiredOverrides/{applicationId}")]
        public async Task<IActionResult> GetNotRequiredOverrides(Guid applicationId)
        {
            var notRequiredOverrides = await _mediator.Send(new GetNotRequiredOverridesRequest(applicationId));
            return Ok(notRequiredOverrides);
        }

        [HttpPost("NotRequiredOverrides/{applicationId}")]
        public async Task<IActionResult> SaveNotRequiredOverrides(Guid applicationId, [FromBody] IEnumerable<NotRequiredOverride> notRequiredOverrides)
        {
            var updateResult = await _mediator.Send(new UpdateNotRequiredOverridesRequest(applicationId, notRequiredOverrides));
            return Ok(updateResult);
        }
    }
}
