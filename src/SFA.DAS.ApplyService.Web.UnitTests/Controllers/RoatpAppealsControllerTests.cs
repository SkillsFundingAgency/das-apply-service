using System;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.InternalApi.Types.Responses.Oversight;
using SFA.DAS.ApplyService.Types;
using SFA.DAS.ApplyService.Web.Controllers.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp.Appeals;

namespace SFA.DAS.ApplyService.Web.UnitTests.Controllers
{
    [TestFixture]
    public class RoatpAppealsControllerTests
    {
        private Guid _applicationId;
        private DateTime _applicationDeterminedDate;

        private RoatpAppealsController _controller;
        private Mock<IOutcomeApiClient> _apiClient;

        [SetUp]
        public void Before_each_test()
        {
            _applicationId = Guid.NewGuid();
            _applicationDeterminedDate = DateTime.Today;

            _apiClient = new Mock<IOutcomeApiClient>();

            var signInId = Guid.NewGuid();
            var givenNames = "Test";
            var familyName = "User";

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, $"{givenNames} {familyName}"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("Email", "test@test.com"),
                new Claim("sub", signInId.ToString()),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            var oversightReview = new GetOversightReviewResponse { Status = OversightReviewStatus.Unsuccessful, ApplicationDeterminedDate = _applicationDeterminedDate };
            _apiClient.Setup(x => x.GetOversightReview(_applicationId)).ReturnsAsync(oversightReview);

            _apiClient.Setup(x => x.GetWorkingDaysAheadDate(It.IsAny<DateTime>(), It.IsAny<int>())).ReturnsAsync(_applicationDeterminedDate.AddDays(10));

            _controller = new RoatpAppealsController(_apiClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() {User = user}
                }
            };
        }

        [Test]
        public async Task MakeAppeal_shows_make_appeal_page_if_within_appeal_window()
        {
            var model = new MakeAppealViewModel { ApplicationId = _applicationId };

            var result = await _controller.MakeAppeal(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("MakeAppeal.cshtml");
            viewResult.Model.Should().BeEquivalentTo(model);
        }

        [Test]
        public async Task MakeAppeal_shows_Tasklist_page_if_outside_appeal_window()
        {
            var oversightReview = new GetOversightReviewResponse { Status = OversightReviewStatus.Unsuccessful, ApplicationDeterminedDate = _applicationDeterminedDate };
            _apiClient.Setup(x => x.GetOversightReview(_applicationId)).ReturnsAsync(oversightReview);
            _apiClient.Setup(x => x.GetWorkingDaysAheadDate(It.IsAny<DateTime>(), It.IsAny<int>())).ReturnsAsync(_applicationDeterminedDate.AddDays(-10));

            var result = await _controller.MakeAppeal(_applicationId);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("TaskList");
        }

        [Test]
        public async Task GroundsOfAppeal_shows_make_appeal_page_if_within_appeal_window()
        {
            var _appealOnPolicyOrProcesses = false;
            var _appealOnEvidenceSubmitted = false;

            var model = new GroundsOfAppealViewModel { ApplicationId = _applicationId, AppealOnPolicyOrProcesses = _appealOnPolicyOrProcesses, AppealOnEvidenceSubmitted = _appealOnEvidenceSubmitted };

            var result = await _controller.GroundsOfAppeal(_applicationId, _appealOnPolicyOrProcesses, _appealOnEvidenceSubmitted);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("GroundsOfAppeal.cshtml");
            viewResult.Model.Should().BeEquivalentTo(model);
        }

        [Test]
        public async Task GroundsOfAppeal_shows_Tasklist_page_if_outside_appeal_window()
        {
            var _appealOnPolicyOrProcesses = false;
            var _appealOnEvidenceSubmitted = false;

            var oversightReview = new GetOversightReviewResponse { Status = OversightReviewStatus.Unsuccessful, ApplicationDeterminedDate = _applicationDeterminedDate };
            _apiClient.Setup(x => x.GetOversightReview(_applicationId)).ReturnsAsync(oversightReview);
            _apiClient.Setup(x => x.GetWorkingDaysAheadDate(It.IsAny<DateTime>(), It.IsAny<int>())).ReturnsAsync(_applicationDeterminedDate.AddDays(-10));

            var result = await _controller.GroundsOfAppeal(_applicationId, _appealOnPolicyOrProcesses, _appealOnEvidenceSubmitted);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("TaskList");
        }
    }
}
