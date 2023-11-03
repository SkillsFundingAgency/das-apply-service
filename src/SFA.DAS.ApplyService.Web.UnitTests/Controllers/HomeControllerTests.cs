using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Web.Controllers;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.UnitTests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController _homeController;
        private Mock<IApplyConfig> _applyConfig;

        [SetUp]
        public void Before_each_test()
        {
            _applyConfig = new Mock<IApplyConfig>();
            _homeController = new HomeController(_applyConfig.Object);
        }

        [Test]
        public void Index_When_GovSignIn_True_ApplyNowLink_Should_Be_SignIn()
        {
            // arrange
            HomeIndexViewModel homeIndexViewModel = new(true);
            _applyConfig.Setup(args => args.UseGovSignIn).Returns(true);

            // sut
            var result = _homeController.Index();

            // assert
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();
            viewResult?.Model.Should().BeEquivalentTo(homeIndexViewModel);
        }

        [Test]
        public void Index_When_GovSignIn_False_ApplyNowLink_Should_Be_ExistingAccount()
        {
            // arrange
            HomeIndexViewModel homeIndexViewModel = new(false);
            _applyConfig.Setup(args => args.UseGovSignIn).Returns(false);
            
            // sut
            var result = _homeController.Index();

            // assert
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();
            viewResult?.Model.Should().BeEquivalentTo(homeIndexViewModel);
        }
    }
}
