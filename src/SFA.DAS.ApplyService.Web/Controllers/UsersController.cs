using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Constants;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.ViewModels;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersApiClient _usersApiClient;
        private readonly ISessionService _sessionService;
        private readonly IReapplicationCheckService _reapplicationCheckService;
        private readonly IApplyConfig _applyConfig;
        private readonly IConfiguration _configuration;

        public UsersController(IUsersApiClient usersApiClient, ISessionService sessionService, IReapplicationCheckService reapplicationCheckService, IApplyConfig applyConfig, IConfiguration configuration)
        { 
            _usersApiClient = usersApiClient;
            _sessionService = sessionService;
            _reapplicationCheckService = reapplicationCheckService;
            _applyConfig = applyConfig;
            _configuration = configuration;
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
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var inviteSuccess = await _usersApiClient.InviteUser(vm);

            _sessionService.Set("NewAccount", vm);

            return inviteSuccess ? RedirectToAction("InviteSent") : RedirectToAction("Error", "Home", new { statusCode = 555});
        }

        [HttpGet]
        [Authorize]
        [Route("SignIn")]
        public IActionResult SignIn()
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = Url.Action("AddUserDetails", "Users") },
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
        [Route(RouteNames.ExistingAccount)]
        public IActionResult ExistingAccount()
        {
            // redirect the user to SignIn page if GovSignIn is enabled.
            if (_applyConfig.UseGovSignIn) return RedirectToAction("SignIn");

            return View(new ExistingAccountViewModel());
        }

        [HttpPost]
        [Route(RouteNames.ExistingAccount)]
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

        [HttpGet]
        [Authorize]
        [Route(RouteNames.AddUserDetails)]
        public async Task<IActionResult> AddUserDetails()
        {
            // find the contact by claims email address
            var email = User.GetEmail();
            if (!string.IsNullOrEmpty(email))
            {
                var contact = await _usersApiClient.GetUserByEmail(email);

                // if the contact is found, redirect the user to PostSignIn page.
                if (contact is not null) return RedirectToAction("PostSignIn");
            }
            return View(new AddUserDetailsViewModel());
        }

        [HttpPost]
        [Authorize]
        [Route(RouteNames.AddUserDetails)]
        public async Task<IActionResult> AddUserDetails(AddUserDetailsViewModel vm)
        {
            // if the model state is invalid and got validation errors, throw the model state error.
            if (!ModelState.IsValid) return View(vm);

            // find the contact by claims email address
            var email = User.GetEmail();
            var contact = await _usersApiClient.GetUserByEmail(email);

            // if the contact is not found then create the contact based on the collected information.
            if (contact is null)
            {
                // create the user using the internal Apis.
                var isUserCreated = await _usersApiClient.CreateUserFromAsLogin(
                    signInId: User.GetSignInId(),
                    email: email,
                    vm.FirstName,
                    vm.LastName);

                // if the contact could not created, redirect the user to the error page.
                if (!isUserCreated) return RedirectToAction("Error", "Home", new { statusCode = 555 });
            }

            _sessionService.Set("AddUserDetails", vm);

            return RedirectToAction("PostSignIn");
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangeSignInDetails()
        {
            // redirect the user to home page if UseGovSignIn is set false.
            if (!_applyConfig.UseGovSignIn) return RedirectToAction("Index", "Home");

            return View(new ChangeSignInDetailsViewModel(_configuration["ResourceEnvironmentName"]));
        }
    }
}