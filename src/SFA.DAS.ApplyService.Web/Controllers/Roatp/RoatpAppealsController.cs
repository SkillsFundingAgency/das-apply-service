using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ApplyService.Application.Services;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp.Appeals;

namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    [Authorize]
    public class RoatpAppealsController : Controller
    {
        private readonly IOutcomeApiClient _outcomeApiClient;
        private readonly IBankHolidayService _bankHolidayService;
        private readonly IAppealsApiClient _appealsApiClient;

        public RoatpAppealsController(IOutcomeApiClient apiClient, IBankHolidayService bankHolidayService, IAppealsApiClient appealsApiClient)
        {
            _outcomeApiClient = apiClient;
            _bankHolidayService = bankHolidayService;
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

            // TODO: Will need to populate any previously uploaded files. THis is done in a different ticket

            var model = new GroundsOfAppealViewModel
            {
                ApplicationId = applicationId,
                AppealOnPolicyOrProcesses = appealOnPolicyOrProcesses,
                AppealOnEvidenceSubmitted = appealOnEvidenceSubmitted
            };

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
            await _appealsApiClient.MakeAppeal(model.ApplicationId, model.HowFailedOnPolicyOrProcesses, model.HowFailedOnEvidenceSubmitted, signInId, userName);

            return RedirectToAction("AppealSubmitted", new { model.ApplicationId });
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

        private async Task<bool> CanMakeAppeal(Guid applicationId)
        {
            var appeal = await _appealsApiClient.GetAppeal(applicationId);

            return appeal is null && await WithinAppealWindow(applicationId);
        }

        private async Task<bool> WithinAppealWindow(Guid applicationId)
        {
            // NOTE: This is an effective workaround until we fully implement BankHolidayService and have AppealRequiredByDate in the OversightReview
            var oversight = await _outcomeApiClient.GetOversightReview(applicationId);

            const int numberOfWorkingDays = 10;
            var appealRequiredByDate = _bankHolidayService.GetWorkingDaysAheadDate(oversight?.ApplicationDeterminedDate, numberOfWorkingDays);

            return appealRequiredByDate.HasValue && appealRequiredByDate >= DateTime.Today;
        }
    }
}
