using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Controllers;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.UnitTests.Controllers
{
    [TestFixture]
    public class UsersControllerTests
    {
        private Mock<IUsersApiClient> _usersApiClient;
        private Mock<ISessionService> _sessionService;
        private UsersController _userController;

        [SetUp]
        public void Before_each_test()
        {
            _usersApiClient = new Mock<IUsersApiClient>();
            _sessionService = new Mock<ISessionService>();

            _userController = new UsersController(_usersApiClient.Object, _sessionService.Object);
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

        [Test]
        public void ExistingAccountViewModel_When_Invalid_FirstTimeSignin_RedirectsTo_ExistingAccount()
        {
            ExistingAccountViewModel model = new ExistingAccountViewModel { FirstTimeSignin = null };
            _userController.ModelState.AddModelError("FirstTimeSignin", "First Name is Required");

            var result = _userController.ConfirmExistingAccount(model);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("ExistingAccount");
        }
    }
}
