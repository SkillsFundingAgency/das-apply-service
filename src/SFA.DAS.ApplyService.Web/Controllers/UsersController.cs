using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Validators;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersApiClient _usersApiClient;
        private readonly IApplicationApiClient _applicationApiClient;
        private readonly ISessionService _sessionService;
        private readonly ILogger<UsersController> _logger;
        private readonly IConfigurationService _config;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly CreateAccountValidator _createAccountValidator;

        public UsersController(IUsersApiClient usersApiClient, IApplicationApiClient applicationApiClient, ISessionService sessionService, ILogger<UsersController> logger, IConfigurationService config, IHttpContextAccessor contextAccessor, CreateAccountValidator createAccountValidator)
        { 
            _usersApiClient = usersApiClient;
            _applicationApiClient = applicationApiClient;
            _sessionService = sessionService;
            _logger = logger;
            _config = config;
            _contextAccessor = contextAccessor;
            _createAccountValidator = createAccountValidator;
        }
        
        [HttpGet]
        public IActionResult CreateAccount()
        {
            var vm = new CreateAccountViewModel();
            return View(vm);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountViewModel vm)
        {
            _createAccountValidator.Validate(vm);

            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            
            var inviteSuccess = await _usersApiClient.InviteUser(vm);

            _sessionService.Set("NewAccount", vm);

            return inviteSuccess ? RedirectToAction("InviteSent") : RedirectToAction("Error", "Home");
            
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return Challenge(new AuthenticationProperties() {RedirectUri = Url.Action("PostSignIn", "Users")},
                "oidc");
        }
        
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            if (!string.IsNullOrEmpty(_contextAccessor.HttpContext.User.FindFirstValue("display_name")))
            {
                return Redirect($"{(await _config.GetConfig()).AssessorServiceBaseUrl}/Account/SignOut");
            }
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }


            return SignOut("Cookies",
               "oidc");
        }

        public IActionResult InviteSent()
        {
            var viewModel = _sessionService.Get<CreateAccountViewModel>("NewAccount");

            if (viewModel?.Email is null)
            {
                RedirectToAction("CreateAccount");
            }

            return View(viewModel);
        }

        public async Task<IActionResult> PostSignIn()
        {
            var user = await _usersApiClient.GetUserBySignInId(User.GetSignInId());
           
            if (user is null)
            {
                return RedirectToAction("NotSetUp");
            }
            
            _logger.LogInformation($"Setting LoggedInUser in Session: {user.GivenNames} {user.FamilyName}");         
            _sessionService.Set("LoggedInUser", $"{user.GivenNames} {user.FamilyName}");

            if (user.ApplyOrganisationId is null)
            {
                return RedirectToAction("Index", "OrganisationSearch");
            }
            else
            {
                var org = await _applicationApiClient.GetOrganisationByUserId(user.Id);

                if (org != null)
                {
                    _logger.LogInformation($"Setting OrganisationName in Session: {org.Name}");
                    _sessionService.Set("OrganisationName", $"{org.Name}");
                }
            }
            
            return RedirectToAction("Applications", "Application");
        }

        [HttpGet("/Users/SignedOut")]
        public IActionResult SignedOut()
        {
            return View();
        }

        public IActionResult NotSetUp()
        {
            return View();
        }
    }
}