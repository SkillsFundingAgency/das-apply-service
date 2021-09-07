using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Types;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;

namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    [Authorize(Policy = "AccessApplication")]
    public class RoatpOverallOutcomeController : Controller
    {
        private readonly IOverallOutcomeService _overallOutcomeService;
        private readonly IOutcomeApiClient _outcomeApiClient;    

        public RoatpOverallOutcomeController(IOverallOutcomeService overallOutcomeService, IOutcomeApiClient outcomeApiClient)
        {
            _overallOutcomeService = overallOutcomeService;
            _outcomeApiClient = outcomeApiClient;
        }

        [HttpGet]
        [Route("application/{applicationId}/sector/{pageId}")]
        public async Task<IActionResult> GetSectorDetails(Guid applicationId, string pageId)
        {
            var model = await _overallOutcomeService.GetSectorDetailsViewModel(applicationId, pageId);
            return View("~/Views/Roatp/ApplicationUnsuccessfulSectorAnswers.cshtml", model);
        }

        [HttpGet]
        [Route("application/{applicationId}/status")]
        public async Task<IActionResult> ProcessApplicationStatus(Guid applicationId)
        {
            var model = await _overallOutcomeService.BuildApplicationSummaryViewModel(applicationId, User.GetEmail());

            switch (model.ApplicationStatus)
            {
                case ApplicationStatus.New:
                case ApplicationStatus.InProgress:
                    return RedirectToAction("TaskList", "RoatpApplication", new { applicationId });
                case ApplicationStatus.Successful:

                    if (model.ApplicationRouteId == Domain.Roatp.ApplicationRoute.SupportingProviderApplicationRoute.ToString())
                    {
                        switch (model.OversightReviewStatus)
                        {
                            case OversightReviewStatus.SuccessfulFitnessForFunding:
                                return View("~/Views/Roatp/ApplicationApprovedSupportingFitnessForFunding.cshtml",
                                    model);
                            case OversightReviewStatus.SuccessfulAlreadyActive:
                                return View("~/Views/Roatp/ApplicationApprovedSupportingAlreadyActive.cshtml",
                                    model);
                            default:
                                return View("~/Views/Roatp/ApplicationApprovedSupporting.cshtml", model);
                        }
                    }
                    else
                    {
                        switch (model.OversightReviewStatus)
                        {
                            case OversightReviewStatus.SuccessfulAlreadyActive:
                                return View("~/Views/Roatp/ApplicationApprovedAlreadyActive.cshtml", model);
                            case OversightReviewStatus.SuccessfulFitnessForFunding:
                                return View("~/Views/Roatp/ApplicationApprovedFitnessForFunding.cshtml", model);
                            default:
                                return View("~/Views/Roatp/ApplicationApproved.cshtml", model);
                        }
                    }

                case ApplicationStatus.Unsuccessful:
                    if (model.GatewayReviewStatus == GatewayReviewStatus.Fail)
                    {
                        return View("~/Views/Roatp/ApplicationUnsuccessful.cshtml", model);
                    }
                    else
                    {
                        var unsuccessfulModel =
                            await _overallOutcomeService.BuildApplicationSummaryViewModelWithGatewayAndModerationDetails(model.ApplicationId, User.GetEmail());

                        return View("~/Views/Roatp/ApplicationUnsuccessfulPostGateway.cshtml", unsuccessfulModel);
                    }

                case ApplicationStatus.FeedbackAdded:
                    return View("~/Views/Roatp/FeedbackAdded.cshtml", model);
                case ApplicationStatus.Withdrawn:
                    return View("~/Views/Roatp/ApplicationWithdrawn.cshtml", model);
                case ApplicationStatus.Removed:
                    if (model.OversightReviewStatus == OversightReviewStatus.Removed)
                        return View("~/Views/Roatp/ApplicationWithdrawnESFA.cshtml", model);
                    return View("~/Views/Roatp/ApplicationSubmitted.cshtml", model);
                case ApplicationStatus.GatewayAssessed:
                    if (model.GatewayReviewStatus == GatewayReviewStatus.Rejected)
                        return View("~/Views/Roatp/ApplicationRejected.cshtml", model);
                    return View("~/Views/Roatp/ApplicationSubmitted.cshtml", model);
                case ApplicationStatus.Rejected:
                    return View("~/Views/Roatp/ApplicationRejected.cshtml", model);
                case ApplicationStatus.Submitted:
                case ApplicationStatus.Resubmitted:
                    return View("~/Views/Roatp/ApplicationSubmitted.cshtml", model);
                case ApplicationStatus.InProgressOutcome:
                    return View("~/Views/Roatp/ApplicationInProgress.cshtml", model);
                default:
                    return RedirectToAction("TaskList", "RoatpApplication", new { applicationId });
            }
        }

        [HttpGet("ClarificationDownload/{applicationId}/Sequence/{sequenceNumber}/Section/{sectionNumber}/Page/{pageId}/Download/{fileName}")]
        [Authorize(Policy = "AccessApplication")]
        public async Task<IActionResult> DownloadClarificationFile(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId, string fileName)
        {
            var response = await _outcomeApiClient.DownloadClarificationfile(applicationId, sequenceNumber, sectionNumber, pageId, fileName);

            if (!response.IsSuccessStatusCode) return NotFound();
            var fileStream = await response.Content.ReadAsStreamAsync();
            return File(fileStream, response.Content.Headers.ContentType.MediaType, fileName);
        }
    }
}
