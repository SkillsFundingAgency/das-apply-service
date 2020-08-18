using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class RoatpModerationController : Controller
    {

        private readonly ILogger<RoatpAssessorController> _logger;
        private readonly IMediator _mediator;

        public RoatpModerationController(ILogger<RoatpAssessorController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
    }
}
