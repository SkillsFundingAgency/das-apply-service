using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Web.Infrastructure;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class ImportController : Controller
    {
        private readonly ILogger<ImportController> _logger;
        private readonly IApplicationApiClient _apiClient;

        public ImportController(IApplicationApiClient apiClient, ILogger<ImportController> logger)
        {
            _logger = logger;
            _apiClient = apiClient;
        }
        
        [HttpGet("/Import/UploadWorkflow")]
        public IActionResult UploadWorkflow()
        {
            return View("~/Views/Import/ImportWorkflow.cshtml");
        }

        [HttpPost("/Import/UploadWorkflow")]
        public async Task<IActionResult> UploadWorkflowFile()
        {
            _logger.LogInformation("UploadWorkflowFile > 1 > Start");
            var formCollection = HttpContext.Request.Form;
            _logger.LogInformation("UploadWorkflowFile > 2 > Got form collection");
            await _apiClient.ImportWorkflow(formCollection.Files.First());
            _logger.LogInformation("UploadWorkflowFile > 3 > Sent to Api");

            return RedirectToAction("UploadWorkflow");
        }
    }
}