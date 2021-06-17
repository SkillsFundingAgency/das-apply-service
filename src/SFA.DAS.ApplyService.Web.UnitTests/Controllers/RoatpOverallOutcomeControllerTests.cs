using System;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types.Responses.Oversight;
using SFA.DAS.ApplyService.Types;
using SFA.DAS.ApplyService.Web.Controllers.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;

namespace SFA.DAS.ApplyService.Web.UnitTests.Controllers
{
    [TestFixture]
    public class RoatpOverallOutcomeControllerTests
    {
        private RoatpOverallOutcomeController _controller;
        private Mock<IOutcomeApiClient> _apiClient;
        private Mock<IApplicationApiClient> _applicationApiClient;
        private Mock<IQnaApiClient> _qnaApiClient;
        private Mock<IOverallOutcomeService> _outcomeService;
        private Mock<ILogger<RoatpOverallOutcomeController>> _logger;

        [SetUp]
        public void Before_each_test()
        {
            _apiClient = new Mock<IOutcomeApiClient>();
            _logger = new Mock<ILogger<RoatpOverallOutcomeController>>();
            _applicationApiClient = new Mock<IApplicationApiClient>();
            _outcomeService = new Mock<IOverallOutcomeService>();

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

            _controller = new RoatpOverallOutcomeController(_apiClient.Object,
                _outcomeService.Object, _applicationApiClient.Object, _logger.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() {User = user}
                }
            };
        }

        [Test]
        public async Task Application_shows_confirmation_page_if_application_Gateway_Assessed()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.GatewayAssessed
            };

            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);

            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationSubmitted.cshtml");
        }

        [Test]
        public async Task Application_shows_active_with_success_page_if_application_approved_and_oversight_review_status_already_active()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Successful
            };
        
            var oversightReview = new GetOversightReviewResponse
            {
                Status = OversightReviewStatus.SuccessfulAlreadyActive
            };
        
            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);
            _apiClient.Setup(x => x.GetOversightReview(It.IsAny<Guid>())).ReturnsAsync(oversightReview);
        
            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());
        
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationApprovedAlreadyActive.cshtml");
        }

        [Test]
        public async Task Application_shows_active_with_success_fintess_for_funging_page_if_application_approved_and_oversight_review_status_already_active()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Successful
            };

            var oversightReview = new GetOversightReviewResponse
            {
                Status = OversightReviewStatus.SuccessfulFitnessForFunding
            };

            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);
            _apiClient.Setup(x => x.GetOversightReview(It.IsAny<Guid>())).ReturnsAsync(oversightReview);

            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationApprovedFitnessForFunding.cshtml");
        }

        [Test]
        public async Task Application_shows_active_with_success_page_if_application_approved_and_oversight_review_status_unset()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Successful
            };
        
            var oversightReview = new GetOversightReviewResponse();
        
            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);
            _apiClient.Setup(x => x.GetOversightReview(It.IsAny<Guid>())).ReturnsAsync(oversightReview);
        
            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationApproved.cshtml");
        }
        
        [Test]
        public async Task Application_shows_confirmation_page_if_application_new()
        {
            var submittedApp = new Domain.Entities.Apply
            {
                ApplicationStatus = ApplicationStatus.New
            };
        
            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);
        
            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());
        
            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("TaskList");
            redirectResult.ControllerName.Should().Be("RoatpApplication");
        }
        
        [Test]
        public async Task Application_shows_confirmation_page_if_application_resubmitted()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Resubmitted
            };
        
            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);
        
            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());
        
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationSubmitted.cshtml");
        }
        
        [Test]
        public async Task Application_shows_rejected_page_if_application_rejected_at_gateway()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.GatewayAssessed,
                GatewayReviewStatus = GatewayReviewStatus.Rejected
            };
        
            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);
        
            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());
        
            var viewRe = result as ViewResult;
            viewRe.Should().NotBeNull();
            viewRe.ViewName.Should().Contain("ApplicationRejected.cshtml");
        }
        
        [Test]
        public async Task Application_shows_feedback_added_page_if_application_status_matches()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.FeedbackAdded
            };
            
            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);
        
            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());
        
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("FeedbackAdded.cshtml");
        }
        
        [Test]
        public async Task Application_shows_task_list_if_an_application_in_progress()
        {
            var inProgressApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.InProgress
            };
            
            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(inProgressApp);
        
            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());
        
            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("TaskList");
            redirectResult.ControllerName.Should().Be("RoatpApplication");
        }
        
        [Test]
        public async Task Application_shows_task_list_if_an_application_not_set()
        {
            var inProgressApp = new Apply();
        
            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(inProgressApp);
        
            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());
        
            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("TaskList");
            redirectResult.ControllerName.Should().Be("RoatpApplication");
        }
        
        [Test]
        public async Task Applications_shows_unsuccessful_page_if_application_unsuccessful_and_gateway_fail()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Unsuccessful,
                GatewayReviewStatus = GatewayReviewStatus.Fail
            };
        
            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);
        
            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());
        
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationUnsuccessful.cshtml");
        }
        
        [Test]
        public async Task Applications_shows_unsuccessful_page_if_application_unsuccessful_and_gateway_not_a_fail()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Unsuccessful
            };
        
            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);
        
            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());
        
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationUnsuccessfulPostGateway.cshtml");
        }
        
        [Test]
        public async Task Application_shows_withdrawn_page_if_application_withdrawn()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Withdrawn
            };
        
            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);
        
            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());
        
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationWithdrawn.cshtml");
        }
        
        [Test]
        public async Task Application_shows_removed_page_if_application_removed_and_oversight_status_is_removed()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Removed
            };

            var oversightReview = new GetOversightReviewResponse {Status = OversightReviewStatus.Removed};

            _apiClient.Setup(x => x.GetOversightReview(It.IsAny<Guid>())).ReturnsAsync(oversightReview);
            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);

            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());
        
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationWithdrawnESFA.cshtml");
        }

        [TestCase(OversightReviewStatus.Unsuccessful)]
        [TestCase(OversightReviewStatus.Successful)]
        [TestCase(OversightReviewStatus.Rejected)]
        [TestCase(OversightReviewStatus.SuccessfulFitnessForFunding)]
        [TestCase(OversightReviewStatus.None)]
        [TestCase(OversightReviewStatus.InProgress)]
        [TestCase(OversightReviewStatus.SuccessfulAlreadyActive)]
        [TestCase(OversightReviewStatus.Withdrawn)]
        public async Task Application_shows_removed_page_if_application_removed_and_oversight_status_is_not_removed(OversightReviewStatus status)
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Removed
            };

            var oversightReview = new GetOversightReviewResponse { Status = status };
            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);
            _apiClient.Setup(x => x.GetOversightReview(It.IsAny<Guid>())).ReturnsAsync(oversightReview);
            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationSubmitted.cshtml");
        }

        [Test]
        public async Task Application_shows_submitted_page_if_application_submitted()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Submitted
            };
        
            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);
        
            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());
        
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationSubmitted.cshtml");
        }
        
        [Test]
        public async Task Application_shows_submitted_page_if_application_resubmitted()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Resubmitted
            };
        
            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);
        
            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());
        
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationSubmitted.cshtml");
        }
    }
}
