using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Services;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types.Responses.Oversight;
using SFA.DAS.ApplyService.Types;
using SFA.DAS.ApplyService.Web.Controllers.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp.Appeals;

namespace SFA.DAS.ApplyService.Web.UnitTests.Controllers
{
    [TestFixture]
    public class RoatpOverallOutcomeControllerTests
    {
        private RoatpOverallOutcomeController _controller;
        private Mock<IOutcomeApiClient> _apiClient;
        private Mock<IApplicationApiClient> _applicationApiClient;
        private Mock<IOverallOutcomeService> _outcomeService;
        private Mock<ILogger<RoatpOverallOutcomeController>> _logger;
        private Mock<IBankHolidayService> _bankHolidayService;


        [SetUp]
        public void Before_each_test()
        {
            _apiClient = new Mock<IOutcomeApiClient>();
            _logger = new Mock<ILogger<RoatpOverallOutcomeController>>();
            _applicationApiClient = new Mock<IApplicationApiClient>();
            _outcomeService = new Mock<IOverallOutcomeService>();
            _bankHolidayService=new Mock<IBankHolidayService>();
            var appealRequiredByDate = DateTime.Today.AddDays(10);

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
            
            _bankHolidayService.Setup((x => x.GetWorkingDaysAheadDate(It.IsAny<DateTime>(), It.IsAny<int>())))
                .Returns(appealRequiredByDate);


            _controller = new RoatpOverallOutcomeController(_apiClient.Object,
                _outcomeService.Object, _applicationApiClient.Object, _logger.Object, _bankHolidayService.Object)
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
        public async Task Application_shows_confirmation_page_if_application_in_outcome_progress()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.InProgressOutcome
            };

            var externalComments = "external comments";

            var oversightReview = new GetOversightReviewResponse
            {
                InProgressExternalComments = externalComments
            };

