using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Web.Infrastructure;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class ImportController : Controller
    {
        private readonly IApplicationApiClient _apiClient;

        public ImportController(IApplicationApiClient apiClient)
        {
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
            var formCollection = HttpContext.Request.Form;
            await _apiClient.ImportWorkflow(formCollection.Files.First());
            
            return View("~/Views/Import/ImportWorkflow.cshtml");
        }
    }
}