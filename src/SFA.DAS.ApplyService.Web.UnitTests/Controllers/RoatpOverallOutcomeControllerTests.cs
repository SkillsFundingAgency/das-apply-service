using System;
using System.Threading.Tasks;
using FluentAssertions;
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
        private Mock<IApplicationApiClient> _apiClient;
        private Mock<IQnaApiClient> _qnaApiClient;
        private Mock<IOverallOutcomeAugmentationService> _augmentationService;
        private Mock<ILogger<RoatpOverallOutcomeController>> _logger;

        [SetUp]
        public void Before_each_test()
        {
           

            _apiClient = new Mock<IApplicationApiClient>();
            _logger = new Mock<ILogger<RoatpOverallOutcomeController>>();
            _qnaApiClient = new Mock<IQnaApiClient>();
            _augmentationService = new Mock<IOverallOutcomeAugmentationService>();
            
            _controller = new RoatpOverallOutcomeController(_apiClient.Object, _qnaApiClient.Object, _augmentationService.Object, _logger.Object);
        }


        [Test]
        public async Task Application_shows_confirmation_page_if_application_Gateway_Assessed()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.GatewayAssessed
            };

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);

            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ApplicationSubmitted");
        }


        [Test]
        public async Task Application_shows_active_with_success_page_if_application_approved_and_oversight_review_status_already_active()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Approved
            };

            var oversightReview = new GetOversightReviewResponse
            {
                Status = OversightReviewStatus.SuccessfulAlreadyActive
            };

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);
            _apiClient.Setup(x => x.GetOversightReview(It.IsAny<Guid>())).ReturnsAsync(oversightReview);

            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ApplicationApprovedAlreadyActive");
        }

        [Test]
        public async Task Application_shows_active_with_success_page_if_application_approved_and_oversight_review_status_unset()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Approved
            };

            var oversightReview = new GetOversightReviewResponse();

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);
            _apiClient.Setup(x => x.GetOversightReview(It.IsAny<Guid>())).ReturnsAsync(oversightReview);

            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ApplicationApproved");
        }

        [Test]
        public async Task Application_shows_confirmation_page_if_application_new()
        {
            var submittedApp = new Domain.Entities.Apply
            {
                ApplicationStatus = ApplicationStatus.New
            };

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);

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

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);

            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ApplicationSubmitted");
        }

        [Test]
        public async Task Application_shows_rejected_page_if_application_rejected_at_gateway()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.GatewayAssessed,
                GatewayReviewStatus = GatewayReviewStatus.Reject
            };

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);

            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ApplicationRejected");
        }

        [Test]
        public async Task Application_shows_feedback_added_page_if_application_status_matches()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.FeedbackAdded
            };
            
            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);

            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("FeedbackAdded");
        }

        [Test]
        public async Task Application_shows_task_list_if_an_application_in_progress()
        {
            var inProgressApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.InProgress
            };
          

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(inProgressApp);

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

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(inProgressApp);

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
                ApplicationStatus = ApplicationStatus.Rejected,
                GatewayReviewStatus = GatewayReviewStatus.Fail
            };

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);

            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ApplicationUnsuccessful");
        }

        [Test]
        public async Task Applications_shows_unsuccessful_page_if_application_unsuccessful_and_gateway_not_a_fail()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Rejected
            };

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);

            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ApplicationUnsuccessful");
        }

        [Test]
        public async Task Application_shows_withdrawn_page_if_application_withdrawn()
        {
            var submittedApp = new Domain.Entities.Apply
            {
                ApplicationStatus = ApplicationStatus.Withdrawn
            };

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);

            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ApplicationWithdrawn");
        }


        [Test]
        public async Task Application_shows_removed_page_if_application_removed()
        {
            var submittedApp = new Domain.Entities.Apply
            {
                ApplicationStatus = ApplicationStatus.Removed
            };

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);

            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ApplicationRemoved");
        }


        [Test]
        public async Task Application_shows_submitted_page_if_application_submitted()
        {
            var submittedApp = new Domain.Entities.Apply
            {
                ApplicationStatus = ApplicationStatus.Submitted
            };

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);

            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ApplicationSubmitted");
        }

        [Test]
        public async Task Application_shows_submitted_page_if_application_resubmitted()
        {
            var submittedApp = new Domain.Entities.Apply
            {
                ApplicationStatus = ApplicationStatus.Resubmitted
            };

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);

            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ApplicationSubmitted");
        }

    }
}
