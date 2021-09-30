using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Web.Controllers.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using System;
using System.Collections.Generic;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.InternalApi.Types;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Validators;
using SFA.DAS.ApplyService.Web.Controllers;
using Microsoft.Extensions.Localization;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.UnitTests.Controllers
{
    [TestFixture]
    public class UsersControllerTest
    {
        private  Mock<IUsersApiClient> _usersApiClient;
        private  Mock<ISessionService> _sessionService;
        private  CreateAccountValidator _createAccountValidator;
        private CreateAccountViewModel _CreateAccountViewModel;
        private UsersController _userController;
        private Mock<IStringLocalizer<CreateAccountViewModel>>  _localizer;

        [SetUp]
        public void Before_each_test()
        {
            _usersApiClient = new Mock<IUsersApiClient>();
            _sessionService = new Mock<ISessionService>();
            _CreateAccountViewModel = new CreateAccountViewModel() { Email = "sureshkuruva@esfa.gov.co.uk", GivenName = "Suresh", FamilyName = "Kuruva" };
            _localizer = new Mock<IStringLocalizer<CreateAccountViewModel>>();       

            var validemailkey = "Email must be valid";
            var emailnotempty = "Email must not be empty";
            var  lastnamenotempty = "Last name must not be empty";
            var firstnmenotempty = "First name must not be empty";
            
            _localizer.Setup(u => u[validemailkey]).Returns(new LocalizedString(validemailkey, validemailkey));
            _localizer.Setup(u => u[emailnotempty]).Returns(new LocalizedString(emailnotempty,emailnotempty));
            _localizer.Setup(u => u[firstnmenotempty]).Returns(new LocalizedString(firstnmenotempty, firstnmenotempty));
            _localizer.Setup(u => u[lastnamenotempty]).Returns(new LocalizedString(lastnamenotempty, lastnamenotempty));


            _createAccountValidator = new CreateAccountValidator(_localizer.Object);
            _userController = new UsersController(_usersApiClient.Object,
                                                                      _sessionService.Object,
                                                                    _createAccountValidator);
           
        }
        [Test]
        public void Should_Return_To_CreateAccount()
        {
            ExistingAccountViewModel model = new ExistingAccountViewModel() { FirstTimeSignin="Y"};
            var result = _userController.ConfirmExistingAccount(model);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("CreateAccount");
        }
        [Test]
        public void Should_Return_To_SignInAccount()
        {
            
          
            ExistingAccountViewModel model = new ExistingAccountViewModel() { FirstTimeSignin = "N" };
            var result = _userController.ConfirmExistingAccount(model);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("SignIn");
        }

        [Test]
        public void ExistingAccountViewModel_When_Model_Is_Not_Valid_Will_Return_Error_State()
        {

            ExistingAccountViewModel model = new ExistingAccountViewModel() { FirstTimeSignin = null };
            _userController.ModelState.AddModelError("FirstTimeSignin", "First Name is Required");

            var result = _userController.ConfirmExistingAccount(model);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("ExistingAccount");
        }

    }
}
