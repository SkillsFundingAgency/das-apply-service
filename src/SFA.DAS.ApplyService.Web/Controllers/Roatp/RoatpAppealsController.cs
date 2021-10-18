using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp.Appeals;
using SFA.DAS.ApplyService.Types;
using SFA.DAS.ApplyService.Web.Infrastructure.FeatureToggles;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.EmailService.Interfaces;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    [Authorize(Policy = "AccessAppeal")]
    [FeatureToggle(nameof(FeatureToggles.EnableAppeals), "RoatpApplication", "Applications")]
    public class RoatpAppealsController : Controller
    {
        private readonly IOutcomeApiClient _outcomeApiClient;
        private readonly IAppealsApiClient _appealsApiClient;
        private readonly IApplicationApiClient _applicationApiClient;
        private readonly ILogger<RoatpAppealsController> _logger;
        private readonly IRequestInvitationToReapplyEmailService _emailService;


        public RoatpAppealsController(IOutcomeApiClient apiClient, IAppealsApiClient appealsApiClient, IApplicationApiClient applicationApiClient, ILogger<RoatpAppealsController> logger, IRequestInvitationToReapplyEmailService emailService)
        {
            _outcomeApiClient = apiClient;
            _appealsApiClient = appealsApiClient;
            _applicationApiClient = applicationApiClient;
            _logger = logger;
            _emailService = emailService;
        }

        [HttpGet("application/{applicationId}/appeal")]
        [ModelStatePersist(ModelStatePersist.RestoreEntry)]
        public async Task<IActionResult> MakeAppeal(Guid applicationId)
        {
            if (!await CanMakeAppeal(applicationId))
            {
                return RedirectToAction("ProcessApplicationStatus", "RoatpOverallOutcome", new { applicationId });
            }

            var model = new MakeAppealViewModel
            {
                ApplicationId = applicationId
            };

            return View("~/Views/Appeals/MakeAppeal.cshtml", model);
        }

        [HttpPost]
        [Route("application/{applicationId}/request-new-invitation")]
        public async Task<IActionResult> RequestNewInvitation(Guid applicationId)
        {

            var success = await _outcomeApiClient.ReapplicationRequested(applicationId, User.GetUserId().ToString());

            if (!success)
            {
                _logger.LogError($"Unable to request reapplication: {applicationId}");
                return RedirectToAction("ProcessApplicationStatus", "RoatpOverallOutcome", new { applicationId });
            }

            var application = await _applicationApiClient.GetApplication(applicationId);

            var emailRequest = new RequestInvitationToReapply
            {
                EmailAddress = User.GetEmail(),
                UKPRN = application?.ApplyData?.ApplyDetails?.UKPRN,
                OrganisationName = application?.ApplyData?.ApplyDetails?.OrganisationName
            };

            await _emailService.SendRequestToReapplyEmail(emailRequest);
            return RedirectToAction("RequestNewInvitationRefresh", "RoatpAppeals", new {applicationId });

        }

        [HttpGet]
        [Route("application/{applicationId}/request-new-invitation")]
        public async Task<IActionResult> RequestNewInvitationRefresh(Guid applicationId)
        {
            return View("~/Views/Roatp/RequestNewInvitation.cshtml", new ApplicationSummaryViewModel { ApplicationId = applicationId, });
        }

        [HttpPost("application/{applicationId}/appeal")]
        [ModelStatePersist(ModelStatePersist.Store)]
        public async Task<IActionResult> MakeAppeal(MakeAppealViewModel model)
        {
            if (!await CanMakeAppeal(model.ApplicationId))
            {
                return RedirectToAction("ProcessApplicationStatus", "RoatpOverallOutcome", new { model.ApplicationId });
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("MakeAppeal", new { model.ApplicationId });
            }

            return RedirectToAction("GroundsOfAppeal", new { model.ApplicationId, model.AppealOnPolicyOrProcesses, model.AppealOnEvidenceSubmitted });
        }

        [HttpGet("application/{applicationId}/grounds-of-appeal")]
        [ModelStatePersist(ModelStatePersist.RestoreEntry)]
        public async Task<IActionResult> GroundsOfAppeal(Guid applicationId, bool appealOnPolicyOrProcesses, bool appealOnEvidenceSubmitted)
        {
            if (!await CanMakeAppeal(applicationId))
            {
                return RedirectToAction("ProcessApplicationStatus", "RoatpOverallOutcome", new { applicationId });
            }

            var appealFileList = await _appealsApiClient.GetAppealFileList(applicationId);

            var model = new GroundsOfAppealViewModel
            {
                ApplicationId = applicationId,
                AppealOnPolicyOrProcesses = appealOnPolicyOrProcesses,
                AppealOnEvidenceSubmitted = appealOnEvidenceSubmitted,
                AppealFiles = appealFileList?.AppealFiles
            };

            RestoreUserInputFromTempData(model);

            return View("~/Views/Appeals/GroundsOfAppeal.cshtml", model);
        }

        [HttpPost("application/{applicationId}/grounds-of-appeal")]
        [ModelStatePersist(ModelStatePersist.Store)]
        [Authorize(Policy = "AccessAppealNotYetSubmitted")]
        public async Task<IActionResult> GroundsOfAppeal(GroundsOfAppealViewModel model)
        {
            if (!await CanMakeAppeal(model.ApplicationId))
            {
                return RedirectToAction("ProcessApplicationStatus", "RoatpOverallOutcome", new { model.ApplicationId });
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("GroundsOfAppeal", new { model.ApplicationId, model.AppealOnPolicyOrProcesses, model.AppealOnEvidenceSubmitted });
            }

            var signInId = User.GetSignInId().ToString();
            var userName = User.Identity.Name;

            switch (model.RequestedFormAction)
            {
                case GroundsOfAppealViewModel.DELETE_APPEALFILE_FORMACTION:
                    await _appealsApiClient.DeleteFile(model.ApplicationId, model.RequestedFileToDelete, signInId, userName);
                    StoreUserInputInTempData(model);
                    return RedirectToAction("GroundsOfAppeal", new { model.ApplicationId, model.AppealOnPolicyOrProcesses, model.AppealOnEvidenceSubmitted });

                case GroundsOfAppealViewModel.UPLOAD_APPEALFILE_FORMACTION:
                    await _appealsApiClient.UploadFile(model.ApplicationId, model.AppealFileToUpload, signInId, userName);
                    StoreUserInputInTempData(model);
                    return RedirectToAction("GroundsOfAppeal", new { model.ApplicationId, model.AppealOnPolicyOrProcesses, model.AppealOnEvidenceSubmitted });

                case GroundsOfAppealViewModel.SUBMIT_APPEAL_FORMACTION:
                    if (model.AppealFileToUpload != null)
                    {
                        await _appealsApiClient.UploadFile(model.ApplicationId, model.AppealFileToUpload, signInId, userName);
                    }
                    await _appealsApiClient.MakeAppeal(model.ApplicationId, model.HowFailedOnPolicyOrProcesses, model.HowFailedOnEvidenceSubmitted, signInId, userName);
                    return RedirectToAction("AppealSubmitted", new { model.ApplicationId });

                default:
                    return RedirectToAction("ProcessApplicationStatus", "RoatpOverallOutcome", new { model.ApplicationId });
            }
        }

        [HttpGet("application/{applicationId}/appeal-submitted")]
        public async Task<IActionResult> AppealSubmitted(Guid applicationId)
        {
            var appeal = await _appealsApiClient.GetAppeal(applicationId);

            if (appeal?.AppealSubmittedDate is null)
            {
                return RedirectToAction("ProcessApplicationStatus", "RoatpOverallOutcome", new { applicationId });
            }

            var model = new AppealSubmittedViewModel
            {
                ApplicationId = appeal.ApplicationId,
                AppealSubmittedDate = appeal.AppealSubmittedDate.Value,
                HowFailedOnEvidenceSubmitted = appeal.HowFailedOnEvidenceSubmitted,
                HowFailedOnPolicyOrProcesses = appeal.HowFailedOnPolicyOrProcesses,
                AppealFiles = appeal.AppealFiles
            };

            return View("~/Views/Appeals/AppealSubmitted.cshtml", model);
        }

        [HttpGet("application/{applicationId}/cancel-appeal")]
        [Authorize(Policy = "AccessAppealNotYetSubmitted")]
        public async Task<IActionResult> CancelAppeal(Guid applicationId)
        {
            var signInId = User.GetSignInId().ToString();
            var userName = User.Identity.Name;

            await _appealsApiClient.CancelAppeal(applicationId, signInId, userName);

            return RedirectToAction("ProcessApplicationStatus", "RoatpOverallOutcome", new { applicationId });
        }

        [HttpGet("application/{applicationId}/appeal/in-progress")]
        public async Task<IActionResult> AppealInProgress(Guid applicationId)
        {
            var appeal = await _appealsApiClient.GetAppeal(applicationId);
            if (appeal?.Status != AppealStatus.InProgress)
            {
                return RedirectToAction("ProcessApplicationStatus", "RoatpOverallOutcome", new { applicationId });
            }

            var model = new AppealInProgressViewModel
            {
                ApplicationId = applicationId,
                AppealSubmittedDate = appeal.AppealSubmittedDate.Value,
                ExternalComments = appeal.InProgressExternalComments
            };

            return View("~/Views/Appeals/AppealInProgress.cshtml", model);
        }

        [HttpGet("application/{applicationId}/appeal/unsuccessful")]
        public async Task<IActionResult> AppealUnsuccessful(Guid applicationId)
        {
            var appeal = await _appealsApiClient.GetAppeal(applicationId);
            if (appeal?.Status != AppealStatus.Unsuccessful)
            {
                return RedirectToAction("ProcessApplicationStatus", "RoatpOverallOutcome", new { applicationId });
            }

            var model = new AppealUnsuccessfulViewModel
            {
                ApplicationId = applicationId,
                AppealSubmittedDate = appeal.AppealSubmittedDate.Value,
                AppealDeterminedDate = appeal.AppealDeterminedDate.Value,
                AppealedOnEvidenceSubmitted = !string.IsNullOrEmpty(appeal.HowFailedOnEvidenceSubmitted),
                AppealedOnPolicyOrProcesses = !string.IsNullOrEmpty(appeal.HowFailedOnPolicyOrProcesses),
                ExternalComments = appeal.ExternalComments
            };

            return View("~/Views/Appeals/AppealUnsuccessful.cshtml", model);
        }

        [HttpGet("application/{applicationId}/appeal/successful")]
        public async Task<IActionResult> AppealSuccessful(Guid applicationId)
        {
            var appeal = await _appealsApiClient.GetAppeal(applicationId);
            if (appeal is null)
                return RedirectToAction("ProcessApplicationStatus", "RoatpOverallOutcome", new { applicationId });

            if (appeal.Status != AppealStatus.Successful && appeal.Status != AppealStatus.SuccessfulFitnessForFunding && appeal.Status != AppealStatus.SuccessfulAlreadyActive)
            {
                return RedirectToAction("ProcessApplicationStatus", "RoatpOverallOutcome", new { applicationId });
            }

            var application = await _applicationApiClient.GetApplication(applicationId);


            var model = new AppealSuccessfulViewModel
            {
                ApplicationId = applicationId,
                AppealSubmittedDate = appeal.AppealSubmittedDate ?? null,
                AppealDeterminedDate = appeal.AppealDeterminedDate ?? null,
                AppealedOnEvidenceSubmitted = !string.IsNullOrEmpty(appeal.HowFailedOnEvidenceSubmitted),
                AppealedOnPolicyOrProcesses = !string.IsNullOrEmpty(appeal.HowFailedOnPolicyOrProcesses),
                ExternalComments = appeal.ExternalComments,
                SubcontractingLimit = application?.ApplyData?.GatewayReviewDetails?.SubcontractingLimit
            };

            var isGatewayFail = application?.GatewayReviewStatus == GatewayReviewStatus.Fail;
            if (isGatewayFail && application?.ApplicationStatus == ApplicationStatus.AppealSuccessful)
            {
                switch (appeal.Status)
                {
                    case AppealStatus.Successful:
                    case AppealStatus.SuccessfulAlreadyActive:
                    case AppealStatus.SuccessfulFitnessForFunding:
                        return View("~/Views/Appeals/AppealSuccessfulGatewayFail.cshtml", model);
                }

            }

            var isSupporting = application?.ApplyData?.ApplyDetails?.ProviderRoute.ToString() ==
                                 Domain.Roatp.ApplicationRoute.SupportingProviderApplicationRoute.ToString();

            switch (appeal.Status)
            {
                case AppealStatus.Successful:
                    return View(isSupporting ? "~/Views/Appeals/AppealSuccessfulSupporting.cshtml" : "~/Views/Appeals/AppealSuccessful.cshtml", model);
                case AppealStatus.SuccessfulAlreadyActive:
                    return View(isSupporting ? "~/Views/Appeals/AppealSuccessfulSupportingAlreadyActive.cshtml" : "~/Views/Appeals/AppealSuccessfulAlreadyActive.cshtml", model);
                case AppealStatus.SuccessfulFitnessForFunding:
                    return View(isSupporting ? "~/Views/Appeals/AppealSuccessfulSupportingFitnessForFunding.cshtml" : "~/Views/Appeals/AppealSuccessfulFitnessForFunding.cshtml", model);
            }

            return View("~/Views/Roatp/AppealSuccessful.cshtml", model);
        }

        [HttpGet("application/{applicationId}/appeal/file/{fileName}")]
        [Authorize(Policy = "AccessAppeal")]
        public async Task<IActionResult> DownloadAppealFile(Guid applicationId, string fileName)
        {
            var response = await _appealsApiClient.DownloadFile(applicationId, fileName);

            if (response.IsSuccessStatusCode)
            {
                var fileStream = await response.Content.ReadAsStreamAsync();

                return File(fileStream, response.Content.Headers.ContentType.MediaType, response.Content.Headers.ContentDisposition.FileNameStar);
            }

            return NotFound();
        }

        private void StoreUserInputInTempData(GroundsOfAppealViewModel model)
        {
            TempData["HowFailedOnEvidenceSubmitted"] = model.HowFailedOnEvidenceSubmitted;
            TempData["HowFailedOnPolicyOrProcesses"] = model.HowFailedOnPolicyOrProcesses;
        }

        private void RestoreUserInputFromTempData(GroundsOfAppealViewModel model)
        {
            if (TempData["HowFailedOnEvidenceSubmitted"] != null)
            {
                model.HowFailedOnEvidenceSubmitted = TempData["HowFailedOnEvidenceSubmitted"] as string;
            }

            if (TempData["HowFailedOnPolicyOrProcesses"] != null)
            {
                model.HowFailedOnPolicyOrProcesses = TempData["HowFailedOnPolicyOrProcesses"] as string;
            }
        }

        private async Task<bool> CanMakeAppeal(Guid applicationId)
        {
            return await AppealNotYetSubmitted(applicationId) && await WithinAppealWindow(applicationId);
        }

        private async Task<bool> AppealNotYetSubmitted(Guid applicationId)
        {
            var appeal = await _appealsApiClient.GetAppeal(applicationId);

            return appeal is null || string.IsNullOrEmpty(appeal.Status) || appeal.AppealSubmittedDate is null;
        }

        private async Task<bool> WithinAppealWindow(Guid applicationId)
        {
            // NOTE: This is an effective workaround until we have AppealRequiredByDate in the OversightReview
            var oversight = await _outcomeApiClient.GetOversightReview(applicationId);

            const int numberOfWorkingDays = 10;
            var appealRequiredByDate = await _outcomeApiClient.GetWorkingDaysAheadDate(oversight?.ApplicationDeterminedDate, numberOfWorkingDays);

            return appealRequiredByDate.HasValue && appealRequiredByDate >= DateTime.Today;
        }
    }
}
