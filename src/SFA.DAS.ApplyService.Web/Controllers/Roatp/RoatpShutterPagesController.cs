using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    [Authorize]
    public class RoatpShutterPagesController : Controller
    {
        private readonly ISessionService _sessionService;

        private const string ApplicationDetailsKey = "Roatp_Application_Details";

        public RoatpShutterPagesController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        
        [Route("not-accept-terms-conditions")]
        public async Task<IActionResult> TermsAndConditionsNotAgreed(ConditionsOfAcceptanceViewModel model)
        {
            return View("~/Views/Roatp/TermsAndConditionsNotAgreed.cshtml", model);
        }

        [Route("uk-provider-reference-number-not-found")]
        public async Task<IActionResult> UkprnNotFound()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

            var viewModel = new UkprnSearchResultsViewModel
            {
                UKPRN = applicationDetails.UKPRN.ToString()
            };

            return View("~/Views/Roatp/UkprnNotFound.cshtml", viewModel);
        }

        [Route("company-not-found")]
        public async Task<IActionResult> CompanyNotFound()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

            var viewModel = new UkprnSearchResultsViewModel
            {
                UKPRN = applicationDetails.UKPRN.ToString(),
                ProviderDetails = applicationDetails.UkrlpLookupDetails
            };

            return View("~/Views/Roatp/CompanyNotFound.cshtml", viewModel);
        }

        [Route("charity-not-found")]
        public async Task<IActionResult> CharityNotFound()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

            var viewModel = new UkprnSearchResultsViewModel
            {
                UKPRN = applicationDetails.UKPRN.ToString(),
                ProviderDetails = applicationDetails.UkrlpLookupDetails
            };

            return View("~/Views/Roatp/CharityNotFound.cshtml", viewModel);
        }

        [Route("chosen-not-apply-roatp")]
        public async Task<IActionResult> NonLevyAbandonedApplication()
        {
            return View("~/Views/Roatp/NonLevyAbandonedApplication.cshtml");
        }

        [Route("ukrlp-unavailable")]
        public async Task<IActionResult> UkrlpNotAvailable()
        {
            return View("~/Views/Roatp/UkrlpNotAvailable.cshtml");
        }

        [Route("companies-house-unavailable")]
        public async Task<IActionResult> CompaniesHouseNotAvailable()
        {
            return View("~/Views/Roatp/CompaniesHouseNotAvailable.cshtml");
        }

        [Route("charity-commission-unavailable")]
        public async Task<IActionResult> CharityCommissionNotAvailable()
        {
            return View("~/Views/Roatp/CharityCommissionNotAvailable.cshtml");
        }

        [Route("application-in-progress")]
        public async Task<IActionResult> ApplicationInProgress(ExistingApplicationViewModel model)
        {
            return View("~/Views/Roatp/ApplicationInProgress.cshtml", model);
        }

        [Route("application-submitted")]
        public async Task<IActionResult> ApplicationPreviouslySubmitted(ExistingApplicationViewModel model)
        {
            return View("~/Views/Roatp/ApplicationPreviouslySubmitted.cshtml", model);
        }
    }
}
