﻿using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Constants;
using SFA.DAS.ApplyService.Web.Controllers;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.ViewModels;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using SFA.DAS.GovUK.Auth.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.UnitTests.Controllers
{
    [TestFixture]
    public class UsersControllerTests
    {
        private Mock<IUsersApiClient> _usersApiClient;
        private Mock<ISessionService> _sessionService;
        private UsersController _userController;
        private Mock<IReapplicationCheckService> _reapplicationCheckService;
        private Mock<IApplyConfig> _applyConfig;
        private Guid _userSignInId;
        private Mock<IConfiguration> _configuration;
        private Mock<IStubAuthenticationService> _stubAuthService;
        private Mock<IAuthenticationService> _stubAuthenticationService;
        private Mock<IUrlHelperFactory> _urlHelperFactory;
        private ClaimsPrincipal _claimsPrincipal;

        [SetUp]
        public void Before_each_test()
        {
            _userSignInId = Guid.NewGuid();
            var givenNames = "Test";
            var familyName = "User";

            _claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, $"{givenNames} {familyName}"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("Email", "test@test.com"),
                new Claim("sub", _userSignInId.ToString()),
                new Claim("sub", _userSignInId.ToString()),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            _usersApiClient = new Mock<IUsersApiClient>();
            _sessionService = new Mock<ISessionService>();
            _reapplicationCheckService = new Mock<IReapplicationCheckService>();
            _applyConfig = new Mock<IApplyConfig>();
            _configuration = new Mock<IConfiguration>();
            _stubAuthService = new Mock<IStubAuthenticationService>();
            _stubAuthenticationService = new Mock<IAuthenticationService>();
            _urlHelperFactory = new Mock<IUrlHelperFactory>();

            _userController = new UsersController(_usersApiClient.Object, _sessionService.Object,
                _reapplicationCheckService.Object, _applyConfig.Object, _configuration.Object, _stubAuthService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = _claimsPrincipal },
                }
            };
        }

        [Test]
        public void ConfirmExistingAccount_When_FirstTimeSignin_Should_Return_To_CreateAccount()
        {
            ExistingAccountViewModel model = new ExistingAccountViewModel { FirstTimeSignin = true };
            var result = _userController.ConfirmExistingAccount(model);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("CreateAccount");
        }

        [Test]
        public void ConfirmExistingAccount_When_Not_FirstTimeSignin_Should_Return_To_SignInAccount()
        {
            ExistingAccountViewModel model = new ExistingAccountViewModel { FirstTimeSignin = false };
            var result = _userController.ConfirmExistingAccount(model);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("SignIn");
        }

        public ApplyConfig GetResult()
        {
            return new ApplyConfig { UseGovSignIn = false };
        }

        [Test]
        public void ExistingAccountViewModel_When_Invalid_FirstTimeSignin_RedirectsTo_ExistingAccount()
        {
            ExistingAccountViewModel model = new ExistingAccountViewModel { FirstTimeSignin = null };
            _userController.ModelState.AddModelError("FirstTimeSignin", "First Name is Required");

            var result = _userController.ConfirmExistingAccount(model);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ExistingAccount.cshtml");
            viewResult.Model.Should().BeEquivalentTo(model);
        }

        [Test]
        public void ExistingAccountViewModel_When_GovSignIn_False_Returns_ExistingAccount()
        {
            _applyConfig.Setup(x => x.UseGovSignIn).Returns(false);

            var result = _userController.ExistingAccount();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();
            viewResult?.Model.Should().BeOfType<ExistingAccountViewModel>();
        }

        [Test]
        public void ExistingAccountViewModel_When_GovSignIn_True_RedirectsTo_SignIn()
        {
            _applyConfig.Setup(x => x.UseGovSignIn).Returns(true);

            var result = _userController.ExistingAccount();

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult?.ActionName.Should().Contain("SignIn");
        }

        [Test]
        public void PostSignIn_user_not_set_up()
        {

            _usersApiClient.Setup(x => x.GetUserBySignInId(_userSignInId)).ReturnsAsync((Contact)null);
            var result = _userController.PostSignIn().Result;

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("NotSetUp");
        }

        [Test]
        public void PostSignIn_user_doesnt_have_applying_organisation_id_enter_ukprn()
        {
            var contact = new Contact();
            _usersApiClient.Setup(x => x.GetUserBySignInId(_userSignInId)).ReturnsAsync(contact);

          
            var result = _userController.PostSignIn().Result;

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("EnterApplicationUkprn");
        }

        [Test]
        public void PostSignIn_reapplication_allowed_not_matching_ukprn_enter_ukprn()
        {
            var contact = new Contact {ApplyOrganisationId = Guid.NewGuid()};
            _usersApiClient.Setup(x => x.GetUserBySignInId(_userSignInId)).ReturnsAsync(contact);

            _reapplicationCheckService.Setup(x => x.ReapplicationAllowed(_userSignInId, It.IsAny<Guid>())).ReturnsAsync(true);
            _reapplicationCheckService.Setup(x => x.ReapplicationUkprnForUser(_userSignInId)).ReturnsAsync((string)null);

            var result = _userController.PostSignIn().Result;

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("EnterApplicationUkprn");
        }

        [Test]
        public void PostSignIn_reapplication_allowed_matching_ukprn_go_to_applications()
        {
            var ukprn = "12345678";
            var contact = new Contact { ApplyOrganisationId = Guid.NewGuid() };
            _usersApiClient.Setup(x => x.GetUserBySignInId(_userSignInId)).ReturnsAsync(contact);

            _reapplicationCheckService.Setup(x => x.ReapplicationAllowed(_userSignInId, It.IsAny<Guid>())).ReturnsAsync(true);
            _reapplicationCheckService.Setup(x => x.ReapplicationUkprnForUser(_userSignInId)).ReturnsAsync(ukprn);

            var result = _userController.PostSignIn().Result;

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("Applications");
        }


        [Test]
        public void PostSignIn_reapplication_requested_and_pending_go_to_request_new_invitation()
        {
            var applicationId = Guid.NewGuid();
            var contact = new Contact { ApplyOrganisationId = Guid.NewGuid() };
            _usersApiClient.Setup(x => x.GetUserBySignInId(_userSignInId)).ReturnsAsync(contact);

            _reapplicationCheckService.Setup(x => x.ReapplicationAllowed(_userSignInId, It.IsAny<Guid>())).ReturnsAsync(false);
            _reapplicationCheckService.Setup(x => x.ReapplicationRequestedAndPending(_userSignInId, It.IsAny<Guid?>())).ReturnsAsync(true);
            _reapplicationCheckService.Setup(x => x.ReapplicationApplicationIdForUser(_userSignInId)).ReturnsAsync(applicationId);

            var result = _userController.PostSignIn().Result;

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("RequestNewInvitationRefresh");
        }


        [Test]
        public void PostSignIn_reapplication_requested_and_pending_no_application_go_to_applications()
        {
            var contact = new Contact { ApplyOrganisationId = Guid.NewGuid() };
            _usersApiClient.Setup(x => x.GetUserBySignInId(_userSignInId)).ReturnsAsync(contact);

            _reapplicationCheckService.Setup(x => x.ReapplicationAllowed(_userSignInId, It.IsAny<Guid>())).ReturnsAsync(false);
            _reapplicationCheckService.Setup(x => x.ReapplicationRequestedAndPending(_userSignInId, It.IsAny<Guid?>())).ReturnsAsync(true);
            _reapplicationCheckService.Setup(x => x.ReapplicationApplicationIdForUser(_userSignInId)).ReturnsAsync((Guid?)null);

            var result = _userController.PostSignIn().Result;

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("Applications");
        }


        [Test]
        public void PostSignIn_reapplication_not_allowed_go_to_applications()
        {
            var contact = new Contact { ApplyOrganisationId = Guid.NewGuid() };
            _usersApiClient.Setup(x => x.GetUserBySignInId(_userSignInId)).ReturnsAsync(contact);
            _reapplicationCheckService.Setup(x => x.ReapplicationAllowed(_userSignInId, It.IsAny<Guid>())).ReturnsAsync(false);
       
            var result = _userController.PostSignIn().Result;

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("Applications");
        }

        [Test]
        public async Task Get_AddUserDetails_Return_AddUserDetailsViewModel()
        {
            // sut
            var result = await _userController.AddUserDetails();

            // assert
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();
            viewResult?.Model.Should().BeOfType<AddUserDetailsViewModel>();
        }

        [Test]
        public async Task Get_AddUserDetails_When_ContactNotFound_Return_PostSignIn()
        {
            // arrange
            var contact = new Contact { SigninId = Guid.NewGuid() };
            _usersApiClient.Setup(x => x.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(contact);

            // sut
            var result = await _userController.AddUserDetails();

            // assert
            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult?.ActionName.Should().Be("PostSignIn");

            _usersApiClient.Verify(args => args.GetUserByEmail(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Post_AddUserDetails_When_InValid_ModelState_Return_AddUserDetailsViewModel()
        {
            // arrange
            _userController.ModelState.AddModelError("FirstName", "First Name is Required");
            _userController.ModelState.AddModelError("LastName", "Last Name is Required");
            
            // sut
            var result = await _userController.AddUserDetails(new AddUserDetailsViewModel());
            var modelState = _userController.ModelState;

            // assert
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();
            modelState.Should().NotBeNull();
            modelState.Should().HaveCount(2);
        }

        [Test]
        public async Task Post_AddUserDetails_When_ContactFound_Redirect_PostSignIn()
        {
            // arrange
            var contact = new Contact { SigninId = Guid.NewGuid() };
            _usersApiClient.Setup(x => x.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(contact);

            // sut
            var result = await _userController.AddUserDetails(new AddUserDetailsViewModel
            {
                FirstName = It.IsAny<string>(),
                LastName = It.IsAny<string>(),
            });

            // assert
            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult?.ActionName.Should().Contain("PostSignIn");

            _usersApiClient.Verify(args => args.GetUserByEmail(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Post_AddUserDetails_When_ContactNotFound_And_CreateUser_Fail_Redirect_ErrorPage()
        {
            // arrange
            _usersApiClient.Setup(x => x.GetUserByEmail(It.IsAny<string>())).ReturnsAsync((Contact)null);
            _usersApiClient.Setup(x => x.CreateUserFromAsLogin(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(), 
                It.IsAny<string>(),
                It.IsAny<Guid?>())).ReturnsAsync(false);

            // sut
            var result = await _userController.AddUserDetails(new AddUserDetailsViewModel
            {
                FirstName = string.Empty,
                LastName = string.Empty
            });

            // assert
            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult?.ActionName.Should().Contain("Error");
        }

        [Test]
        public async Task Post_AddUserDetails_When_ContactNotFound_And_CreateUser_Pass_Redirect_PostSignInPage()
        {
            // arrange
            _usersApiClient.Setup(x => x.GetUserByEmail(It.IsAny<string>())).ReturnsAsync((Contact)null);
            _usersApiClient.Setup(x => x.CreateUserFromAsLogin(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(), 
                It.IsAny<string>(),
                It.IsAny<Guid?>())).ReturnsAsync(true);

            // sut
            var result = await _userController.AddUserDetails(new AddUserDetailsViewModel
            {
                FirstName = string.Empty,
                LastName = string.Empty
            });

            // assert
            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult?.ActionName.Should().Contain("PostSignIn");
        }

        [Test]
        public void Post_Verify_AddUserDetails_Method_Is_Decorated_With_Authorize_Attribute()
        {
            var type = _userController.GetType();
            var methodInfo = type.GetMethod("AddUserDetails", new Type[] { typeof(AddUserDetailsViewModel) });
            var attributes = methodInfo?.GetCustomAttributes(typeof(AuthorizeAttribute), true);
            Assert.IsTrue(attributes?.Any(), "No AuthorizeAttribute found on AddUserDetails(AddUserDetailsViewModel model) method");
        }


        [Test]
        public void Get_Verify_AddUserDetails_Method_Is_Decorated_With_Authorize_Attribute()
        {
            var type = _userController.GetType();
            var methodInfo = type.GetMethod("AddUserDetails", Array.Empty<Type>());
            var attributes = methodInfo?.GetCustomAttributes(typeof(AuthorizeAttribute), true);
            Assert.IsTrue(attributes?.Any(), "No AuthorizeAttribute found on AddUserDetails() method");
        }

        [Ignore("Not required?")]
        public void Get_Verify_SignIn_Method_Is_Decorated_With_Authorize_Attribute()
        {
            var type = _userController.GetType();
            var methodInfo = type.GetMethod("SignIn", Array.Empty<Type>());
            var attributes = methodInfo?.GetCustomAttributes(typeof(AuthorizeAttribute), true);
            Assert.IsTrue(attributes?.Any(), "No AuthorizeAttribute found on SignIn() method");
        }

        [Test]
        public void ChangeSignInDetails_Redirect_When_UseGovSignIn_False_Then_The_Redirect_To_Home()
        {
            //arrange
            _applyConfig.Setup(args => args.UseGovSignIn).Returns(false);

            //sut
            var actual = _userController.ChangeSignInDetails() as RedirectToActionResult;

            //assert
            actual.Should().NotBeNull();
            actual?.ActionName.Should().Be("Index");
            actual?.ControllerName.Should().Be("Home");
        }

        [Test]
        public void ChangeSignInDetails_When_UseGovSignIn_True_Then_The_View_Is_Returned_With_Model()
        {
            //arrange
            _configuration.Setup(x => x["ResourceEnvironmentName"]).Returns("test");
            _applyConfig.Setup(args => args.UseGovSignIn).Returns(true);

            //sut
            var actual = _userController.ChangeSignInDetails() as ViewResult;

            //assert
            actual.Should().NotBeNull();
            var actualModel = actual?.Model as ChangeSignInDetailsViewModel;
            Assert.AreEqual("https://home.integration.account.gov.uk/settings", actualModel?.SettingsLink);
        }

        [Test]
        public async Task Then_The_Stub_Auth_Is_Created_When_Not_Prod()
        {
            var model = new StubAuthenticationViewModel();
            _configuration.Setup(x => x["ResourceEnvironmentName"]).Returns("test");
            _stubAuthService.Setup(x => x.GetStubSignInClaims(model)).ReturnsAsync(_claimsPrincipal);

            var httpContext = new DefaultHttpContext();

            var httpContextRequestServices = new Mock<IServiceProvider>();
            httpContextRequestServices.Setup(x => x.GetService(typeof(IAuthenticationService))).Returns(_stubAuthenticationService.Object);
            httpContextRequestServices.Setup(x => x.GetService(typeof(IUrlHelperFactory))).Returns(_urlHelperFactory.Object);
            httpContext.RequestServices = httpContextRequestServices.Object;

            var controllerContext = new ControllerContext { HttpContext = httpContext };
            _userController.ControllerContext = controllerContext;

            var actual = await _userController.AccountDetails(model) as RedirectToRouteResult;

            actual?.RouteName.Should().Be(RouteNames.StubSignedIn);
            _stubAuthService.Verify(x => x.GetStubSignInClaims(model), Times.Once);
            _stubAuthenticationService.Verify(x => x.SignInAsync(httpContext, CookieAuthenticationDefaults.AuthenticationScheme, _claimsPrincipal, It.IsAny<AuthenticationProperties>()), Times.Once);
        }

        [Test]
        public async Task Then_The_Stub_Auth_Is_Not_Created_When_Prod()
        {
            var model = new StubAuthenticationViewModel();
            _configuration.Setup(x => x["ResourceEnvironmentName"]).Returns("prd");
            var httpContext = new DefaultHttpContext();

            var httpContextRequestServices = new Mock<IServiceProvider>();
            httpContextRequestServices.Setup(x => x.GetService(typeof(IAuthenticationService))).Returns(_stubAuthenticationService.Object);
            var controllerContext = new ControllerContext { HttpContext = httpContext };
            _userController.ControllerContext = controllerContext;

            var actual = await _userController.AccountDetails(model) as NotFoundResult;

            actual.Should().NotBeNull();
            _stubAuthService.Verify(x => x.GetStubSignInClaims(It.IsAny<StubAuthenticationViewModel>()), Times.Never);
            _stubAuthenticationService.Verify(x => x.SignInAsync(httpContext, CookieAuthenticationDefaults.AuthenticationScheme, It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()), Times.Never);
        }

        [TestCase("http://someurl")]
        public void Then_The_Stub_Auth_Details_Are_Not_Returned_When_Prod(string returnUrl)
        {
            _configuration.Setup(x => x["ResourceEnvironmentName"]).Returns("prd");

            var actual = _userController.StubSignedIn(returnUrl) as NotFoundResult;

            actual.Should().NotBeNull();
        }

        [TestCase("somebody@email.com", "some name", "https://someurl", "https://someurl")]
        [TestCase("somebody@email.com", "some name", "", "/users/PostSignIn")]
        [TestCase("somebody@email.com", "some name", null, "/users/PostSignIn")]
        public void Then_The_Stub_Auth_Details_Are_Returned_When_Not_Prod(string email, string name, string returnUrl, string expectedUrl)
        {
            _configuration.Setup(x => x["ResourceEnvironmentName"]).Returns("test");
            var httpContext = new DefaultHttpContext();
            var emailClaim = new Claim(ClaimTypes.Email, email);
            var nameClaim = new Claim(ClaimTypes.NameIdentifier, name);
            var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[]
            {
                emailClaim,
                nameClaim
            })});
            httpContext.User = claimsPrinciple;
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var actual = _userController.StubSignedIn(returnUrl) as ViewResult;

            actual.Should().NotBeNull();
            var actualModel = actual?.Model as AccountStubViewModel;
            actualModel.Should().NotBeNull();
            actualModel?.Email.Should().Be(email);
            actualModel?.Id.Should().Be(name);
            actualModel?.ReturnUrl.Should().Be(expectedUrl);
        }

        [TestCase("http://someurl.com")]
        public void Then_The_Get_For_Entering_Stub_Auth_Details_Is_Returned_When_Not_Prod(string returnUrl)
        {
            _configuration.Setup(x => x["ResourceEnvironmentName"]).Returns("test");

            var actual = _userController.AccountDetails(returnUrl) as ViewResult;

            actual.Should().NotBeNull();
            var actualModel = actual?.Model as StubAuthenticationViewModel;
            actualModel?.ReturnUrl.Should().Be(returnUrl);
        }

        [TestCase("http://someurl.com")]
        public void Then_The_Get_For_Entering_Stub_Auth_Details_Is_Not_Returned_When_Prod(string returnUrl)
        {
            _configuration.Setup(x => x["ResourceEnvironmentName"]).Returns("prd");

            var actual = _userController.AccountDetails(returnUrl) as NotFoundResult;

            actual.Should().NotBeNull();
        }
    }
}
