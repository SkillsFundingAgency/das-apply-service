using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Constants;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.ViewModels;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersApiClient _usersApiClient;
        private readonly ISessionService _sessionService;
        private readonly IReapplicationCheckService _reapplicationCheckService;
        private readonly IApplyConfig _applyConfig;
        private readonly IConfiguration _configuration;
        private readonly IStubAuthenticationService _stubAuthenticationService;

        public UsersController(IUsersApiClient usersApiClient, ISessionService sessionService, IReapplicationCheckService reapplicationCheckService, IApplyConfig applyConfig, IConfiguration configuration, IStubAuthenticationService stubAuthenticationService)
        { 
            _usersApiClient = usersApiClient;
            _sessionService = sessionService;
            _reapplicationCheckService = reapplicationCheckService;
            _applyConfig = applyConfig;
            _configuration = configuration;
            _stubAuthenticationService = stubAuthenticationService;
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
        //[Authorize]
        [Route("SignIn")]
        public IActionResult SignIn()
        {
            _ = bool.TryParse(_configuration["StubAuth"], out var stubAuth);
            var authenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme;
            if (stubAuth)
            {
                authenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme;
            }

            return Challenge(new AuthenticationProperties() { RedirectUri = Url.Action("AddUserDetails", "Users") },
                authenticationSchemes);
        }

        [HttpGet]
        public IActionResult SignOut()
        {
            HttpContext.Session.Clear();
            foreach (var cookie in HttpContext.Request.Cookies.Keys)
            {
                HttpContext.Response.Cookies.Delete(cookie);
            }

            if (User.Identity is { IsAuthenticated: false })
            {
                // If they are no longer authenticated then the cookie has expired. Don't try to signout.
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var schemes = new List<string>
                {
                    CookieAuthenticationDefaults.AuthenticationScheme
                };

                _ = bool.TryParse(_configuration["StubAuth"], out var stubAuth);

                if (!stubAuth)
                {
                    schemes.Add(OpenIdConnectDefaults.AuthenticationScheme);
                }
                var authenticationProperties = new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Index", "Home")
                };

                return SignOut(authenticationProperties, schemes.ToArray());
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

        [HttpGet]
        [Route("account-details", Name = RouteNames.StubAccountDetailsGet)]
        public IActionResult AccountDetails([FromQuery] string returnUrl)
        {
            if (_configuration["ResourceEnvironmentName"].ToUpper() == "PRD")
            {
                return NotFound();
            }
            return View("AccountDetails", new StubAuthenticationViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [Route("account-details", Name = RouteNames.StubAccountDetailsPost)]
        public async Task<IActionResult> AccountDetails(StubAuthenticationViewModel model)
        {
            if (_configuration["ResourceEnvironmentName"].ToUpper() == "PRD")
            {
                return NotFound();
            }

            var claims = await _stubAuthenticationService.GetStubSignInClaims(model);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claims,
                new AuthenticationProperties());

            return RedirectToRoute(RouteNames.StubSignedIn, new { returnUrl = model.ReturnUrl });
        }

        [HttpGet]
        [Authorize]
        [Route("Stub-Auth", Name = RouteNames.StubSignedIn)]
        public IActionResult StubSignedIn([FromQuery] string returnUrl)
        {
            if (_configuration["ResourceEnvironmentName"].ToUpper() == "PRD")
            {
                return NotFound();
            }
            var viewModel = new AccountStubViewModel
            {
                Email = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))?.Value,
                Id = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value,
                ReturnUrl = string.IsNullOrEmpty(returnUrl) ? "/users/PostSignIn" : returnUrl
            };
            return View(viewModel);
        }
    }
}