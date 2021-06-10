using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NLog.Common;
using SFA.DAS.AdminService.Common.Extensions;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Types;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    [Authorize]
    public class RoatpOverallOutcomeController : Controller
    {
        private readonly IOutcomeApiClient _apiClient;
        private readonly IApplicationApiClient _applicationApiClient;
        private readonly IQnaApiClient _qnaApiClient;
        private readonly IOverallOutcomeService _overallOutcomeService;
        private readonly ILogger<RoatpOverallOutcomeController> _logger;

        public RoatpOverallOutcomeController(IOutcomeApiClient apiClient, IQnaApiClient qnaApiClient,
            IOverallOutcomeService overallOutcomeService, IApplicationApiClient applicationApiClient,
            ILogger<RoatpOverallOutcomeController> logger)
        {
            _apiClient = apiClient;
            _qnaApiClient = qnaApiClient;
            _overallOutcomeService = overallOutcomeService;
            _logger = logger;
            _applicationApiClient = applicationApiClient;
        }



        [HttpGet] 
        // [Authorize(Policy = "AccessApplication")]
        [Route("application/{applicationId}/sector/{pageId}")]
        public async Task<IActionResult> GetSectorDetails(Guid applicationId, string pageId)
        {
            //var model = new OutcomeSectorDetailsViewModel();

            var model = await _overallOutcomeService.GetSectorDetailsViewModel(applicationId, pageId);

            return View("~/Views/Roatp/ApplicationUnsuccessfulSectorAnswers.cshtml", model);
        }

        [HttpGet]
        [Authorize(Policy = "AccessApplication")]
        public async Task<IActionResult> ProcessApplicationStatus(Guid applicationId)
        {
            var application = await _applicationApiClient.GetApplication(applicationId);
            var model = _overallOutcomeService.BuildApplicationSummaryViewModel(application, User.GetEmail());

            switch (application.ApplicationStatus)
            {
                case ApplicationStatus.New:
                case ApplicationStatus.InProgress:
                    return RedirectToAction("TaskList", "RoatpApplication", new {applicationId});
                case ApplicationStatus.Successful:
                {
                    var oversightReview = await _apiClient.GetOversightReview(applicationId);
                    if (oversightReview?.Status == OversightReviewStatus.SuccessfulAlreadyActive)
                        return View("~/Views/Roatp/ApplicationApprovedAlreadyActive.cshtml", model);

                    return View("~/Views/Roatp/ApplicationApproved.cshtml", model);
                }
                case ApplicationStatus.Unsuccessful: 
                    if (application.GatewayReviewStatus == GatewayReviewStatus.Fail)
                        return View("~/Views/Roatp/ApplicationUnsuccessful.cshtml", model);

                    var unsuccessfulModel =
                        await _overallOutcomeService.BuildApplicationSummaryViewModelWithModerationDetails(application,
                            User.GetEmail());
                    return View("~/Views/Roatp/ApplicationUnsuccessfulPostGateway.cshtml", unsuccessfulModel);

                case ApplicationStatus.FeedbackAdded:
                    return View("~/Views/Roatp/FeedbackAdded.cshtml", model);
                case ApplicationStatus.Withdrawn:
                    return View("~/Views/Roatp/ApplicationWithdrawn.cshtml", model);
                case ApplicationStatus.Removed:
                    return View("~/Views/Roatp/ApplicationWithdrawnESFA.cshtml", model);
                case ApplicationStatus.GatewayAssessed:
                    if (application.GatewayReviewStatus == GatewayReviewStatus.Rejected)
                        return View("~/Views/Roatp/ApplicationRejected.cshtml", model);
                    return View("~/Views/Roatp/ApplicationSubmitted.cshtml", model);
                case ApplicationStatus.Rejected:
                    return View("~/Views/Roatp/ApplicationRejected.cshtml", model);   
                case ApplicationStatus.Submitted:
                case ApplicationStatus.Resubmitted:
                    return View("~/Views/Roatp/ApplicationSubmitted.cshtml", model);
                default:
                    return RedirectToAction("TaskList", "RoatpApplication", new {applicationId});
            }
        }
    }
}
