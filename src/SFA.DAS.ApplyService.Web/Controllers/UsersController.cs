using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.Validators;
using SFA.DAS.ApplyService.Web.ViewModels;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersApiClient _usersApiClient;
        private readonly ISessionService _sessionService;
        private readonly IReapplicationCheckService _reapplicationCheckService;

        public UsersController(IUsersApiClient usersApiClient, ISessionService sessionService, IReapplicationCheckService reapplicationCheckService)
        { 
            _usersApiClient = usersApiClient;
            _sessionService = sessionService;
            _reapplicationCheckService = reapplicationCheckService;
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
            var errorMessages = CreateAccountViewModelValidator.Validate(vm);

            if (errorMessages.Any())
            {
                vm.ErrorMessages = errorMessages;
                return View(vm);
            }

            var inviteSuccess = await _usersApiClient.InviteUser(vm);

            _sessionService.Set("NewAccount", vm);

            return inviteSuccess ? RedirectToAction("InviteSent") : RedirectToAction("Error", "Home");
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = Url.Action("PostSignIn", "Users") },
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public IActionResult SignOut()
        {
            HttpContext.Session.Clear();
            foreach (var cookie in HttpContext.Request.Cookies.Keys)
            {
                HttpContext.Response.Cookies.Delete(cookie);
            }

            if (!User.Identity.IsAuthenticated)
            {
                // If they are no longer authenticated then the cookie has expired. Don't try to signout.
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var authenticationProperties = new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Index", "Home")
                };

                return SignOut(authenticationProperties, CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
            }
        }

        public IActionResult InviteSent()
        {
            var viewModel = _sessionService.Get<CreateAccountViewModel>("NewAccount");

            if (viewModel?.Email is null)
            {
                return RedirectToAction("CreateAccount");
            }

            return View(viewModel);
        }

        public async Task<IActionResult> PostSignIn()
        {
            var signInId = User.GetSignInId();
            var user = await _usersApiClient.GetUserBySignInId(signInId);

            if (user is null)
            {
                return RedirectToAction("NotSetUp");
            }
            else if (user.ApplyOrganisationId is null)
            {
                return RedirectToAction("EnterApplicationUkprn", "RoatpApplicationPreamble");
            }

            var reapplicationAllowed =
                await _reapplicationCheckService.ReapplicationAllowed(signInId, user.ApplyOrganisationId);
            
            if (reapplicationAllowed)
            {
                var ukprn = await _reapplicationCheckService.ReapplicationUkprnForUser(User.GetSignInId());

                if (string.IsNullOrEmpty(ukprn))
                {
                    return RedirectToAction("EnterApplicationUkprn", "RoatpApplicationPreamble");
                }
             }

            var reapplicationRequestedAndPending =
                await _reapplicationCheckService.ReapplicationRequestedAndPending(signInId, user.ApplyOrganisationId);

            if (reapplicationRequestedAndPending)
            {
                var applicationId = await _reapplicationCheckService.ReapplicationApplicationIdForUser(signInId);
                if (applicationId!=null && applicationId!=Guid.Empty)
                    return RedirectToAction("RequestNewInvitationRefresh", "RoatpAppeals", new { applicationId });
            }

            return RedirectToAction("Applications", "RoatpApplication", new { applicationType = ApplicationTypes.RegisterTrainingProviders });
            
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

        [HttpGet]
        [Route("first-time-apprenticeship-service")]
        public IActionResult ExistingAccount()
        {
            return View(new ExistingAccountViewModel());
        }

        [HttpPost]
        [Route("first-time-apprenticeship-service")]
        public IActionResult ConfirmExistingAccount(ExistingAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Users/ExistingAccount.cshtml", model);
            }

            if (model.FirstTimeSignin == true)
            {
                return RedirectToAction("CreateAccount");
            }
            else
            {
                return RedirectToAction("SignIn");
            }
        }
    }
}