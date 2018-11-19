using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.InternalApi.Types;
using SFA.DAS.ApplyService.Web.Infrastructure;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersApiClient _apiClient;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IUsersApiClient apiClient, ILogger<AccountController> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public IActionResult Callback([FromBody] DfeSignInCallback callback)
        {
            _apiClient.Callback(callback);
            return Ok();
        }
    }
}