using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class UsersController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> CreateAccount()
        {
            var vm = new CreateAccountViewModel();
            return View(vm);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountViewModel vm)
        {
            if (ModelState.IsValid)
            {
                // Call API to create account and invite user to dfe SignIn.
                return RedirectToAction("InviteSent");
            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return Challenge(new AuthenticationProperties() {RedirectUri = Url.Action("PostSignIn", "Users")},
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        public IActionResult InviteSent()
        {
            return View();
        }

        public IActionResult PostSignIn()
        {
            return RedirectToAction("Index", "Application");
        }
    }
}