            var model = new ApplicationSummaryViewModel();

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(submittedApp, "test@test.com")).Returns(model);

            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);
            _apiClient.Setup(x => x.GetOversightReview(It.IsAny<Guid>())).ReturnsAsync(oversightReview);


            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationInProgress.cshtml");
            var modelReturned = viewResult.Model as ApplicationSummaryViewModel;
            modelReturned.OversightInProgressExternalComments.Should().Be(externalComments);
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


            var model = new ApplicationSummaryViewModel();
            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(submittedApp, "test@test.com")).Returns(model);
            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);
            _apiClient.Setup(x => x.GetOversightReview(It.IsAny<Guid>())).ReturnsAsync(oversightReview);
        
            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());
        
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationApprovedAlreadyActive.cshtml");
        }



        [TestCase(100000,"100,000")]
        [TestCase(500000, "500,000")]
        public async Task Application_shows_supporting_success_page_with_formatted_amount_if_application_approved_and_successful_and_route_is_supporting(int subcontractorLimit, string subcontractLimitFormatted)
        {
            var supportingRouteId = 3;
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Successful,
                ApplyData = new ApplyData
                {
                    ApplyDetails = new ApplyDetails
                    {
                        ProviderRoute = supportingRouteId
                    }
                }
            };

            var model = new ApplicationSummaryViewModel
            {
                SubcontractingLimit = subcontractorLimit,
                ApplicationRouteId = supportingRouteId.ToString()
            };

            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);
            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(submittedApp, "test@test.com")).Returns(model);

            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationApprovedSupporting.cshtml");
            var modelReturned = viewResult.Model as ApplicationSummaryViewModel;
            modelReturned.SubcontractingLimit.Should().Be(subcontractorLimit);
            modelReturned.SubcontractingLimitFormatted.Should().Be(subcontractLimitFormatted);
        }

        [TestCase(100000, "100,000")]
        [TestCase(500000, "500,000")]
        public async Task Application_shows_supporting_success_page_with_formatted_amount_if_application_approved_and_successful_fitness_for_funding_and_route_is_supporting(int subcontractorLimit, string subcontractLimitFormatted)
        {
            var supportingRouteId = 3;
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Successful,
                ApplyData = new ApplyData
                {
                    ApplyDetails = new ApplyDetails
                    {
                        ProviderRoute = supportingRouteId
                    }
                }
            };

            var model = new ApplicationSummaryViewModel
            {
                SubcontractingLimit = subcontractorLimit,
                ApplicationRouteId = supportingRouteId.ToString()
            };

            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);
            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(submittedApp, "test@test.com")).Returns(model);
            var oversightReview = new GetOversightReviewResponse
            {
                Status = OversightReviewStatus.SuccessfulFitnessForFunding
            };
            _apiClient.Setup(x => x.GetOversightReview(It.IsAny<Guid>())).ReturnsAsync(oversightReview);

            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationApprovedSupportingFitnessForFunding.cshtml");
            var modelReturned = viewResult.Model as ApplicationSummaryViewModel;
            modelReturned.SubcontractingLimit.Should().Be(subcontractorLimit);
            modelReturned.SubcontractingLimitFormatted.Should().Be(subcontractLimitFormatted);
        }

        [TestCase(100000, "100,000")]
        [TestCase(500000, "500,000")]
        public async Task Application_shows_supporting_success_page_with_formatted_amount_if_application_approved_and_successful_already_active_and_route_is_supporting(int subcontractorLimit, string subcontractLimitFormatted)
        {
            var supportingRouteId = 3;
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Successful,
                ApplyData = new ApplyData
                {
                    ApplyDetails = new ApplyDetails
                    {
                        ProviderRoute = supportingRouteId
                    }
                }
            };

            var model = new ApplicationSummaryViewModel
            {
                SubcontractingLimit = subcontractorLimit,
                ApplicationRouteId = supportingRouteId.ToString()
            };

            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);
            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(submittedApp, "test@test.com")).Returns(model);
            var oversightReview = new GetOversightReviewResponse
            {
                Status = OversightReviewStatus.SuccessfulAlreadyActive
            };
            _apiClient.Setup(x => x.GetOversightReview(It.IsAny<Guid>())).ReturnsAsync(oversightReview);

            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationApprovedSupportingAlreadyActive.cshtml");
            var modelReturned = viewResult.Model as ApplicationSummaryViewModel;
            modelReturned.SubcontractingLimit.Should().Be(subcontractorLimit);
            modelReturned.SubcontractingLimitFormatted.Should().Be(subcontractLimitFormatted);
        }

        [Test]
        public async Task Application_shows_active_with_success_fitness_for_funding_page_if_application_approved_and_oversight_review_status_already_active()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Successful
            };

            var oversightReview = new GetOversightReviewResponse
            {
                Status = OversightReviewStatus.SuccessfulFitnessForFunding
            };

            var model = new ApplicationSummaryViewModel();
            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(submittedApp, "test@test.com")).Returns(model);

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

            var model = new ApplicationSummaryViewModel();
            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(submittedApp, "test@test.com")).Returns(model);

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

            var applicationDeterminedDate = DateTime.Today;
            var appealRequiredByDate = DateTime.Today.AddDays(10);
        
            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);

            var oversightReview = new GetOversightReviewResponse { Status = OversightReviewStatus.Unsuccessful, ApplicationDeterminedDate = applicationDeterminedDate };

            _apiClient.Setup(x => x.GetOversightReview(It.IsAny<Guid>())).ReturnsAsync(oversightReview);

            _bankHolidayService.Setup((x => x.GetWorkingDaysAheadDate(It.IsAny<DateTime>(), It.IsAny<int>())))
                .Returns(appealRequiredByDate);
            
            var model = new ApplicationSummaryViewModel();

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(submittedApp, "test@test.com")).Returns(model);

            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());
        
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationUnsuccessful.cshtml");

            var returnedModel = viewResult.Model as ApplicationSummaryViewModel;
            returnedModel.AppealRequiredByDate.Should().Be(appealRequiredByDate);
            returnedModel.ApplicationDeterminedDate.Should().Be(applicationDeterminedDate);

        }
        
        [Test]
        public async Task Applications_shows_unsuccessful_page_if_application_unsuccessful_and_gateway_not_a_fail()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Unsuccessful
            };
        
            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);


            var model = new ApplicationSummaryViewModel();

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(submittedApp, "test@test.com")).Returns(model);
            var modelWithModeratorDetails = new ApplicationSummaryWithModeratorDetailsViewModel();

            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModelWithGatewayAndModerationDetails(submittedApp, "test@test.com")).ReturnsAsync(modelWithModeratorDetails);

            var result = await _controller.ProcessApplicationStatus(It.IsAny<Guid>());
        
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ApplicationUnsuccessfulPostGateway.cshtml");
        }

        [Test]
        public async Task MakeAppeal_shows_make_appeal_page_if_within_appeal_window()
        {
            var _applicationId = Guid.NewGuid();

            var applicationDeterminedDate = DateTime.Today;
            var appealRequiredByDate = DateTime.Today.AddDays(10);

            var oversightReview = new GetOversightReviewResponse { Status = OversightReviewStatus.Unsuccessful, ApplicationDeterminedDate = DateTime.Today };
            _apiClient.Setup(x => x.GetOversightReview(It.IsAny<Guid>())).ReturnsAsync(oversightReview);
            _bankHolidayService.Setup(x => x.GetWorkingDaysAheadDate(It.IsAny<DateTime>(), It.IsAny<int>())).Returns(DateTime.Today.AddDays(10));

            var model = new MakeAppealViewModel {ApplicationId = _applicationId};

            var result = await _controller.MakeAppeal(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("MakeAppeal.cshtml");
            viewResult.Model.Should().BeEquivalentTo(model);
        }

        [Test]
        public async Task MakeAppeal_shows_Tasklist_page_if_outside_appeal_window()
        {
            var _applicationId = Guid.NewGuid();

            var oversightReview = new GetOversightReviewResponse { Status = OversightReviewStatus.Unsuccessful, ApplicationDeterminedDate = DateTime.Today };
            _apiClient.Setup(x => x.GetOversightReview(It.IsAny<Guid>())).ReturnsAsync(oversightReview);
            _bankHolidayService.Setup(x => x.GetWorkingDaysAheadDate(It.IsAny<DateTime>(), It.IsAny<int>())).Returns(DateTime.Today.AddDays(-10));

            var result = await _controller.MakeAppeal(_applicationId);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("TaskList");
        }

        [Test]
        public async Task GroundsOfAppeal_shows_make_appeal_page_if_within_appeal_window()
        {
            var _applicationId = Guid.NewGuid();
            var _appealOnPolicyOrProcesses = false;
            var _appealOnEvidenceSubmitted = false;

            var oversightReview = new GetOversightReviewResponse { Status = OversightReviewStatus.Unsuccessful, ApplicationDeterminedDate = DateTime.Today };
            _apiClient.Setup(x => x.GetOversightReview(It.IsAny<Guid>())).ReturnsAsync(oversightReview);
            _bankHolidayService.Setup(x => x.GetWorkingDaysAheadDate(It.IsAny<DateTime>(), It.IsAny<int>())).Returns(DateTime.Today.AddDays(10));

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
            var _applicationId = Guid.NewGuid();
            var _appealOnPolicyOrProcesses = false;
            var _appealOnEvidenceSubmitted = false;

            var oversightReview = new GetOversightReviewResponse { Status = OversightReviewStatus.Unsuccessful, ApplicationDeterminedDate = DateTime.Today };
            _apiClient.Setup(x => x.GetOversightReview(It.IsAny<Guid>())).ReturnsAsync(oversightReview);
            _bankHolidayService.Setup(x => x.GetWorkingDaysAheadDate(It.IsAny<DateTime>(), It.IsAny<int>())).Returns(DateTime.Today.AddDays(-10));

            var result = await _controller.GroundsOfAppeal(_applicationId, _appealOnPolicyOrProcesses, _appealOnEvidenceSubmitted);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("TaskList");
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
            var model = new ApplicationSummaryViewModel();
            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(submittedApp, "test@test.com")).Returns(model);


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
            var applicationDeterminedDate = DateTime.Today;
            var appealRequiredByDate = DateTime.Today.AddDays(10);
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Removed
            };

            var oversightReview = new GetOversightReviewResponse { Status = status,ApplicationDeterminedDate = applicationDeterminedDate};
            _applicationApiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(submittedApp);
            _apiClient.Setup(x => x.GetOversightReview(It.IsAny<Guid>())).ReturnsAsync(oversightReview);
            var model = new ApplicationSummaryViewModel();
            _outcomeService.Setup(x => x.BuildApplicationSummaryViewModel(submittedApp, "test@test.com")).Returns(model);
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


        [Test]
        public async Task Application_sectors_show_sector_details()
        {
            var applicationId = Guid.NewGuid();
            const string pageId = "pageId";
            const string sectorName = "name of sector";

            var viewModel = new OutcomeSectorDetailsViewModel
            {
                ApplicationId = applicationId,
                SectorDetails = new SectorDetails {SectorName = sectorName}
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

            _apiClient.Setup(x => x.DownloadClarificationfile(applicationId, sequenceNumber, sectionNumber, pageId, filename)).ReturnsAsync(response);
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
            _apiClient.Setup(x => x.DownloadClarificationfile(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), filename)).ReturnsAsync(response);
            var result = await _controller.DownloadClarificationFile(Guid.NewGuid(), 1, 2, "_pageId", filename) as NotFoundResult;
            Assert.IsNotNull(result);
        }
    }
}
