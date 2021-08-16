using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ApplyService.Application.Services;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp.Appeals;

namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    [Authorize]
    public class RoatpAppealsController : Controller
    {
        private readonly IOutcomeApiClient _apiClient;
        private readonly IOverallOutcomeService _overallOutcomeService;
        private readonly IBankHolidayService _bankHolidayService;

        public RoatpAppealsController(IOutcomeApiClient apiClient, IOverallOutcomeService overallOutcomeService, IBankHolidayService bankHolidayService)
        {
            _apiClient = apiClient;
            _overallOutcomeService = overallOutcomeService;
            _bankHolidayService = bankHolidayService;
        }

        [HttpGet("application/{applicationId}/appeal")]
        [ModelStatePersist(ModelStatePersist.RestoreEntry)]
        public async Task<IActionResult> MakeAppeal(Guid applicationId)
        {
            if (!await WithinAppealWindow(applicationId))
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
            if (!await WithinAppealWindow(model.ApplicationId))
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
            if (!await WithinAppealWindow(applicationId))
            {
                return RedirectToAction("TaskList", "RoatpApplication", new { applicationId });
            }

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
            if (!await WithinAppealWindow(model.ApplicationId))
            {
                return RedirectToAction("TaskList", "RoatpApplication", new { model.ApplicationId });
            }

            return RedirectToAction("GroundsOfAppeal", new { model.ApplicationId, model.AppealOnPolicyOrProcesses, model.AppealOnEvidenceSubmitted });
        }

        [HttpGet]
        [Route("application/{applicationId}/sector/{pageId}")]
        public async Task<IActionResult> GetSectorDetails(Guid applicationId, string pageId)
        {
            var model = await _overallOutcomeService.GetSectorDetailsViewModel(applicationId, pageId);
            return View("~/Views/Roatp/ApplicationUnsuccessfulSectorAnswers.cshtml", model);
        }


        private async Task<bool> WithinAppealWindow(Guid applicationId)
        {
            // NOTE: This is an effective workaround until we fully implement BankHolidayService and have AppealRequiredByDate in the OversightReview
            var oversight = await _apiClient.GetOversightReview(applicationId);

            const int numberOfWorkingDays = 10;
            var appealRequiredByDate = _bankHolidayService.GetWorkingDaysAheadDate(oversight?.ApplicationDeterminedDate, numberOfWorkingDays);

            return appealRequiredByDate.HasValue && appealRequiredByDate >= DateTime.Today;
        }
    }
}
