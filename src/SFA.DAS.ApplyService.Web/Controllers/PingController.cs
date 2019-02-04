using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class PingController : Controller
    {
        [HttpGet("/Ping")]
        public IActionResult Ping()
        {
            return Ok("Pong");
        }
    }
}