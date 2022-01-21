using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Controllers.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure.Interfaces;

namespace SFA.DAS.ApplyService.Web.UnitTests.Controllers
{
    [TestFixture]
    public class RoatpWhosInControlRefreshControllerTests
    {
        private Mock<ISessionService> _sessionService;
        private Mock<IRefreshTrusteesService> _refreshTrusteesService;
        private RoatpWhosInControlRefreshController _controller;
        
        [SetUp]
        public void Before_each_test()
        {
            _sessionService = new Mock<ISessionService>();
            _refreshTrusteesService = new Mock<IRefreshTrusteesService>();

             var signInId = Guid.NewGuid();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, $"Test user"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("Email", "test@test.com"),
                new Claim("sub", signInId.ToString()),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            _controller = new RoatpWhosInControlRefreshController(_refreshTrusteesService.Object,
                                                                      _sessionService.Object
                                                                     )
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = user },
                },
                TempData = Mock.Of<ITempDataDictionary>()
            };
        }

        [Test]
        public async Task refresh_trustees_and_redirect_if_details_not_available()
        {
            var charityNumber = "12345678";
            var applicationId = Guid.NewGuid();
            _refreshTrusteesService.Setup(x => x.RefreshTrustees(applicationId, It.IsAny<Guid>())).ReturnsAsync(new RefreshTrusteesResult { CharityDetailsNotFound = true, CharityNumber = charityNumber });

            var result = await _controller.RefreshTrustees(applicationId);
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("CharityNotFoundRefresh");

            var routeValue = redirectResult.RouteValues.FirstOrDefault(x => x.Key == "CharityNumber");
            routeValue.Value.Should().Be(charityNumber);
        }

        [Test]
        public async Task refresh_trustees_and_redirect_if_details_updated()
        {
            var charityNumber = "12345678";
            var applicationId = Guid.NewGuid();
            _refreshTrusteesService.Setup(x => x.RefreshTrustees(applicationId, It.IsAny<Guid>())).ReturnsAsync(new RefreshTrusteesResult { CharityDetailsNotFound = false, CharityNumber = charityNumber });

            var result = await _controller.RefreshTrustees(applicationId);
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("ConfirmTrustees");

            var routeValue = redirectResult.RouteValues.FirstOrDefault(x => x.Key == "applicationId");
            routeValue.Value.Should().Be(applicationId);
        }
    }
}
