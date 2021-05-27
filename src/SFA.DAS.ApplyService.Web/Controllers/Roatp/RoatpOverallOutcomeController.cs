using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Types;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;

namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    [Authorize]
    public class RoatpOverallOutcomeController : Controller
    {
        private readonly IOutcomeApiClient _apiClient;
        private readonly IApplicationApiClient _applicationApiClient;
        private readonly IQnaApiClient _qnaApiClient;
        private readonly IOverallOutcomeService _service;
        private readonly ILogger<RoatpOverallOutcomeController> _logger;

        public RoatpOverallOutcomeController(IOutcomeApiClient apiClient, IQnaApiClient qnaApiClient,
            IOverallOutcomeService service, IApplicationApiClient applicationApiClient,
            ILogger<RoatpOverallOutcomeController> logger)
        {
            _apiClient = apiClient;
            _qnaApiClient = qnaApiClient;
            _service = service;
            _logger = logger;
            _applicationApiClient = applicationApiClient;
        }
        
        [HttpGet]
        [Authorize(Policy = "AccessApplication")]
        public async Task<IActionResult> ProcessApplicationStatus(Guid applicationId)
        {
            var application = await _applicationApiClient.GetApplication(applicationId);
            var model = _service.BuildApplicationSummaryViewModel(application, User.GetEmail());
            var applicationStatus = application.ApplicationStatus;

            switch (applicationStatus)
            {
                case ApplicationStatus.New:
                case ApplicationStatus.InProgress:
                    return RedirectToAction("TaskList", "RoatpApplication", new {applicationId});
                case ApplicationStatus.Approved:
                {
                    var oversightReview = await _apiClient.GetOversightReview(applicationId);
                    if (oversightReview?.Status == OversightReviewStatus.SuccessfulAlreadyActive)
                        return View("~/Views/Roatp/ApplicationApprovedAlreadyActive.cshtml", model);

                    return View("~/Views/Roatp/ApplicationApproved.cshtml", model);
                }
                case ApplicationStatus.Rejected: //this logic will need to change with the coming status update story
                    if (application.GatewayReviewStatus == GatewayReviewStatus.Fail)
                    {
                        return View("~/Views/Roatp/ApplicationUnsuccessful.cshtml", model);
                    }

                    var unsuccessfulModel = await _service.ApplicationUnsuccessful(application, User.GetEmail());
                    return View("~/Views/Roatp/ApplicationUnsuccessfulPostGateway.cshtml", unsuccessfulModel);

                case ApplicationStatus.FeedbackAdded:
                    return View("~/Views/Roatp/FeedbackAdded.cshtml", model);
                case ApplicationStatus.Withdrawn:
                    return View("~/Views/Roatp/ApplicationWithdrawn.cshtml", model);
                case ApplicationStatus.Removed:
                    return View("~/Views/Roatp/ApplicationWithdrawnESFA.cshtml", model);
                case ApplicationStatus.GatewayAssessed:
                    if (application.GatewayReviewStatus == GatewayReviewStatus.Reject)
                        return View("~/Views/Roatp/ApplicationRejected.cshtml", model);
                    return View("~/Views/Roatp/ApplicationSubmitted.cshtml", model);
                case ApplicationStatus.Submitted:
                case ApplicationStatus.Resubmitted:
                    return View("~/Views/Roatp/ApplicationSubmitted.cshtml", model);
                default:
                    return RedirectToAction("TaskList", "RoatpApplication", new {applicationId});
            }
        }

        public async Task<IActionResult> DownloadFile(Guid applicationId, int sequenceNo, int sectionNo,
            string pageId, string questionId, string filename)
        {

            var section = await _qnaApiClient.GetSectionBySectionNo(applicationId, sequenceNo, sectionNo);
            var sectionId = section.Id;
            var response = await _qnaApiClient.DownloadFile(applicationId, sectionId, pageId, questionId,
                filename);

            if (response.IsSuccessStatusCode)
            {
                var fileStream = await response.Content.ReadAsStreamAsync();

                return File(fileStream, response.Content.Headers.ContentType.MediaType, filename);
            }

            return NotFound();
        }

    }

}
