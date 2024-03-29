﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.EmailService.Interfaces;
using SFA.DAS.ApplyService.Types;
using SFA.DAS.ApplyService.Web.Controllers.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.UnitTests.Controllers
{
    [TestFixture]
    public class RoatpOverallOutcomeControllerTests
    {
        private readonly Guid _applicationId = Guid.NewGuid();

        private Mock<IOutcomeApiClient> _outcomeApiClient;
        private Mock<IOverallOutcomeService> _outcomeService;
        private Mock<IApplicationApiClient> _applicationApiClient;


        private RoatpOverallOutcomeController _controller;

        [SetUp]
        public void Before_each_test()
        {
            _outcomeService = new Mock<IOverallOutcomeService>();
            _outcomeApiClient = new Mock<IOutcomeApiClient>();
            _applicationApiClient = new Mock<IApplicationApiClient>();
         
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

            _controller = new RoatpOverallOutcomeController(_outcomeService.Object, _outcomeApiClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = user }
                }
            };
        }

        [Test]
        public async Task Application_shows_confirmation_page_if_application_GatewayAssessed()
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.GatewayAssessed
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationSubmitted.cshtml");
        }

        [Test]
        public async Task Application_shows_outcome_in_progress_page_if_application_InProgressOutcome()
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.InProgressOutcome
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationInProgress.cshtml");
        }

        [Test]
        public async Task Application_shows_already_active_success_page_if_application_approved_and_oversight_review_status_already_active()
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.Successful,
                OversightReviewStatus = OversightReviewStatus.SuccessfulAlreadyActive
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);
        
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationApprovedAlreadyActive.cshtml");
        }



        [TestCase(100000,"100,000")]
        [TestCase(500000, "500,000")]
        public async Task Application_shows_approved_supporting_page_with_formatted_amount_if_application_approved_and_successful_and_route_is_supporting(int subcontractorLimit, string subcontractLimitFormatted)
        {
            var supportingRouteId = Domain.Roatp.ApplicationRoute.SupportingProviderApplicationRoute;

            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.Successful,
                ApplicationRouteId = supportingRouteId.ToString(),
                SubcontractingLimit = subcontractorLimit  
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationApprovedSupporting.cshtml");

            var modelReturned = viewResult.Model as ApplicationSummaryViewModel;
            modelReturned.SubcontractingLimit.Should().Be(subcontractorLimit);
            modelReturned.SubcontractingLimitFormatted.Should().Be(subcontractLimitFormatted);
        }
        
    [TestCase( OversightReviewStatus.Successful, null, "ApplicationApprovedSupporting.cshtml",100000, "100,000")]
    [TestCase(OversightReviewStatus.Successful, null, "ApplicationApprovedSupporting.cshtml", 500000, "500,000")]
    [TestCase(OversightReviewStatus.Unsuccessful, AppealStatus.Successful, "ApplicationApprovedSupporting.cshtml", 100000, "100,000")]
    [TestCase(OversightReviewStatus.Unsuccessful, AppealStatus.Successful, "ApplicationApprovedSupporting.cshtml", 500000, "500,000")]
    [TestCase(OversightReviewStatus.SuccessfulFitnessForFunding, null, "ApplicationApprovedSupportingFitnessForFunding.cshtml", 100000, "100,000")]
    [TestCase(OversightReviewStatus.SuccessfulFitnessForFunding, null, "ApplicationApprovedSupportingFitnessForFunding.cshtml", 500000, "500,000")]
    [TestCase(OversightReviewStatus.Unsuccessful, AppealStatus.SuccessfulFitnessForFunding, "ApplicationApprovedSupportingFitnessForFunding.cshtml", 100000, "100,000")]
    [TestCase(OversightReviewStatus.Unsuccessful, AppealStatus.SuccessfulFitnessForFunding, "ApplicationApprovedSupportingFitnessForFunding.cshtml", 500000, "500,000")]
    [TestCase(OversightReviewStatus.SuccessfulAlreadyActive, null, "ApplicationApprovedSupportingAlreadyActive.cshtml", 100000, "100,000")]
    [TestCase(OversightReviewStatus.SuccessfulAlreadyActive, null, "ApplicationApprovedSupportingAlreadyActive.cshtml", 500000, "500,000")]
    [TestCase(OversightReviewStatus.Unsuccessful, AppealStatus.SuccessfulAlreadyActive, "ApplicationApprovedSupportingAlreadyActive.cshtml", 100000, "100,000")]
    [TestCase(OversightReviewStatus.Unsuccessful, AppealStatus.SuccessfulAlreadyActive, "ApplicationApprovedSupportingAlreadyActive.cshtml", 500000, "500,000")]
    public async Task Application_shows_approved_supporting_page_with_formatted_amount_that_matches_oversight_and_appeal_statuses(OversightReviewStatus oversightReviewStatus, string appealStatus, string expectedView, int subcontractorLimit, string subcontractLimitFormatted)
        {
            var supportingRouteId = Domain.Roatp.ApplicationRoute.SupportingProviderApplicationRoute;

            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.Successful,
                ApplicationRouteId = supportingRouteId.ToString(),
                SubcontractingLimit = subcontractorLimit ,
                OversightReviewStatus = oversightReviewStatus,
                AppealStatus = appealStatus
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain(expectedView);

            var modelReturned = viewResult.Model as ApplicationSummaryViewModel;
            modelReturned.SubcontractingLimit.Should().Be(subcontractorLimit);
            modelReturned.SubcontractingLimitFormatted.Should().Be(subcontractLimitFormatted);
        }

        [TestCase(OversightReviewStatus.SuccessfulFitnessForFunding, null, "ApplicationApprovedFitnessForFunding.cshtml")]
        [TestCase(OversightReviewStatus.Unsuccessful, AppealStatus.SuccessfulFitnessForFunding, "ApplicationApprovedFitnessForFunding.cshtml")]
        [TestCase(OversightReviewStatus.Successful, null, "ApplicationApproved.cshtml")]
        [TestCase(OversightReviewStatus.Unsuccessful, AppealStatus.Successful, "ApplicationApproved.cshtml")]
        [TestCase(OversightReviewStatus.SuccessfulAlreadyActive, null, "ApplicationApprovedAlreadyActive.cshtml")]
        [TestCase(OversightReviewStatus.Unsuccessful, AppealStatus.SuccessfulAlreadyActive, "ApplicationApprovedAlreadyActive.cshtml")]
        public async Task Application_shows_active_with_view_appropriate_to_oversight_or_appeal_status(OversightReviewStatus oversightReviewStatus, string appealStatus, string expectedView)
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.Successful,
                OversightReviewStatus = oversightReviewStatus,
                AppealStatus = appealStatus
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain(expectedView);
        }

        [Test]
        public async Task Application_shows_tasklist_page_if_application_new()
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.New
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("TaskList");
            redirectResult.ControllerName.Should().Be("RoatpApplication");
        }

        [Test]
        public async Task Application_shows_application_submitted_page_if_application_resubmitted()
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.Resubmitted
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationSubmitted.cshtml");
        }

        [Test]
        public async Task Application_shows_rejected_page_if_application_rejected_at_gateway()
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.GatewayAssessed,
                GatewayReviewStatus = GatewayReviewStatus.Rejected
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationRejected.cshtml");
        }

        [Test]
        public async Task Application_shows_feedback_added_page_if_application_status_matches()
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.FeedbackAdded
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("FeedbackAdded.cshtml");
        }

        [Test]
        public async Task Application_shows_task_list_if_an_application_in_progress()
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.InProgress
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("TaskList");
            redirectResult.ControllerName.Should().Be("RoatpApplication");
        }

        [Test]
        public async Task Application_shows_task_list_if_an_application_not_set()
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = null
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("TaskList");
            redirectResult.ControllerName.Should().Be("RoatpApplication");
        }

        [Test]
        public async Task Applications_shows_unsuccessful_page_if_application_unsuccessful_and_gateway_fail()
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.Unsuccessful,
                GatewayReviewStatus = GatewayReviewStatus.Fail,
                ApplicationDeterminedDate = DateTime.Today,
                AppealRequiredByDate = DateTime.Today.AddDays(10)
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationUnsuccessful.cshtml");
        }

        [Test]
        public async Task Applications_shows_unsuccessful_page_if_application_unsuccessful_and_gateway_not_a_fail()
        {
            var model = new ApplicationSummaryWithModeratorDetailsViewModel
            {
                ApplicationStatus = ApplicationStatus.Unsuccessful,
                GatewayReviewStatus = GatewayReviewStatus.Pass,
                ApplicationDeterminedDate = DateTime.Today,
                AppealRequiredByDate = DateTime.Today.AddDays(10)
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);
            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModelWithGatewayAndModerationDetails(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationUnsuccessfulPostGateway.cshtml");
        }

        [Test]
        public async Task Application_shows_withdrawn_page_if_application_withdrawn()
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.Withdrawn
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationWithdrawn.cshtml");
        }

        [Test]
        public async Task Application_shows_removed_page_if_application_removed_and_oversight_status_is_removed()
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.Removed,
                OversightReviewStatus = OversightReviewStatus.Removed
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);

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
        public async Task Application_shows_removed_page_if_application_removed_and_oversight_status_is_not_removed(OversightReviewStatus oversightStatus)
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.Removed,
                OversightReviewStatus = oversightStatus,
                ApplicationDeterminedDate = DateTime.Today,
                AppealRequiredByDate = DateTime.Today.AddDays(10)
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationSubmitted.cshtml");
        }

        [Test]
        public async Task Application_shows_submitted_page_if_application_submitted()
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.Submitted
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationSubmitted.cshtml");
        }

        [Test]
        public async Task Application_shows_submitted_page_if_application_resubmitted()
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.Resubmitted
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationSubmitted.cshtml");
        }

        [Test]
        public async Task Application_shows_appeal_submitted_page_when_appeal_submitted_and_pending_outcome()
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.Unsuccessful,
                OversightReviewStatus = OversightReviewStatus.Unsuccessful,
                ApplicationDeterminedDate = DateTime.Today,
                AppealRequiredByDate = DateTime.Today.AddDays(10),
                AppealStatus = AppealStatus.Submitted,
                IsAppealSubmitted = true
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("AppealSubmitted");
            redirectResult.ControllerName.Should().Be("RoatpAppeals");
        }

        [Test]
        public async Task Application_shows_unsuccessful_page_when_appeal_submitted_and_pending_outcome_and_application_overview_clicked()
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.Unsuccessful,
                OversightReviewStatus = OversightReviewStatus.Unsuccessful,
                ApplicationDeterminedDate = DateTime.Today,
                AppealRequiredByDate = DateTime.Today.AddDays(10),
                AppealStatus = AppealStatus.Submitted,
                IsAppealSubmitted = true
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            _controller.HttpContext.Request.Headers.Add("Referer", $"http://localhost/application/{_applicationId}/status");

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationUnsuccessfulPostGateway.cshtml");
        }

        [Test]
        public async Task Application_shows_appeal_inprogress_page_when_appeal_submitted_and_inprogress_outcome()
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.InProgressAppeal,
                OversightReviewStatus = OversightReviewStatus.Unsuccessful,
                ApplicationDeterminedDate = DateTime.Today,
                AppealRequiredByDate = DateTime.Today.AddDays(10),
                AppealStatus = AppealStatus.InProgress,
                IsAppealSubmitted = true
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("AppealInProgress");
            redirectResult.ControllerName.Should().Be("RoatpAppeals");
        }

        [Test]
        public async Task Application_shows_unsuccessful_page_when_appeal_submitted_and_inprogress_outcome_and_application_overview_clicked()
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.InProgressAppeal,
                OversightReviewStatus = OversightReviewStatus.Unsuccessful,
                ApplicationDeterminedDate = DateTime.Today,
                AppealRequiredByDate = DateTime.Today.AddDays(10),
                AppealStatus = AppealStatus.InProgress,
                IsAppealSubmitted = true
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            _controller.HttpContext.Request.Headers.Add("Referer", $"http://localhost/application/{_applicationId}/status");

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationUnsuccessfulPostGateway.cshtml");
        }

        [Test]
        public async Task Application_shows_appeal_unsuccessful_page_when_appeal_submitted_and_unsuccessful_outcome()
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.Unsuccessful,
                OversightReviewStatus = OversightReviewStatus.Unsuccessful,
                ApplicationDeterminedDate = DateTime.Today,
                AppealRequiredByDate = DateTime.Today.AddDays(10),
                AppealStatus = AppealStatus.Unsuccessful,
                IsAppealSubmitted = true
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("AppealUnsuccessful");
            redirectResult.ControllerName.Should().Be("RoatpAppeals");
        }

        [Test]
        public async Task Application_shows_appeal_successful_page_when_appeal_submitted_and_successful_outcome()
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.Unsuccessful,
                OversightReviewStatus = OversightReviewStatus.Unsuccessful,
                ApplicationDeterminedDate = DateTime.Today,
                AppealRequiredByDate = DateTime.Today.AddDays(10),
                AppealStatus = AppealStatus.Successful,
                IsAppealSubmitted = true
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("AppealSuccessful");
            redirectResult.ControllerName.Should().Be("RoatpAppeals");
        }

        [Test]
        public async Task Application_shows_unsuccessful_page_when_appeal_submitted_and_unsuccessful_outcome_and_application_overview_clicked()
        {
            var model = new ApplicationSummaryViewModel
            {
                ApplicationStatus = ApplicationStatus.Unsuccessful,
                OversightReviewStatus = OversightReviewStatus.Unsuccessful,
                ApplicationDeterminedDate = DateTime.Today,
                AppealRequiredByDate = DateTime.Today.AddDays(10),
                AppealStatus = AppealStatus.Unsuccessful,
                IsAppealSubmitted = true
            };

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(_applicationId, It.IsAny<string>())).ReturnsAsync(model);

            _controller.HttpContext.Request.Headers.Add("Referer", $"http://localhost/application/{_applicationId}/status");

            var result = await _controller.ProcessApplicationStatus(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationUnsuccessfulPostGateway.cshtml");
        }

        [Test]
        public async Task Application_sectors_show_sector_details()
        {
            var applicationId = Guid.NewGuid();
            const string pageId = "pageId";
            const string sectorName = "name of sector";

            var viewModel = new OutcomeSectorDetailsViewModel
            {
                ApplicationId = applicationId,
                SectorDetails = new SectorDetails { SectorName = sectorName }
            };

            _outcomeService.Setup(x => x.GetSectorDetailsViewModel(applicationId, pageId)).ReturnsAsync(viewModel);
            var result = await _controller.GetSectorDetails(applicationId, pageId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.Model.Should().Be(viewModel);
            viewResult.ViewName.Should().Contain("ApplicationUnsuccessfulSectorAnswers.cshtml");
        }

        [Test]
        public async Task DownloadClarificationFile_when_file_exists_downloads_the_requested_file()
        {
            var filename = "test.pdf";
            var contentType = "application/pdf";
            var applicationId = Guid.NewGuid();
            var sequenceNumber = 1;
            var sectionNumber = 2;
            var pageId = "pageId";
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new MemoryStream())
            { Headers = { ContentLength = 0, ContentType = new MediaTypeHeaderValue(contentType) } };

            _outcomeApiClient.Setup(x => x.DownloadClarificationfile(applicationId, sequenceNumber, sectionNumber, pageId, filename)).ReturnsAsync(response);
            var result = await _controller.DownloadClarificationFile(applicationId, sequenceNumber, sectionNumber, pageId, filename) as FileStreamResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(filename, result.FileDownloadName);
            Assert.AreEqual(contentType, result.ContentType);
        }

        [Test]
        public async Task DownloadClarificationFile_when_file_does_not_exists_then_gives_NotFound_result()
        {
            var filename = "test.pdf";
            var response = new HttpResponseMessage(HttpStatusCode.NotFound);
            _outcomeApiClient.Setup(x => x.DownloadClarificationfile(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), filename)).ReturnsAsync(response);
            var result = await _controller.DownloadClarificationFile(Guid.NewGuid(), 1, 2, "_pageId", filename) as NotFoundResult;
            Assert.IsNotNull(result);
        }
    }
}


