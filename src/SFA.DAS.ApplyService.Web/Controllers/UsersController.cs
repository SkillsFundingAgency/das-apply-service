using System.Security.Claims;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Validators;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
    using System.Collections.Generic;
    using System.Linq;

    public class UsersController : Controller
    {
        private readonly IUsersApiClient _usersApiClient;
        private readonly ISessionService _sessionService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly CreateAccountValidator _createAccountValidator;

        public UsersController(IUsersApiClient usersApiClient, ISessionService sessionService, IHttpContextAccessor contextAccessor, 
                               CreateAccountValidator createAccountValidator)
        { 
            _usersApiClient = usersApiClient;
            _sessionService = sessionService;
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
            _createAccountValidator.Validate(vm).AddToModelState(ModelState, null);

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
                OpenIdConnectDefaults.AuthenticationScheme);
        }
        
        [HttpGet]
        public IActionResult SignOut()
        {
            _contextAccessor.HttpContext.Session.Clear();
            foreach (var cookie in _contextAccessor.HttpContext.Request.Cookies.Keys)
            {
                _contextAccessor.HttpContext.Response.Cookies.Delete(cookie);
            }

            if (string.IsNullOrEmpty(_contextAccessor.HttpContext.User.FindFirstValue("display_name")))
            {
                var authenticationProperties = new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Index", "Home")
                };
                return SignOut(authenticationProperties, CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
            }

            return RedirectToAction("Index", "Home");
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
            else if (user.ApplyOrganisationId is null)
            {
                return RedirectToAction("EnterApplicationUkprn", "RoatpApplicationPreamble");
            }

            var selectedApplicationType = ApplicationTypes.RegisterTrainingProviders;
            
            return RedirectToAction("Applications", "RoatpApplication", new { applicationType = selectedApplicationType });

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
        public IActionResult ConfirmExistingAccount(ExistingAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ErrorMessages = new List<ValidationErrorDetail>();

                var modelErrors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var modelError in modelErrors)
                {
                    model.ErrorMessages.Add(new ValidationErrorDetail
                    {
                        Field = "ExistingAccount",
                        ErrorMessage = modelError.ErrorMessage
                    });
                }

                return View("~/Views/Users/ExistingAccount.cshtml", model);
            }

            if (model.FirstTimeSignin == "Y")
            {
                return RedirectToAction("CreateAccount");
            }

            return RedirectToAction("SignIn");
        }
    }
}