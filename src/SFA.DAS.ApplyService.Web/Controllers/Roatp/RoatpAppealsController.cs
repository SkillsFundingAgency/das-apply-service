using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp.Appeals;

namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    [Authorize(Policy = "AccessAppeal")]
    public class RoatpAppealsController : Controller
    {
        private readonly IOutcomeApiClient _outcomeApiClient;
        private readonly IAppealsApiClient _appealsApiClient;

        public RoatpAppealsController(IOutcomeApiClient apiClient, IAppealsApiClient appealsApiClient)
        {
            _outcomeApiClient = apiClient;
            _appealsApiClient = appealsApiClient;
        }

        [HttpGet("application/{applicationId}/appeal")]
        [ModelStatePersist(ModelStatePersist.RestoreEntry)]
        public async Task<IActionResult> MakeAppeal(Guid applicationId)
        {
            if (!await CanMakeAppeal(applicationId))
            {
                return RedirectToAction("TaskList", "RoatpApplication", new { applicationId });
            }

            var model = new MakeAppealViewModel
            {
                ApplicationId = applicationId
            };

            return View("~/Views/Appeals/MakeAppeal.cshtml", model);
        }

        [HttpPost("application/{applicationId}/appeal")]
        [ModelStatePersist(ModelStatePersist.Store)]
        public async Task<IActionResult> MakeAppeal(MakeAppealViewModel model)
        {
            if (!await CanMakeAppeal(model.ApplicationId))
            {
                return RedirectToAction("TaskList", "RoatpApplication", new { model.ApplicationId });
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
                return RedirectToAction("TaskList", "RoatpApplication", new { applicationId });
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
        public async Task<IActionResult> GroundsOfAppeal(GroundsOfAppealViewModel model)
        {
            if (!await CanMakeAppeal(model.ApplicationId))
            {
                return RedirectToAction("TaskList", "RoatpApplication", new { model.ApplicationId });
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("GroundsOfAppeal", new { model.ApplicationId, model.AppealOnPolicyOrProcesses, model.AppealOnEvidenceSubmitted });
            }

            var signInId = User.GetSignInId().ToString();
            var userName = User.Identity.Name;

            if(model.AppealFileToUpload != null)
            {
                await _appealsApiClient.UploadFile(model.ApplicationId, model.AppealFileToUpload, signInId, userName);
            }

            switch(model.FormAction)
            {
                case GroundsOfAppealViewModel.UPLOAD_APPEALFILE_FORMACTION:
                    StoreUserInputInTempData(model);
                    return RedirectToAction("GroundsOfAppeal", new { model.ApplicationId, model.AppealOnPolicyOrProcesses, model.AppealOnEvidenceSubmitted });

                case GroundsOfAppealViewModel.SUBMIT_APPEAL_FORMACTION:
                default:
                    await _appealsApiClient.MakeAppeal(model.ApplicationId, model.HowFailedOnPolicyOrProcesses, model.HowFailedOnEvidenceSubmitted, signInId, userName);
                    return RedirectToAction("AppealSubmitted", new { model.ApplicationId });
            }
        }

        [HttpGet("application/{applicationId}/appeal-submitted")]
        [ModelStatePersist(ModelStatePersist.RestoreEntry)]
        public IActionResult AppealSubmitted(Guid applicationId)
        {
            var model = new AppealSubmittedViewModel
            {
                ApplicationId = applicationId
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

            return RedirectToAction("TaskList", "RoatpApplication", new { applicationId });
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

        [HttpGet("application/{applicationId}/appeal/file/delete/{fileName}")]
        [Authorize(Policy = "AccessAppealNotYetSubmitted")]
        public async Task<IActionResult> DeleteAppealFile(Guid applicationId, string fileName, bool appealOnPolicyOrProcesses, bool appealOnEvidenceSubmitted)
        {
            if (!await CanMakeAppeal(applicationId))
            {
                return RedirectToAction("TaskList", "RoatpApplication", new { applicationId });
            }

            var signInId = User.GetSignInId().ToString();
            var userName = User.Identity.Name;

            await _appealsApiClient.DeleteFile(applicationId, fileName, signInId, userName);

            return RedirectToAction("GroundsOfAppeal", new { applicationId, appealOnPolicyOrProcesses, appealOnEvidenceSubmitted });
        }

        private void StoreUserInputInTempData(GroundsOfAppealViewModel model)
        {
            TempData["HowFailedOnEvidenceSubmitted"] = model.HowFailedOnEvidenceSubmitted;
            TempData["HowFailedOnPolicyOrProcesses"] = model.HowFailedOnPolicyOrProcesses;
        }

        private void RestoreUserInputFromTempData(GroundsOfAppealViewModel model)
        {
            if(TempData["HowFailedOnEvidenceSubmitted"] != null)
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
