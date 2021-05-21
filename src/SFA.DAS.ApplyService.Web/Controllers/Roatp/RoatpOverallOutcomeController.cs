using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Apply;
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
        private readonly IApplicationApiClient _apiClient;
        private readonly IQnaApiClient _qnaApiClient;
        private readonly IOverallOutcomeAugmentationService _augmentationService;
        private readonly ILogger<RoatpOverallOutcomeController> _logger;

        public RoatpOverallOutcomeController(IApplicationApiClient apiClient, IQnaApiClient qnaApiClient,
              IOverallOutcomeAugmentationService augmentationService, ILogger<RoatpOverallOutcomeController> logger)
        {
            _apiClient = apiClient;
            _qnaApiClient = qnaApiClient;
            _augmentationService = augmentationService;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> ProcessApplicationStatus(Guid applicationId)
        {
            var application = await _apiClient.GetApplication(applicationId);
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
                        return RedirectToAction("ApplicationApprovedAlreadyActive", new {applicationId});

                    return RedirectToAction("ApplicationApproved", new {applicationId});
                }
                case ApplicationStatus.Rejected: //this logic will need to change with the coming status update story
                    if (application.GatewayReviewStatus == GatewayReviewStatus.Fail)
                        return RedirectToAction("ApplicationUnsuccessful", new {applicationId});
                    return RedirectToAction("ApplicationCheckedAgainstModeration",  new {applicationId});
                case ApplicationStatus.FeedbackAdded:
                    return RedirectToAction("FeedbackAdded", new {applicationId});
                case ApplicationStatus.Withdrawn:
                    return RedirectToAction("ApplicationWithdrawn", new {applicationId});
                case ApplicationStatus.Removed:
                    return RedirectToAction("ApplicationRemoved",  new {applicationId});
                case ApplicationStatus.GatewayAssessed:
                    if (application.GatewayReviewStatus == GatewayReviewStatus.Reject)
                        return RedirectToAction("ApplicationRejected", 
                            new {applicationId});
                    return RedirectToAction("ApplicationSubmitted",  new {applicationId});
                case ApplicationStatus.Submitted:
                case ApplicationStatus.Resubmitted:
                    return RedirectToAction("ApplicationSubmitted",  new {applicationId});
                default:
                    return RedirectToAction("TaskList", "RoatpApplication", new {applicationId});
            }
        }

        [HttpGet]
        [Route("ApplicationStatus")]
        public async Task<IActionResult> ApplicationCheckedAgainstModeration(Guid applicationId)
        {
            var application = await _apiClient.GetApplication(applicationId);
            var applicationData = application.ApplyData.ApplyDetails;

            var oversightReview = await _apiClient.GetOversightReview(applicationId);
            // special page for pmo fail and/or moderation fail

            var applicationUnsuccessful = false;
            var applicationUnsuccessfulModerationFail = false;
            if (application?.GatewayReviewStatus == GatewayAnswerStatus.Pass)
            {
                if (application?.ModerationStatus != null
                    && application?.ModerationStatus == ModerationStatus.Fail
                    && oversightReview.ModerationApproved.HasValue
                    && oversightReview.ModerationApproved == true)
                {
                    applicationUnsuccessful = true;
                    applicationUnsuccessfulModerationFail = true;
                }

                if (application?.FinancialReviewStatus != null
                    && application?.FinancialReviewStatus == FinancialReviewStatus.Fail)
                    applicationUnsuccessful = true;
            }

            if (applicationUnsuccessful)
            {
                var model = new ApplicationSummaryWithModeratorDetails
                {
                    ApplicationId = application.ApplicationId,
                    UKPRN = applicationData.UKPRN,
                    OrganisationName = applicationData.OrganisationName,
                    TradingName = applicationData.TradingName,
                    ApplicationRouteId = applicationData.ProviderRoute.ToString(),
                    ApplicationReference = applicationData.ReferenceNumber,
                    SubmittedDate = applicationData?.ApplicationSubmittedOn,
                    ExternalComments = application?.ApplyData?.GatewayReviewDetails?.ExternalComments,
                    EmailAddress = User.GetEmail(),
                    FinancialReviewStatus = application?.FinancialReviewStatus,
                    FinancialGrade = application?.FinancialGrade?.SelectedGrade,
                    FinancialExternalComments = application?.FinancialGrade?.ExternalComments,
                    GatewayReviewStatus = application?.GatewayReviewStatus,
                    ModerationStatus = application?.ModerationStatus
                };

                if (applicationUnsuccessfulModerationFail)
                {
                    await _augmentationService.AugmentModelWithModerationFailDetails(model,
                        User.GetUserId().ToString());
                }
                return View("~/Views/Roatp/ApplicationUnsuccessfulPostGateway.cshtml", model);
            }

            return RedirectToAction("ApplicationUnsuccessful", new { applicationId });
        }

        // private async Task AugmentModelWithModerationFailDetails(ApplicationSummaryWithModeratorDetails model, string userId)
        // {
        //     // add much more details to the model if moderation is a fail
        //     // build the model
        //
        //     // A check clarifications for failed details
        //     var sequences = await _apiClient.GetClarificationSequences(model.ApplicationId);
        //
        //     var passFailDetails = await _apiClient.GetAllClarificationPageReviewOutcomes(model.ApplicationId, userId);
        //     var failedDetails = passFailDetails.Where(x => x.ModeratorReviewStatus == ModerationStatus.Fail).ToList();
        //
        //     // add the failed question pages to the sections under each sequence where fails have been found
        //     if (failedDetails.Any())
        //     {
        //         AddPagesToSequencesFromFailedDetails(sequences, failedDetails);
        //
        //         var sequencesWithModerationFails = new List<AssessorSequence>();
        //         BuildSequencesWithModerationFails(sequences, sequencesWithModerationFails);
        //
        //         var allSections = await _qnaApiClient.GetSections(model.ApplicationId);
        //
        //         RemoveInactiveOrEmptyPagesFromSequences(sequencesWithModerationFails, allSections);
        //         AddPageTitlesToSequences(sequencesWithModerationFails);
        //         AddAnswersToSequences(sequencesWithModerationFails, allSections);
        //         AddQuestionsToSequences(sequencesWithModerationFails, allSections);
        //         AddSequenceTitlesToSequences(sequencesWithModerationFails);
        //
        //         model.Sequences = sequencesWithModerationFails;
        //         model.PagesWithGuidance =
        //             GatherGuidancePagesForSequenceQuestions(sequencesWithModerationFails, allSections);
        //     }
        // }

        
        
        [HttpGet]
        [Authorize(Policy = "AccessApplication")]
        public async Task<IActionResult> ApplicationUnsuccessful(Guid applicationId)
        {
            var application = await _apiClient.GetApplication(applicationId);
            var applicationData = application.ApplyData.ApplyDetails;

            var model = new ApplicationSummaryViewModel
            {
                ApplicationId = application.ApplicationId,
                UKPRN = applicationData.UKPRN,
                OrganisationName = applicationData.OrganisationName,
                TradingName = applicationData.TradingName,
                ApplicationRouteId = applicationData.ProviderRoute.ToString(),
                ApplicationReference = applicationData.ReferenceNumber,
                EmailAddress = User.GetEmail(),
                SubmittedDate = applicationData.ApplicationSubmittedOn,
                ExternalComments = application.ApplyData.GatewayReviewDetails.ExternalComments
            };

            return View("~/Views/Roatp/ApplicationUnsuccessful.cshtml", model);
        }

        [HttpGet]
        [Authorize(Policy = "AccessApplication")]
        public async Task<IActionResult> ApplicationSubmitted(Guid applicationId)
        {
            var model = await BuildApplicationSummaryViewModel(applicationId);
            return View("~/Views/Roatp/ApplicationSubmitted.cshtml", model);
        }

        [HttpGet]
        [Authorize(Policy = "AccessApplication")]
        public async Task<IActionResult> ApplicationWithdrawn(Guid applicationId)
        {
            var model = await BuildApplicationSummaryViewModel(applicationId);

            return View("~/Views/Roatp/ApplicationWithdrawn.cshtml", model);
        }


        [HttpGet]
        [Authorize(Policy = "AccessApplication")]
        public async Task<IActionResult> ApplicationRemoved(Guid applicationId)
        {
            var model = await BuildApplicationSummaryViewModel(applicationId);
            return View("~/Views/Roatp/ApplicationWithdrawnESFA.cshtml", model);
        }

        [HttpGet]
        [Authorize(Policy = "AccessApplication")]
        public async Task<IActionResult> ApplicationRejected(Guid applicationId)
        {
            var model = await BuildApplicationSummaryViewModel(applicationId);
            return View("~/Views/Roatp/ApplicationRejected.cshtml", model);
        }




        [HttpGet]
        [Authorize(Policy = "AccessApplication")]
        public async Task<IActionResult> ApplicationApproved(Guid applicationId)
        {
            var model = await BuildApplicationSummaryViewModel(applicationId);
            return View("~/Views/Roatp/ApplicationApproved.cshtml", model);
        }

        [HttpGet]
        [Authorize(Policy = "AccessApplication")]
        public async Task<IActionResult> FeedbackAdded(Guid applicationId)
        {
            var model = await BuildApplicationSummaryViewModel(applicationId);
            return View("~/Views/Roatp/FeedbackAdded.cshtml", model);
        }

        [HttpGet]
        [Authorize(Policy = "AccessApplication")]
        public async Task<IActionResult> ApplicationApprovedAlreadyActive(Guid applicationId)
        {
            var model = await BuildApplicationSummaryViewModel(applicationId);
            return View("~/Views/Roatp/ApplicationApprovedAlreadyActive.cshtml", model);
        }

        public async Task<IActionResult> DownloadFile(Guid applicationId, Guid sectionId,
            string pageId, string questionId, string filename)
        {
            var response = await _qnaApiClient.DownloadFile(applicationId, sectionId, pageId, questionId,
                filename);

            if (response.IsSuccessStatusCode)
            {
                var fileStream = await response.Content.ReadAsStreamAsync();

                return File(fileStream, response.Content.Headers.ContentType.MediaType, filename);
            }

            return NotFound();
        }


        private async Task<ApplicationSummaryViewModel> BuildApplicationSummaryViewModel(Guid applicationId)
        {
            var application = await _apiClient.GetApplication(applicationId);
            var applicationData = application.ApplyData.ApplyDetails;

            var model = new ApplicationSummaryViewModel
            {
                ApplicationId = application.ApplicationId,
                UKPRN = applicationData.UKPRN,
                OrganisationName = applicationData.OrganisationName,
                TradingName = applicationData.TradingName,
                ApplicationRouteId = applicationData.ProviderRoute.ToString(),
                ApplicationReference = applicationData.ReferenceNumber,
                SubmittedDate = applicationData?.ApplicationSubmittedOn,
                ExternalComments = application?.ApplyData?.GatewayReviewDetails?.ExternalComments,
                EmailAddress = User.GetEmail(),
                FinancialReviewStatus = application?.FinancialReviewStatus,
                FinancialGrade = application?.FinancialGrade?.SelectedGrade,
                FinancialExternalComments = application?.FinancialGrade?.ExternalComments,
                GatewayReviewStatus = application?.GatewayReviewStatus,
                ModerationStatus = application?.ModerationStatus
            };
            return model;
        }
    }
}
