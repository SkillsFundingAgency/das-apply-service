namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using SFA.DAS.ApplyService.Web.Infrastructure;
    using System.Threading.Tasks;
    using Domain.Apply;
    using Domain.CharityCommission;
    using Domain.CompaniesHouse;
    using Domain.Roatp;
    using Domain.Ukrlp;
    using global::AutoMapper;
    using InternalApi.Types.CharityCommission;
    using Session;
    using ViewModels.Roatp;
    using Validators;
    using Microsoft.AspNetCore.Authorization;
    using SFA.DAS.ApplyService.InternalApi.Types;
    using Trustee = Domain.CharityCommission.Trustee;

    [Authorize]
    public class RoatpApplicationPreambleController : Controller
    {
        private ILogger<RoatpApplicationPreambleController> _logger;
        private IRoatpApiClient _roatpApiClient;
        private IUkrlpApiClient _ukrlpApiClient;
        private ISessionService _sessionService;
        private ICompaniesHouseApiClient _companiesHouseApiClient;
        private ICharityCommissionApiClient _charityCommissionApiClient;
        private IOrganisationApiClient _organisationApiClient;
        private readonly IUsersApiClient _usersApiClient;

        private const string ApplicationDetailsKey = "Roatp_Application_Details";
        
        private string[] StatusOnlyCompanyNumberPrefixes = new[] { "IP", "SP", "IC", "SI", "NP", "NV", "RC", "SR", "NR", "NO" };

        private string[] ExcludedCharityCommissionPrefixes = new[] {"SC", "NI"};

        public RoatpApplicationPreambleController(ILogger<RoatpApplicationPreambleController> logger, IRoatpApiClient roatpApiClient, 
                                                  IUkrlpApiClient ukrlpApiClient, ISessionService sessionService, 
                                                  ICompaniesHouseApiClient companiesHouseApiClient, 
                                                  ICharityCommissionApiClient charityCommissionApiClient,
                                                  IOrganisationApiClient organisationApiClient,
                                                  IUsersApiClient usersApiClient)
        {
            _logger = logger;
            _roatpApiClient = roatpApiClient;
            _ukrlpApiClient = ukrlpApiClient;
            _sessionService = sessionService;
            _companiesHouseApiClient = companiesHouseApiClient;
            _charityCommissionApiClient = charityCommissionApiClient;
            _organisationApiClient = organisationApiClient;
            _usersApiClient = usersApiClient;
        }

        [Route("select-application-route")]
        public async Task<IActionResult> SelectApplicationRoute()
        {
            var applicationRoutes = await _roatpApiClient.GetApplicationRoutes();

            var viewModel = new SelectApplicationRouteViewModel
            {
                ApplicationRoutes = applicationRoutes
            };

            return View("~/Views/Roatp/SelectApplicationRoute.cshtml", viewModel);
        }

        [Route("enter-your-ukprn")]
        public async Task<IActionResult> EnterApplicationUkprn(SelectApplicationRouteViewModel model)
        {
            model.ApplicationRoutes = await _roatpApiClient.GetApplicationRoutes();

            if (!ModelState.IsValid)
            {
                return View("~/Views/Roatp/SelectApplicationRoute.cshtml", model);
            }

            var applicationDetails = new ApplicationDetails
            {
                ApplicationRoute = model.ApplicationRoutes.FirstOrDefault(x => x.Id == model.ApplicationRouteId)
            };

            _sessionService.Set(ApplicationDetailsKey, applicationDetails);

            var viewModel = new SearchByUkprnViewModel { ApplicationRouteId = model.ApplicationRouteId };

            return View("~/Views/Roatp/EnterApplicationUkprn.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SearchByUkprn(SearchByUkprnViewModel model)
        {
            long ukprn;
            if (!UkprnValidator.IsValidUkprn(model.UKPRN, out ukprn))
            {
                ModelState.AddModelError(nameof(model.UKPRN), "Enter a valid UKPRN");
                TempData["ShowErrors"] = true;

                return View("~/Views/Roatp/EnterApplicationUkprn.cshtml", model);
            }
            
            var matchingResults = await _ukrlpApiClient.GetTrainingProviderByUkprn(ukprn);

            if (matchingResults.Any())
            {
                var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);
                applicationDetails.UkrlpLookupDetails = matchingResults.FirstOrDefault();

                _sessionService.Set(ApplicationDetailsKey, applicationDetails);

                var registerStatus = await _roatpApiClient.UkprnOnRegister(ukprn);

                if (registerStatus.ExistingUKPRN)
                {
                    if (registerStatus.ProviderTypeId != applicationDetails.ApplicationRoute.Id
                        || registerStatus.StatusId == OrganisationRegisterStatus.RemovedStatus)
                    {
                        return RedirectToAction("UkprnFound");
                    }
                    else
                    {
                        return RedirectToAction("UkprnActive");
                    }
                }
                
                return RedirectToAction("UkprnFound");
            }
            else
            {
                var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);
                applicationDetails.UKPRN = ukprn;

                _sessionService.Set(ApplicationDetailsKey, applicationDetails);
                return RedirectToAction("UkprnNotFound");
            }
        }

        [Route("confirm-organisation-details")]
        public async Task<IActionResult> UkprnFound()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);
            var providerDetails = applicationDetails.UkrlpLookupDetails;
            CompaniesHouseSummary companyDetails = null;
            Charity charityDetails = null;

            if (providerDetails.VerifiedByCompaniesHouse)
            {
                var companiesHouseVerification = providerDetails.VerificationDetails.FirstOrDefault(x =>
                        x.VerificationAuthority == VerificationAuthorities.CompaniesHouseAuthority);

                companyDetails = await _companiesHouseApiClient.GetCompanyDetails(companiesHouseVerification.VerificationId);
                
                if (!CompanyReturnsFullDetails(companyDetails.CompanyNumber))
                {
                    companyDetails.ManualEntryRequired = true;
                }
                
                if (String.IsNullOrWhiteSpace(companyDetails.Status) 
                    || companyDetails.Status.ToLower() != CompaniesHouseSummary.CompanyStatusActive)
                {
                    if (companyDetails.Status == CompaniesHouseSummary.CompanyStatusNotFound)
                    {
                        return RedirectToAction("CompanyNotFound");
                    }
                    return RedirectToAction("CompanyNotActive");
                }

                if (!ProviderHistoryValidator.HasSufficientHistory(applicationDetails.ApplicationRoute.Id,
                    companyDetails.IncorporationDate))
                {
                    return RedirectToAction("InvalidCompanyTradingHistory");
                }

                applicationDetails.CompanySummary = companyDetails;
            }

            if (applicationDetails.UkrlpLookupDetails.VerifiedbyCharityCommission)
            {
                var charityCommissionVerification = providerDetails.VerificationDetails.FirstOrDefault(x =>
                    x.VerificationAuthority == VerificationAuthorities.CharityCommissionAuthority);

                int charityNumber;
                string verificationId = charityCommissionVerification.VerificationId;
                if (verificationId.Contains("-"))
                {
                    verificationId = verificationId.Substring(0, verificationId.IndexOf("-"));
                }

                if (IsEnglandAndWalesCharityCommissionNumber(verificationId))
                {
                    bool isValidCharityNumber = int.TryParse(verificationId, out charityNumber);
                    if (!isValidCharityNumber)
                    {
                        return RedirectToAction("CharityNotFound");
                    }

                    charityDetails = await _charityCommissionApiClient.GetCharityDetails(charityNumber);

                    if (!charityDetails.IsActivelyTrading)
                    {
                        return RedirectToAction("CharityNotActive");
                    }

                    if (!ProviderHistoryValidator.HasSufficientHistory(applicationDetails.ApplicationRoute.Id,
                        charityDetails.IncorporatedOn))
                    {
                        return RedirectToAction("InvalidCharityFormationHistory");
                    }

                    applicationDetails.CharitySummary = Mapper.Map<CharityCommissionSummary>(charityDetails);
                }
                else
                {
                    applicationDetails.CharitySummary = new CharityCommissionSummary
                    {
                        CharityNumber = charityCommissionVerification.VerificationId,
                        TrusteeManualEntryRequired = true,
                        Trustees = new List<Trustee>()
                    };
                }
            }

            _sessionService.Set(ApplicationDetailsKey, applicationDetails);

                var viewModel = new UkprnSearchResultsViewModel
                {
                    ProviderDetails = applicationDetails.UkrlpLookupDetails,
                    ApplicationRouteId = applicationDetails.ApplicationRoute.Id,
                    UKPRN = applicationDetails.UkrlpLookupDetails.UKPRN,
                    CompaniesHouseInformation = applicationDetails.CompanySummary,
                    CharityCommissionInformation = applicationDetails.CharitySummary
                };
            

            return View("~/Views/Roatp/UkprnFound.cshtml", viewModel);
        }

        [Route("organisation-not-found")]
        public async Task<IActionResult> UkprnNotFound()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

            var viewModel = new UkprnSearchResultsViewModel
            {
                ApplicationRouteId = applicationDetails.ApplicationRoute.Id,
                UKPRN = applicationDetails.UKPRN.ToString()
            };

            return View("~/Views/Roatp/UkprnNotFound.cshtml", viewModel);
        }

        [Route("already-on-register")]
        public async Task<IActionResult> UkprnActive()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

            var viewModel = new UkprnSearchResultsViewModel
            {
                ApplicationRouteId = applicationDetails.ApplicationRoute.Id,
                UKPRN = applicationDetails.UKPRN.ToString()
            };

            return View("~/Views/Roatp/UkprnActive.cshtml", viewModel);
        }

        [Route("company-not-active")]
        public async Task<IActionResult> CompanyNotActive()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

            var viewModel = new UkprnSearchResultsViewModel
            {
                ApplicationRouteId = applicationDetails.ApplicationRoute.Id,
                UKPRN = applicationDetails.UKPRN.ToString()
            };

            return View("~/Views/Roatp/CompanyNotActive.cshtml", viewModel);
        }

        [Route("company-not-found")]
        public async Task<IActionResult> CompanyNotFound()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

            var viewModel = new UkprnSearchResultsViewModel
            {
                ApplicationRouteId = applicationDetails.ApplicationRoute.Id,
                UKPRN = applicationDetails.UKPRN.ToString()
            };

            return View("~/Views/Roatp/CompanyNotFound.cshtml", viewModel);
        }

        [Route("charity-not-active")]
        public async Task<IActionResult> CharityNotActive()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

            var viewModel = new UkprnSearchResultsViewModel
            {
                ApplicationRouteId = applicationDetails.ApplicationRoute.Id,
                UKPRN = applicationDetails.UKPRN.ToString()
            };

            return View("~/Views/Roatp/CharityNotActive.cshtml", viewModel);
        }

        [Route("charity-not-found")]
        public async Task<IActionResult> CharityNotFound()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

            var viewModel = new UkprnSearchResultsViewModel
            {
                ApplicationRouteId = applicationDetails.ApplicationRoute.Id,
                UKPRN = applicationDetails.UKPRN.ToString()
            };

            return View("~/Views/Roatp/CharityNotFound.cshtml", viewModel);
        }

        public async Task<IActionResult> InvalidCompanyTradingHistory()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

            var viewModel = new UkprnSearchResultsViewModel
            {
                ApplicationRouteId = applicationDetails.ApplicationRoute.Id,
                UKPRN = applicationDetails.UKPRN.ToString()
            };

            return View("~/Views/Roatp/InvalidCompanyTradingHistory.cshtml", viewModel);
        }

        public async Task<IActionResult> InvalidCharityFormationHistory()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

            var viewModel = new UkprnSearchResultsViewModel
            {
                ApplicationRouteId = applicationDetails.ApplicationRoute.Id,
                UKPRN = applicationDetails.UKPRN.ToString()
            };

            return View("~/Views/Roatp/InvalidCharityFormationHistory.cshtml", viewModel);
        }

        [Route("start-application")]
        public async Task<IActionResult> StartApplication()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

            var user = await _usersApiClient.GetUserBySignInId(User.GetSignInId());

            applicationDetails.CreatedBy = user.Id;

            var createOrganisationRequest = Mapper.Map<CreateOrganisationRequest>(applicationDetails);
            
            var organisation = await _organisationApiClient.Create(createOrganisationRequest, user.Id);

            if (!user.IsApproved)
            {
                await _usersApiClient.ApproveUser(user.Id);
            }

            return RedirectToAction("Applications", "Application", new { applicationType = ApplicationTypes.RegisterTrainingProviders } );
        }

        private bool CompanyReturnsFullDetails(string companyNumber)
        {
            if (String.IsNullOrWhiteSpace(companyNumber))
            {
                return false;
            }

            foreach (var prefix in StatusOnlyCompanyNumberPrefixes)
            {
                if (companyNumber.ToUpper().StartsWith(prefix))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsEnglandAndWalesCharityCommissionNumber(string charityNumber)
        {
            if (String.IsNullOrWhiteSpace(charityNumber))
            {
                return false;
            }

            foreach (var prefix in ExcludedCharityCommissionPrefixes)
            {
                if (charityNumber.ToUpper().StartsWith(prefix))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
