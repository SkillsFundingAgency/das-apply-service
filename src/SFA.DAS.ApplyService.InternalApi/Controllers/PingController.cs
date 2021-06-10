using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [AllowAnonymous]
    [ExcludeFromCodeCoverage]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PingController : Controller
    {
        [HttpGet("/Ping")]
        public IActionResult Ping()
        {
            return Ok("Pong");
        }
    }
}