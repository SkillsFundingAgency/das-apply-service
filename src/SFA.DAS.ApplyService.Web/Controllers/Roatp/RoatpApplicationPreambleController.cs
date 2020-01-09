﻿namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    using System;
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
    using System.Collections.Generic;
    using SFA.DAS.ApplyService.Domain.Entities;
    using SFA.DAS.ApplyService.Application.Apply.Roatp;

    [Authorize]
    public class RoatpApplicationPreambleController : RoatpApplyControllerBase
    {
        private readonly ILogger<RoatpApplicationPreambleController> _logger;
        private readonly IRoatpApiClient _roatpApiClient;
        private readonly IUkrlpApiClient _ukrlpApiClient;
        private readonly ICompaniesHouseApiClient _companiesHouseApiClient;
        private readonly ICharityCommissionApiClient _charityCommissionApiClient;
        private readonly IOrganisationApiClient _organisationApiClient;
        private readonly IUsersApiClient _usersApiClient;
        private readonly IApplicationApiClient _applicationApiClient;
        private readonly IQnaApiClient _qnaApiClient;
        
        private const string GetHelpSubmittedForPageFormatString = "Roatp_GetHelpSubmitted_{0}";

        private string[] StatusOnlyCompanyNumberPrefixes = new[] { "IP", "SP", "IC", "SI", "NP", "NV", "RC", "SR", "NR", "NO" };

        private string[] ExcludedCharityCommissionPrefixes = new[] {"SC", "NI"};

        public RoatpApplicationPreambleController(ILogger<RoatpApplicationPreambleController> logger, IRoatpApiClient roatpApiClient, 
                                                  IUkrlpApiClient ukrlpApiClient, ISessionService sessionService, 
                                                  ICompaniesHouseApiClient companiesHouseApiClient, 
                                                  ICharityCommissionApiClient charityCommissionApiClient,
                                                  IOrganisationApiClient organisationApiClient,
                                                  IUsersApiClient usersApiClient,
                                                  IApplicationApiClient applicationApiClient,
                                                  IQnaApiClient qnaApiClient)
            :base(sessionService)
        {
            _logger = logger;
            _roatpApiClient = roatpApiClient;
            _ukrlpApiClient = ukrlpApiClient;
            _sessionService = sessionService;
            _companiesHouseApiClient = companiesHouseApiClient;
            _charityCommissionApiClient = charityCommissionApiClient;
            _organisationApiClient = organisationApiClient;
            _usersApiClient = usersApiClient;
            _applicationApiClient = applicationApiClient;
            _qnaApiClient = qnaApiClient;
        }

        [Route("terms-conditions-making-application")]
        public IActionResult TermsAndConditions()
        {
            return View("~/Views/Roatp/TermsAndConditions.cshtml", new ConditionsOfAcceptanceViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmTermsAndConditions(ConditionsOfAcceptanceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ErrorMessages = new List<ValidationErrorDetail>();
                var modelErrors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var modelError in modelErrors)
                {
                    model.ErrorMessages.Add(new ValidationErrorDetail
                    {
                        Field = "ConditionsAccepted",
                        ErrorMessage = modelError.ErrorMessage
                    });
                }
                return View("~/Views/Roatp/TermsAndConditions.cshtml", model);
            }

            if (model.ConditionsAccepted != "Y")
            {
                return RedirectToAction("TermsAndConditionsNotAgreed", "RoatpShutterPages");
            }

            return RedirectToAction("EnterApplicationUkprn");
        }
        
        [Route("enter-uk-provider-reference-number")]
        public IActionResult EnterApplicationUkprn(string ukprn)
        {
            var model = new SearchByUkprnViewModel();
            if (!String.IsNullOrWhiteSpace(ukprn))
            {
                model.UKPRN = ukprn;
            }

            PopulateGetHelpWithQuestion(model, "UKPRN");

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
                model.ErrorMessages = new List<ValidationErrorDetail>
                {
                    new ValidationErrorDetail { Field = "UKPRN", ErrorMessage = validationMessage }
                };

                return View("~/Views/Roatp/EnterApplicationUkprn.cshtml", model);
            }
            
            var ukrlpLookupResults = await _ukrlpApiClient.GetTrainingProviderByUkprn(ukprn);

            if (!ukrlpLookupResults.Success)
            {
                return RedirectToAction("UkrlpNotAvailable", "RoatpShutterPages");
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
                return RedirectToAction("UkprnNotFound", "RoatpShutterPages");
            }
        }

        [Route("confirm-organisations-details")]
        public IActionResult ConfirmOrganisation()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);
            
            var viewModel = new UkprnSearchResultsViewModel
            {
                ProviderDetails = applicationDetails.UkrlpLookupDetails,
                UKPRN = applicationDetails.UkrlpLookupDetails.UKPRN
            };

            return View("~/Views/Roatp/ConfirmOrganisation.cshtml", viewModel);
        }
                
        [Route("start-application")]
        [HttpPost]
        public async Task<IActionResult> StartApplication(SelectApplicationRouteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ApplicationRoutes = await GetApplicationRoutesForOrganisation();
                model.ErrorMessages = new List<ValidationErrorDetail>();
                var modelErrors = ModelState.Values.SelectMany(v => v.Errors);
                foreach(var modelError in modelErrors)
                {
                    model.ErrorMessages.Add(new ValidationErrorDetail
                    {
                        Field = "ApplicationRouteId",
                        ErrorMessage = modelError.ErrorMessage
                    });
                }

                return View("~/Views/Roatp/SelectApplicationRoute.cshtml", model);
            }

            if (model.ApplicationRouteId == ApplicationRoute.EmployerProviderApplicationRoute)
            {
                var applicationRoutes = await GetApplicationRoutesForOrganisation();
                var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);
                applicationDetails.ApplicationRoute = applicationRoutes.FirstOrDefault(x => x.Id == model.ApplicationRouteId);
                _sessionService.Set(ApplicationDetailsKey, applicationDetails);

                return RedirectToAction("ConfirmLevyStatus");
            }

            return await StartRoatpApplication(model);
        }

        [Route("organisation-levy-paying-employer")]
        public IActionResult ConfirmLevyStatus()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);
            var viewModel = new EmployerLevyStatusViewModel
            {
                UKPRN = applicationDetails.UKPRN.ToString(),
                ApplicationRouteId = applicationDetails.ApplicationRoute.Id
            };

            PopulateGetHelpWithQuestion(viewModel, "ConfirmLevyStatus");

            return View("~/Views/Roatp/ConfirmLevyStatus.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitLevyStatus(EmployerLevyStatusViewModel model)
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

            if (!ModelState.IsValid)
            {
                model.ErrorMessages = new List<ValidationErrorDetail>();
                var modelErrors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var modelError in modelErrors)
                {
                    model.ErrorMessages.Add(new ValidationErrorDetail
                    {
                        Field = "LevyPayingEmployer",
                        ErrorMessage = modelError.ErrorMessage
                    });
                }
                return View("~/Views/Roatp/ConfirmLevyStatus.cshtml", model);
            }
            
            applicationDetails.LevyPayingEmployer = model.LevyPayingEmployer;
            _sessionService.Set(ApplicationDetailsKey, applicationDetails);

            if (applicationDetails.LevyPayingEmployer == "Y")
            {
                var selectApplicationRouteModel = new SelectApplicationRouteViewModel
                {
                    ApplicationRouteId = applicationDetails.ApplicationRoute.Id
                };
                return await StartRoatpApplication(selectApplicationRouteModel);
            }
            return RedirectToAction("IneligibleNonLevy");
        }

        [Route("organisation-cannot-apply-employer")]
        public IActionResult IneligibleNonLevy()
        {
            var model = new EmployerProviderContinueApplicationViewModel();
            PopulateGetHelpWithQuestion(model, "IneligibleNonLevy");
            return View("~/Views/Roatp/IneligibleNonLevy.cshtml", model);
        }

        [HttpPost]
        public IActionResult ConfirmNonLevyContinue(EmployerProviderContinueApplicationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ErrorMessages = new List<ValidationErrorDetail>();
                var modelErrors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var modelError in modelErrors)
                {
                    model.ErrorMessages.Add(new ValidationErrorDetail
                    {
                        Field = "ContinueWithApplication",
                        ErrorMessage = modelError.ErrorMessage
                    });
                }
                return View("~/Views/Roatp/IneligibleNonLevy.cshtml", model);
            }

            if (model.ContinueWithApplication == "Y")
            {
                return RedirectToAction("SelectApplicationRoute");
            }

            return RedirectToAction("NonLevyAbandonedApplication", "RoatpShutterPages");
        }
                
        [Route("choose-provider-route")]
        public async Task<IActionResult> SelectApplicationRoute()
        {
            var model = new SelectApplicationRouteViewModel();
            var applicationRoutes = await GetApplicationRoutesForOrganisation();

            model.ApplicationRoutes = applicationRoutes;

            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);
            if (applicationDetails.ApplicationRoute != null)
            {
                model.ApplicationRouteId = applicationDetails.ApplicationRoute.Id;
            }

            PopulateGetHelpWithQuestion(model, "ApplicationRoute");

            return View("~/Views/Roatp/SelectApplicationRoute.cshtml", model);
        }

        public async Task<IActionResult> VerifyOrganisationDetails()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);
            var providerDetails = applicationDetails.UkrlpLookupDetails;

            var existingApplicationStatuses = await _applicationApiClient.GetExistingApplicationStatus(providerDetails.UKPRN);
            
            if (existingApplicationStatuses.Any(x => x.Status == ApplicationStatus.InProgress))
            {
                return RedirectToAction("ApplicationInProgress", "RoatpShutterPages", new ExistingApplicationViewModel { UKPRN = providerDetails.UKPRN });
            }

            if (existingApplicationStatuses.Any(x => x.Status == ApplicationStatus.Submitted))
            {
                return RedirectToAction("ApplicationPreviouslySubmitted", "RoatpShutterPages", new ExistingApplicationViewModel { UKPRN = providerDetails.UKPRN });
            }
            
            CompaniesHouseSummary companyDetails = null;
            Charity charityDetails = null;

            if (providerDetails.VerifiedByCompaniesHouse)
            {
                var companiesHouseVerification = providerDetails.VerificationDetails.FirstOrDefault(x =>
                        x.VerificationAuthority == VerificationAuthorities.CompaniesHouseAuthority);

                companyDetails = await _companiesHouseApiClient.GetCompanyDetails(companiesHouseVerification.VerificationId);

                if ((companyDetails.Directors == null || companyDetails.Directors.Count == 0)
                    && (companyDetails.PersonsSignificationControl == null ||
                        companyDetails.PersonsSignificationControl.Count == 0))
                {
                    companyDetails.ManualEntryRequired = true;
                }
                
                if (companyDetails.Status == CompaniesHouseSummary.ServiceUnavailable)
                {
                    return RedirectToAction("CompaniesHouseNotAvailable", "RoatpShutterPages");
                }

                if (companyDetails.Status == CompaniesHouseSummary.CompanyStatusNotFound)
                {
                    return RedirectToAction("CompanyNotFound", "RoatpShutterPages");
                }
                
                if (!CompaniesHouseValidator.CompaniesHouseStatusValid(companyDetails.CompanyNumber, companyDetails.Status))
                {
                    return RedirectToAction("CompanyNotFound", "RoatpShutterPages");
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
                        return RedirectToAction("CharityNotFound", "RoatpShutterPages");
                    }

                    var charityApiResponse = await _charityCommissionApiClient.GetCharityDetails(charityNumber);

                    if (!charityApiResponse.Success)
                    {
                        return RedirectToAction("CharityCommissionNotAvailable", "RoatpShutterPages");
                    } 
                    charityDetails = charityApiResponse.Response;

                    if (charityDetails == null || !charityDetails.IsActivelyTrading)
                    {
                        return RedirectToAction("CharityNotFound", "RoatpShutterPages");
                    }
                    
                    applicationDetails.CharitySummary = Mapper.Map<CharityCommissionSummary>(charityDetails);
                }
                else
                {
                    applicationDetails.CharitySummary = new CharityCommissionSummary
                    {
                        TrusteeManualEntryRequired = true
                    };
                }
            }

            var roatpRegisterStatus = await _roatpApiClient.GetOrganisationRegisterStatus(applicationDetails.UKPRN);

            applicationDetails.RoatpRegisterStatus = roatpRegisterStatus;
            
            _sessionService.Set(ApplicationDetailsKey, applicationDetails);


            if (ProviderEligibleToChangeRoute(roatpRegisterStatus))
            {
                return RedirectToAction("ProviderAlreadyOnRegister");
            }

            return RedirectToAction("SelectApplicationRoute");           
        }

        private async Task<IActionResult> StartRoatpApplication(SelectApplicationRouteViewModel model)
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);
            applicationDetails.ApplicationRoute = new ApplicationRoute { Id = model.ApplicationRouteId };

            var user = await _usersApiClient.GetUserBySignInId(User.GetSignInId());

            applicationDetails.CreatedBy = user.Id;

            var createOrganisationRequest = Mapper.Map<CreateOrganisationRequest>(applicationDetails);

            var organisation = await _organisationApiClient.Create(createOrganisationRequest, user.Id);

            _sessionService.Set(ApplicationDetailsKey, applicationDetails);

            if (!user.IsApproved)
            {
                await _usersApiClient.ApproveUser(user.Id);
            }

            return RedirectToAction("Applications", "RoatpApplication", new { applicationType = ApplicationTypes.RegisterTrainingProviders });
		}
		
        [Route("already-on-roatp")]
        public async Task<IActionResult> ProviderAlreadyOnRegister()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

            var providerRoutes = await _roatpApiClient.GetApplicationRoutes();

            var existingProviderRoute = providerRoutes.FirstOrDefault(x => x.Id == applicationDetails.RoatpRegisterStatus.ProviderTypeId);

            var model = new ChangeProviderRouteViewModel
            {
                UKPRN = applicationDetails.UKPRN.ToString(),
                CurrentProviderType = existingProviderRoute
            };

            PopulateGetHelpWithQuestion(model, "AlreadyOnRegister");

            return View("~/Views/Roatp/ProviderAlreadyOnRegister.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeProviderRoute(ChangeProviderRouteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

                var providerRoutes = await _roatpApiClient.GetApplicationRoutes();

                var existingProviderRoute = providerRoutes.FirstOrDefault(x => x.Id == applicationDetails.RoatpRegisterStatus.ProviderTypeId);

                model = new ChangeProviderRouteViewModel
                {
                    UKPRN = applicationDetails.UKPRN.ToString(),
                    CurrentProviderType = existingProviderRoute,
                    ErrorMessages = new List<ValidationErrorDetail>()
                };

                var modelErrors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var modelError in modelErrors)
                {
                    model.ErrorMessages.Add(new ValidationErrorDetail
                    {
                        Field = "ChangeApplicationRoute",
                        ErrorMessage = modelError.ErrorMessage
                    });
                }

                return View("~/Views/Roatp/ProviderAlreadyOnRegister.cshtml", model);
            }

            if (model.ChangeApplicationRoute != "Y")
            {                            
                return RedirectToAction("ChosenToRemainOnRegister", model);
            }
            else
            {
                return RedirectToAction("SelectApplicationRoute");
            }
        }
        
        [Route("chosen-stay-on-roatp")]
        public async Task<IActionResult> ChosenToRemainOnRegister()
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

            var providerRoutes = await _roatpApiClient.GetApplicationRoutes();

            var existingProviderRoute = providerRoutes.FirstOrDefault(x => x.Id == applicationDetails.RoatpRegisterStatus.ProviderTypeId);

            var model = new ChangeProviderRouteViewModel
            {
                UKPRN = applicationDetails.UKPRN.ToString(),
                CurrentProviderType = existingProviderRoute
            };

            return View("~/Views/Roatp/ChosenToRemainOnRegister.cshtml", model);
        }

        [Route("change-ukprn")]
        [HttpGet]
        public async Task<IActionResult> ChangeUkprn(Guid applicationId)
        {
            var model = new ChangeUkprnViewModel { ApplicationId = applicationId };

            return View("~/Views/Roatp/ChangeUkprn.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmChangeUkprn(ChangeUkprnViewModel model)
        {
            var ukprnAnswer = await _qnaApiClient.GetAnswerByTag(model.ApplicationId, RoatpWorkflowQuestionTags.UKPRN);
            if (ukprnAnswer != null && ukprnAnswer.Value != null)
            {
                _logger.LogInformation($"Cancelling RoATP application for UKPRN {ukprnAnswer.Value}");
            }

            _sessionService.Remove(ApplicationDetailsKey);

            return RedirectToAction("TermsAndConditions");
        }
        
        private bool ProviderEligibleToChangeRoute(OrganisationRegisterStatus roatpRegisterStatus)
        {
            if (roatpRegisterStatus.UkprnOnRegister 
                && (roatpRegisterStatus.StatusId == OrganisationStatus.Active 
                || roatpRegisterStatus.StatusId == OrganisationStatus.ActiveNotTakingOnApprentices
                || roatpRegisterStatus.StatusId == OrganisationStatus.Onboarding))
            {
                return true;
            }

            return false;
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
               
        private async Task<List<ApplicationRoute>> GetApplicationRoutesForOrganisation()
        {
            var applicationRoutes = (await _roatpApiClient.GetApplicationRoutes()).ToList();

            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);
            if (applicationDetails != null 
                && applicationDetails.RoatpRegisterStatus !=null 
                && applicationDetails.RoatpRegisterStatus.UkprnOnRegister
                && applicationDetails.RoatpRegisterStatus.StatusId != OrganisationStatus.Removed)
            {
                var existingRoute = applicationRoutes.FirstOrDefault(x => x.Id == applicationDetails.RoatpRegisterStatus.ProviderTypeId);
                if (existingRoute != null)
                {
                    applicationRoutes.Remove(existingRoute);
                }
            }

            return applicationRoutes;
        }

    }
}
