using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Infrastructure.Interfaces;

namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    [Authorize(Policy = "AccessInProgressApplication")]
    public class RoatpWhosInControlRefreshController : RoatpApplyControllerBase
    {
        private readonly IRefreshTrusteesService _refreshTrusteesService;

        public RoatpWhosInControlRefreshController(IRefreshTrusteesService refreshTrusteesService, ISessionService sessionService) : base(sessionService)
        {
            _refreshTrusteesService = refreshTrusteesService;
        }
       

        [HttpGet("refresh-trustees")]
        public async Task<IActionResult> RefreshTrustees(Guid applicationId)
        {
            var userId = User.GetUserId();
            var trusteeResult = await _refreshTrusteesService.RefreshTrustees(applicationId, userId);
            if (trusteeResult.CharityDetailsNotFound)
            {
                return RedirectToAction("CharityNotFoundRefresh", "RoatpShutterPages", new { trusteeResult.CharityNumber });
            }

            return RedirectToAction("ConfirmTrustees", "RoatpWhosInControlApplication", new { applicationId });
        }
    }
}
