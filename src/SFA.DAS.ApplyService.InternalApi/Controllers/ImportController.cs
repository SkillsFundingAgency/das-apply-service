using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Import;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    public class ImportController : Controller
    {
        private readonly IMediator _mediator;

        public ImportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/Import/Workflow")]
        public async Task<ActionResult> Workflow()
        {
            if (!HttpContext.Request.HasFormContentType || !HttpContext.Request.Form.Files.Any())
            {
                return BadRequest();
            }
            
            var importFile = HttpContext.Request.Form.Files.First();

            await _mediator.Send(new ImportWorkflowRequest(importFile));

            return Ok();
        }
    }

    
}