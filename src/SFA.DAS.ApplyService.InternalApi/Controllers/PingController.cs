using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
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