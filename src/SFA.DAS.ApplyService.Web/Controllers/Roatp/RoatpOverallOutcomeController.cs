﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.EmailService.Interfaces;
using SFA.DAS.ApplyService.Types;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

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

            // Force appeal status page to show if the user did not click on the 'application overview' link
            var overviewLinkClicked = HttpContext.Request.Headers.ContainsKey("Referer");
            if(!overviewLinkClicked && model.IsAppealSubmitted)
            {
                switch(model.AppealStatus)
                {
                    case AppealStatus.Submitted:
                        return RedirectToAction("AppealSubmitted", "RoatpAppeals", new { applicationId });
                    case AppealStatus.InProgress:
                        return RedirectToAction("AppealInProgress", "RoatpAppeals", new { applicationId });
                    case AppealStatus.Unsuccessful:
                        return RedirectToAction("AppealUnsuccessful", "RoatpAppeals", new { applicationId });
                    case AppealStatus.Successful:
                    case AppealStatus.SuccessfulAlreadyActive:
                    case AppealStatus.SuccessfulFitnessForFunding:
                        return RedirectToAction("AppealSuccessful", "RoatpAppeals", new { applicationId });
                    default:
                        break;
                }
            }

            switch (model.ApplicationStatus)
            {
                case ApplicationStatus.New:
                case ApplicationStatus.InProgress:
                    return RedirectToAction("TaskList", "RoatpApplication", new { applicationId });
                case ApplicationStatus.Successful:

                    if (model.ApplicationRouteId ==
                        Domain.Roatp.ApplicationRoute.SupportingProviderApplicationRoute.ToString())
                    {
                        if (model.OversightReviewStatus == OversightReviewStatus.SuccessfulFitnessForFunding || model.AppealStatus == AppealStatus.SuccessfulFitnessForFunding)
                            return View("~/Views/Roatp/ApplicationApprovedSupportingFitnessForFunding.cshtml",
                                model);
                        if (model.OversightReviewStatus == OversightReviewStatus.SuccessfulAlreadyActive || model.AppealStatus==AppealStatus.SuccessfulAlreadyActive)
                            return View("~/Views/Roatp/ApplicationApprovedSupportingAlreadyActive.cshtml",
                                model);
                        return View("~/Views/Roatp/ApplicationApprovedSupporting.cshtml", model);
                    }
                    else
                    {
                        if (model.OversightReviewStatus == OversightReviewStatus.SuccessfulAlreadyActive || model.AppealStatus==AppealStatus.SuccessfulAlreadyActive)
                            return View("~/Views/Roatp/ApplicationApprovedAlreadyActive.cshtml", model);
                        if (model.OversightReviewStatus == OversightReviewStatus.SuccessfulFitnessForFunding || model.AppealStatus==AppealStatus.SuccessfulFitnessForFunding)
                            return View("~/Views/Roatp/ApplicationApprovedFitnessForFunding.cshtml", model);
                        
                        return View("~/Views/Roatp/ApplicationApproved.cshtml", model);
                    }

                case ApplicationStatus.InProgressAppeal:
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
                case ApplicationStatus.AppealSuccessful:
                    if(model.GatewayReviewStatus == GatewayReviewStatus.Fail)
                    {
                        return RedirectToAction("AppealSuccessful", "RoatpAppeals", new { applicationId });
                    }
                    else
                    {
                        return View("~/Views/Roatp/ApplicationAppealSuccessful.cshtml", model);
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

        [HttpGet("application/{applicationId}/Sequence/{sequenceNumber}/Section/{sectionNumber}/Page/{pageId}/Download/{fileName}")]
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
