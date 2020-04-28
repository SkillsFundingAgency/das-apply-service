using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class NotRequiredOverridesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<NotRequiredOverridesController> _logger;

        public NotRequiredOverridesController(IMediator mediator, ILogger<NotRequiredOverridesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("NotRequiredOverrides/{applicationId}")]
        public async Task<IActionResult> GetNotRequiredOverrides(Guid applicationId)
        {
            return Ok(await _mediator.Send(new GetNotRequiredOverridesRequest(applicationId)));
        }

        [HttpPost("NotRequiredOverrides/{applicationId}")]
        public async Task<IActionResult> SaveNotRequiredOverrides(Guid applicationId, [FromBody] NotRequiredOverrideConfiguration notRequiredOverrides)
        {
            return Ok(await _mediator.Send(new UpdateNotRequiredOverridesRequest(applicationId, notRequiredOverrides)));
        }
    }
}
