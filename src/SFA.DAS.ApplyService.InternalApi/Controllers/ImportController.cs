using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Import;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    public class ImportController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ImportController> _logger;

        public ImportController(IMediator mediator, ILogger<ImportController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("/Import/Workflow")]
        public async Task<ActionResult> Workflow()
        {
            _logger.LogInformation("In Import Controller.Workflow");
            if (!HttpContext.Request.HasFormContentType || !HttpContext.Request.Form.Files.Any())
            {
                return BadRequest();
            }
            
            var importFile = HttpContext.Request.Form.Files.First();
            
            _logger.LogInformation($"Found file {importFile.FileName}");

            try
            {
                await _mediator.Send(new ImportWorkflowRequest(importFile));
            }
            catch (Exception e)
            {
                _logger.LogError($"Error importing {importFile.FileName}", e);    
                throw;
            }
            

            return Ok();
        }
    }

    
}