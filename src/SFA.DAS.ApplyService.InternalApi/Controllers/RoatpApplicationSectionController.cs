using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.InternalApi.Services;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class RoatpApplicationSectionController : Controller
    {
        private readonly IApplicationSectionOrchestrator _orchestrator;
        private readonly ILogger<RoatpApplicationSectionController> _logger;

        public RoatpApplicationSectionController(IApplicationSectionOrchestrator orchestrator,
                                                 ILogger<RoatpApplicationSectionController> logger)
        {
            _orchestrator = orchestrator;
            _logger = logger;
        }

        [Route("ApplySection/FirstPage/{applicationId}/{sequenceId}/{sectionId}")]
        [HttpGet]
        public async Task<IActionResult> GetFirstPage(GetApplicationSectionFirstPageRequest request) 
        {
            // Map AssessorPageId constants -> SequenceId, SectionId instead of passing in directly?

            return Ok(await _orchestrator.GetFirstPage(request));
        } 

        [Route("ApplySection/NextPage/{applicationId}/{sequenceId}/{sectionId}/{pageId}")]
        [HttpGet]
        public async Task<IActionResult> GetNextPage(GetApplicationSectionNextPageRequest request)
        {
            return Ok(await _orchestrator.GetNextPage(request));
        }
    }
}
