using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

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

        [Route("one-unsuccessful-application-within-twelve-months")]
        public IActionResult OneApplicationWithinTwelveMonths()
        {
            return View("~/Views/Roatp/ShutterPages/OneApplicationWithinTwelveMonths.cshtml");
        }

        [Route("not-accepted-terms-and-conditions")]
        public IActionResult TermsAndConditionsNotAgreed(ConditionsOfAcceptanceViewModel model)
        {
            return View("~/Views/Roatp/ShutterPages/TermsAndConditionsNotAgreed.cshtml", model);
        }

        [Route("uk-provider-reference-number-not-found")]
        public IActionResult UkprnNotFound()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

            var viewModel = new UkprnSearchResultsViewModel
            {
                UKPRN = applicationDetails.UKPRN.ToString()
            };

            return View("~/Views/Roatp/ShutterPages/UkprnNotFound.cshtml", viewModel);
        }

        [Route("company-not-found")]
        public IActionResult CompanyNotFound()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

            var viewModel = new UkprnSearchResultsViewModel
            {
                UKPRN = applicationDetails.UKPRN.ToString(),
                ProviderDetails = applicationDetails.UkrlpLookupDetails
            };

            return View("~/Views/Roatp/ShutterPages/CompanyNotFound.cshtml", viewModel);
        }

        [Route("charity-not-found")]
        public IActionResult CharityNotFound()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

            var viewModel = new UkprnSearchResultsViewModel
            {
                UKPRN = applicationDetails.UKPRN.ToString(),
                ProviderDetails = applicationDetails.UkrlpLookupDetails
            };

            return View("~/Views/Roatp/ShutterPages/CharityNotFound.cshtml", viewModel);
        }

        [Route("chosen-not-apply-roatp")]
        public IActionResult NonLevyAbandonedApplication()
        {
            return View("~/Views/Roatp/ShutterPages/NonLevyAbandonedApplication.cshtml");
        }

        [Route("ukrlp-unavailable")]
        public IActionResult UkrlpNotAvailable()
        {
            return View("~/Views/Roatp/ShutterPages/UkrlpNotAvailable.cshtml");
        }

        [Route("companies-house-unavailable")]
        public IActionResult CompaniesHouseNotAvailable()
        {
            return View("~/Views/Roatp/ShutterPages/CompaniesHouseNotAvailable.cshtml");
        }

        [Route("charity-commission-unavailable")]
        public IActionResult CharityCommissionNotAvailable()
        {
            return View("~/Views/Roatp/ShutterPages/CharityCommissionNotAvailable.cshtml");
        }

        [Route("application-in-progress")]
        public IActionResult ApplicationInProgress(ExistingApplicationViewModel model)
        {
            return View("~/Views/Roatp/ShutterPages/ApplicationInProgress.cshtml", model);
        }

        [Route("application-submitted")]
        public IActionResult ApplicationPreviouslySubmitted(ExistingApplicationViewModel model)
        {
            return View("~/Views/Roatp/ShutterPages/ApplicationPreviouslySubmitted.cshtml", model);
        }
    }
}
