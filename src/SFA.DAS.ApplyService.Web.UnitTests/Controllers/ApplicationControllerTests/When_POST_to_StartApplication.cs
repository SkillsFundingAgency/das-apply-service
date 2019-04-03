using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Controllers;
using SFA.DAS.ApplyService.Web.Infrastructure;

namespace SFA.DAS.ApplyService.Web.UnitTests.Controllers.ApplicationControllerTests
{
    [TestFixture]
    public class When_POST_to_StartApplication
    {
        [Test]
        public async Task Then_RedirectToActionResult_is_returned()
        {
            var applicationApiClient = new Mock<IApplicationApiClient>();

            var newApplicationId = Guid.NewGuid();
            applicationApiClient.Setup(client => client.StartApplication(It.IsAny<Guid>())).ReturnsAsync(new StartApplicationResponse(){ApplicationId = newApplicationId});

            var userService = new Mock<IUserService>();
            userService.Setup(us => us.GetUserId()).ReturnsAsync(Guid.NewGuid());
            
            var controller = new ApplicationController(
                applicationApiClient.Object, 
                Mock.Of<ILogger<ApplicationController>>(), 
                Mock.Of<ISessionService>(), 
                Mock.Of<IConfigurationService>(), 
                userService.Object);

            var result = await controller.StartApplication();
            result.Should().BeOfType<RedirectToActionResult>();
            result.As<RedirectToActionResult>().ActionName.Should().Be("SequenceSignPost");
            result.As<RedirectToActionResult>().RouteValues["applicationId"].Should().Be(newApplicationId);
        }
    }
}