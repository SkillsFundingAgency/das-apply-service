using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly UsersApiClient _usersApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISessionService _sessionService;

        public UsersController(UsersApiClient usersApiClient, IHttpContextAccessor httpContextAccessor, ISessionService sessionService)
        {
            _usersApiClient = usersApiClient;
            _httpContextAccessor = httpContextAccessor;
            _sessionService = sessionService;
        }
        
        [HttpGet]
        public async Task<IActionResult> CreateAccount()
        {
            var vm = new CreateAccountViewModel();
            return View(vm);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            
            await _usersApiClient.InviteUser(vm);

            TempData["NewAccount"] = JsonConvert.SerializeObject(vm);
            
            return RedirectToAction("InviteSent");
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return Challenge(new AuthenticationProperties() {RedirectUri = Url.Action("PostSignIn", "Users")},
                OpenIdConnectDefaults.AuthenticationScheme);
        }
        
        [HttpGet]
        public IActionResult SignOut()
        {
            //var callbackUrl = Url.Action(nameof(SignedOut), "Account", values: null, protocol: Request.Scheme);

            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            return SignOut(CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        public IActionResult InviteSent()
        {
            var email =  JsonConvert.DeserializeObject<CreateAccountViewModel>(TempData["NewAccount"].ToString());
            return View(email);
        }

        public async Task<IActionResult> PostSignIn()
        {
            var user = await _usersApiClient.GetUserBySignInId(_httpContextAccessor.HttpContext.User.FindFirstValue("sub"));
         
            _sessionService.Set("LoggedInUser", $"{user.GivenNames} {user.FamilyName}");
            
            return RedirectToAction("Index", "Application");
        }
    }
}