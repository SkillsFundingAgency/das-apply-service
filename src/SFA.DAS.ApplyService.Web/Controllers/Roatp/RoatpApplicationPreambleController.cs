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
    using SFA.DAS.ApplyService.Web.Resources;

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

        [Route("terms-conditions-making-application")]
        public async Task<IActionResult> TermsAndConditions()
        {
            return View("~/Views/Roatp/TermsAndConditions.cshtml");
        }

        [Route("not-accept-terms-conditions")]
        public async Task<IActionResult> TermsAndConditionsNotAgreed()
        {
            return View("~/Views/Roatp/TermsAndConditionsNotAgreed.cshtml");
        }

        [Route("enter-uk-provider-reference-number")]
        public async Task<IActionResult> EnterApplicationUkprn(string ukprn)
        {
            var model = new SearchByUkprnViewModel();
            if (!String.IsNullOrWhiteSpace(ukprn))
            {
                model.UKPRN = ukprn;
            }
            return View("~/Views/Roatp/EnterApplicationUkprn.cshtml", model);
        }

        [Route("search-by-ukprn")]
        [HttpPost]
        public async Task<IActionResult> SearchByUkprn(SearchByUkprnViewModel model)
        {
            long ukprn = 0;
            string validationMessage = string.Empty;
            if (String.IsNullOrWhiteSpace(model.UKPRN))
            {
                validationMessage = UkprnValidationMessages.MissingUkprn;
            }
            else
            {
                bool isValidUkprn = UkprnValidator.IsValidUkprn(model.UKPRN, out ukprn);
                if (!isValidUkprn)
                {
                    validationMessage = UkprnValidationMessages.InvalidUkprn;
                }
            }

            if (!String.IsNullOrEmpty(validationMessage))
            {
                ModelState.AddModelError(nameof(model.UKPRN), validationMessage);
                TempData["ShowErrors"] = true;

                return View("~/Views/Roatp/EnterApplicationUkprn.cshtml", model);
            }
            
            var ukrlpLookupResults = await _ukrlpApiClient.GetTrainingProviderByUkprn(ukprn);

            if (!ukrlpLookupResults.Success)
            {
                return RedirectToAction("UkrlpNotAvailable");
            }

            if (ukrlpLookupResults.Results.Any())
            {
                var applicationDetails = new ApplicationDetails
                {
                    UKPRN = ukprn,
                    UkrlpLookupDetails = ukrlpLookupResults.Results.FirstOrDefault()
                };

                _sessionService.Set(ApplicationDetailsKey, applicationDetails);
                
                return RedirectToAction("ConfirmOrganisation");
            }
            else
            {
                var applicationDetails = new ApplicationDetails
                {
                    UKPRN = ukprn
                };

                _sessionService.Set(ApplicationDetailsKey, applicationDetails);
                return RedirectToAction("UkprnNotFound");
            }
        }

        [Route("confirm-organisations-details")]
        public async Task<IActionResult> ConfirmOrganisation()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);
            
            var viewModel = new UkprnSearchResultsViewModel
            {
                ProviderDetails = applicationDetails.UkrlpLookupDetails,
                UKPRN = applicationDetails.UkrlpLookupDetails.UKPRN
            };

            return View("~/Views/Roatp/ConfirmOrganisation.cshtml", viewModel);
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
 
        [Route("already-on-register")]
        public async Task<IActionResult> UkprnActive()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

            var viewModel = new UkprnSearchResultsViewModel
            {
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

            return RedirectToAction("Applications", "Application", new { applicationType = ApplicationTypes.RegisterTrainingProviders });
        }

        public async Task<IActionResult> InvalidCharityFormationHistory()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

            var viewModel = new UkprnSearchResultsViewModel
            {
                UKPRN = applicationDetails.UKPRN.ToString()
            };

            return View("~/Views/Roatp/InvalidCharityFormationHistory.cshtml", viewModel);
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

        private async Task<IActionResult> CheckIfOrganisationAlreadyOnRegister(long ukprn)
        {
            var registerStatus = await _roatpApiClient.UkprnOnRegister(ukprn);

            if (registerStatus.ExistingUKPRN)
            {
                var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);
                if (registerStatus.ProviderTypeId != applicationDetails.ApplicationRoute.Id
                    || registerStatus.StatusId == OrganisationRegisterStatus.RemovedStatus)
                {
                    return RedirectToAction("ConfirmOrganisation");
                }
                else
                {
                    return RedirectToAction("UkprnActive");
                }
            }

            return RedirectToAction("ConfirmOrganisation");
        }
        
        public async Task<IActionResult> VerifyOrganisationDetails()
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

                    if (companyDetails.Status == CompaniesHouseSummary.ServiceUnavailable)
                    {
                        return RedirectToAction("CompaniesHouseNotAvailable");
                    }
                    return RedirectToAction("CompanyNotActive");
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

                    var charityApiResponse = await _charityCommissionApiClient.GetCharityDetails(charityNumber);

                    if (!charityApiResponse.Success)
                    {
                        return RedirectToAction("CharityCommissionNotAvailable");
                    } 
                    charityDetails = charityApiResponse.Response;

                    if (!charityDetails.IsActivelyTrading)
                    {
                        return RedirectToAction("CharityNotActive");
                    }
                    
                    applicationDetails.CharitySummary = Mapper.Map<CharityCommissionSummary>(charityDetails);
                }
            }

            _sessionService.Set(ApplicationDetailsKey, applicationDetails);

            return RedirectToAction("StartApplication");
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
