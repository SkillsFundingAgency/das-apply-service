using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    
    public class UploadController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Chunks()
        {
            var a = HttpContext.Request;
            
            
            return Ok();
        }
    }
}