namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using SFA.DAS.ApplyService.Web.Infrastructure.Interfaces;
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
        private readonly IUkprnWhitelistValidator _ukprnWhitelistValidator;
        private readonly IResetRouteQuestionsService _resetRouteQuestionsService;

        public RoatpApplicationPreambleController(ILogger<RoatpApplicationPreambleController> logger, IRoatpApiClient roatpApiClient,
                                                  IUkrlpApiClient ukrlpApiClient, ISessionService sessionService,
                                                  ICompaniesHouseApiClient companiesHouseApiClient,
                                                  ICharityCommissionApiClient charityCommissionApiClient,
                                                  IOrganisationApiClient organisationApiClient,
                                                  IUsersApiClient usersApiClient,
                                                  IApplicationApiClient applicationApiClient,
                                                  IQnaApiClient qnaApiClient,
                                                  IUkprnWhitelistValidator ukprnWhitelistValidator, 
                                                  IResetRouteQuestionsService resetRouteQuestionsService)
            : base(sessionService)
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
            _ukprnWhitelistValidator = ukprnWhitelistValidator;
            _resetRouteQuestionsService = resetRouteQuestionsService;
        }

        [Route("terms-conditions-making-application")]
        public async Task<IActionResult> TermsAndConditions(SelectApplicationRouteViewModel routeViewModel)
        {
            if (!ModelState.IsValid)
            {
                var model = new SelectApplicationRouteViewModel
                {
                    ApplicationRoutes = await GetApplicationRoutesForOrganisation(),
                    ErrorMessages = new List<ValidationErrorDetail>()
                };

                var modelErrors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var modelError in modelErrors)
                {
                    model.ErrorMessages.Add(new ValidationErrorDetail
                    {
                        Field = "ApplicationRouteId",
                        ErrorMessage = modelError.ErrorMessage
                    });
                }

                return View("~/Views/Roatp/SelectApplicationRoute.cshtml", model);
            }

            if (routeViewModel.ApplicationRouteId == ApplicationRoute.SupportingProviderApplicationRoute)
            {
                return View("~/Views/Roatp/TermsAndConditionsSupporting.cshtml", new ConditionsOfAcceptanceViewModel { ApplicationId = routeViewModel.ApplicationId, ApplicationRouteId = routeViewModel.ApplicationRouteId });
            }
            else
            {
                return View("~/Views/Roatp/TermsAndConditions.cshtml", new ConditionsOfAcceptanceViewModel { ApplicationId = routeViewModel.ApplicationId, ApplicationRouteId = routeViewModel.ApplicationRouteId });
            }
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
                return RedirectToAction("TermsAndConditionsNotAgreed", "RoatpShutterPages", model);
            }
            else if (model.ApplicationId == Guid.Empty)
            {
                return await StartApplication(new SelectApplicationRouteViewModel
                {
                    ApplicationRouteId = model.ApplicationRouteId,
                    ApplicationId = model.ApplicationId
                });

            }
            else
            {
                return RedirectToAction("TaskList", "RoatpApplication", new { applicationId = model.ApplicationId}, "Sequence_1" );
            }
        }

        [Route("enter-uk-provider-reference-number")]
        public IActionResult EnterApplicationUkprn(string ukprn)
        {
            var model = new SearchByUkprnViewModel();
            if (!string.IsNullOrWhiteSpace(ukprn))
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
            int ukprn = 0;
            string validationMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(model.UKPRN))
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
                else if (!await _ukprnWhitelistValidator.IsWhitelistedUkprn(ukprn))
                {
                    validationMessage = UkprnValidationMessages.NotWhitelistedUkprn;
                }
            }

            if (!string.IsNullOrEmpty(validationMessage))
            {
                model.ErrorMessages = new List<ValidationErrorDetail>
                {
                    new ValidationErrorDetail { Field = "UKPRN", ErrorMessage = validationMessage }
                };

                return View("~/Views/Roatp/EnterApplicationUkprn.cshtml", model);
            }

            var ukrlpLookupResults = await _ukrlpApiClient.GetTrainingProviderByUkprn(ukprn);

            if (ukrlpLookupResults?.Results is null || !ukrlpLookupResults.Success)
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
                foreach (var modelError in modelErrors)
                {
                    model.ErrorMessages.Add(new ValidationErrorDetail
                    {
                        Field = "ApplicationRouteId",
                        ErrorMessage = modelError.ErrorMessage
                    });
                }

                return View("~/Views/Roatp/SelectApplicationRoute.cshtml", model);
            }

            if (model.ApplicationId == Guid.Empty)
            {
                return await StartRoatpApplication(model);
            }
            else
            {
                return await UpdateApplicationProviderRoute(model);
            }
        }

        [Route("organisation-levy-paying-employer")]
        public IActionResult ConfirmLevyStatus(Guid applicationId, string ukprn, int applicationRouteId)
        {
            var viewModel = new EmployerLevyStatusViewModel
            {
                ApplicationId = applicationId,
                UKPRN = ukprn,
                ApplicationRouteId = applicationRouteId
            };

            PopulateGetHelpWithQuestion(viewModel, "ConfirmLevyStatus");

            return View("~/Views/Roatp/ConfirmLevyStatus.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitLevyStatus(EmployerLevyStatusViewModel model)
        {
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

            if (model.ApplicationId == Guid.Empty)
            {
                var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);
                applicationDetails.LevyPayingEmployer = model.LevyPayingEmployer;
                _sessionService.Set(ApplicationDetailsKey, applicationDetails);
            }

            if (model.LevyPayingEmployer == "Y")
            {
                var selectApplicationRouteModel = new SelectApplicationRouteViewModel
                {
                    ApplicationRouteId = model.ApplicationRouteId,
                    ApplicationId = model.ApplicationId,
                    LevyPayingEmployer = model.LevyPayingEmployer
                };

                if (selectApplicationRouteModel.ApplicationId == Guid.Empty)
                {
                    return await TermsAndConditions(new SelectApplicationRouteViewModel { ApplicationRouteId = model.ApplicationRouteId });
                }
                else
                {
                    return await UpdateApplicationProviderRoute(selectApplicationRouteModel);
                }
            }

            return RedirectToAction("IneligibleNonLevy", new { applicationId = model.ApplicationId });
        }

        [Route("organisation-cannot-apply-employer")]
        public IActionResult IneligibleNonLevy(Guid applicationId)
        {
            var model = new EmployerProviderContinueApplicationViewModel { ApplicationId = applicationId };
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
                if (model.ApplicationId == Guid.Empty)
                {
                    return RedirectToAction("SelectApplicationRoute");
                }
                else
                {
                    return RedirectToAction("ChangeApplicationProviderRoute", new { applicationId = model.ApplicationId });
                }
            }
            else
            {
                return RedirectToAction("NonLevyAbandonedApplication", "RoatpShutterPages");
            }
        }

        [Route("choose-provider-route")]
        public async Task<IActionResult> SelectApplicationRoute()
        {
            var model = new SelectApplicationRouteViewModel();

            var applicationRoutes = await GetApplicationRoutesForOrganisation();

            model.ApplicationRoutes = applicationRoutes;

            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);
            if (applicationDetails?.ApplicationRoute != null)
            {
                model.ApplicationRouteId = applicationDetails.ApplicationRoute.Id;
            }

            PopulateGetHelpWithQuestion(model, "ApplicationRoute");

            return View("~/Views/Roatp/SelectApplicationRoute.cshtml", model);
        }

        public async Task<IActionResult> ProcessRoute(SelectApplicationRouteViewModel model)
        {
            if (model.ApplicationRouteId == ApplicationRoute.EmployerProviderApplicationRoute)
            {
                var applicationRoutes = await GetApplicationRoutesForOrganisation();
                var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);
                applicationDetails.ApplicationRoute = applicationRoutes.FirstOrDefault(x => x.Id == model.ApplicationRouteId);
                _sessionService.Set(ApplicationDetailsKey, applicationDetails);

                return RedirectToAction("ConfirmLevyStatus", new { ukprn = applicationDetails.UKPRN, applicationRouteId = model.ApplicationRouteId });
            }
            else
            {
                return await TermsAndConditions(new SelectApplicationRouteViewModel { ApplicationRouteId = model.ApplicationRouteId });
            }
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
            else if (existingApplicationStatuses.Any(x => x.Status == ApplicationStatus.Submitted))
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
                    && (companyDetails.PersonsWithSignificantControl == null ||
                        companyDetails.PersonsWithSignificantControl.Count == 0))
                {
                    companyDetails.ManualEntryRequired = true;
                }

                if (companyDetails.Status == CompaniesHouseSummary.ServiceUnavailable)
                {
                    return RedirectToAction("CompaniesHouseNotAvailable", "RoatpShutterPages");
                }
                else if (companyDetails.Status == CompaniesHouseSummary.CompanyStatusNotFound)
                {
                    return RedirectToAction("CompanyNotFound", "RoatpShutterPages");
                }
                else if (!CompaniesHouseValidator.CompaniesHouseStatusValid(companyDetails.CompanyNumber, companyDetails.Status))
                {
                    return RedirectToAction("CompanyNotFound", "RoatpShutterPages");
                }

                applicationDetails.CompanySummary = companyDetails;
            }

            if (applicationDetails.UkrlpLookupDetails.VerifiedbyCharityCommission)
            {
                var charityCommissionVerification = providerDetails.VerificationDetails.FirstOrDefault(x =>
                    x.VerificationAuthority == VerificationAuthorities.CharityCommissionAuthority);

                string verificationId = charityCommissionVerification.VerificationId;
                if (verificationId.Contains("-"))
                {
                    verificationId = verificationId.Substring(0, verificationId.IndexOf("-"));
                }

                if (IsEnglandAndWalesCharityCommissionNumber(verificationId))
                {
                    bool isValidCharityNumber = int.TryParse(verificationId, out var charityNumber);
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
            else
            {
                return RedirectToAction("SelectApplicationRoute");
            }
        }

        private async Task<IActionResult> StartRoatpApplication(SelectApplicationRouteViewModel model)
        {
            _logger.LogDebug("StartRoatpApplication invoked");

            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);
            applicationDetails.ApplicationRoute = new ApplicationRoute { Id = model.ApplicationRouteId };

            var user = await _usersApiClient.GetUserBySignInId(User.GetSignInId());

            applicationDetails.CreatedBy = user.Id;

            var createOrganisationRequest = Mapper.Map<CreateOrganisationRequest>(applicationDetails);

            await _organisationApiClient.Create(createOrganisationRequest, user.Id);

            _sessionService.Set(ApplicationDetailsKey, applicationDetails);

            if (!user.IsApproved)
            {
                await _usersApiClient.ApproveUser(user.Id);
            }

            _logger.LogDebug("StartRoatpApplication completed");

            return RedirectToAction("Applications", "RoatpApplication", new { applicationType = ApplicationTypes.RegisterTrainingProviders });
        }

        [HttpGet]
        public IActionResult ConfirmChangeRoute(Guid applicationId)
        {
            var model = new ConfirmChangeRouteViewModel { ApplicationId = applicationId };
            PopulateGetHelpWithQuestion(model, "ConfirmChangeRoute");
            return View("~/Views/Roatp/ConfirmChangeRoute.cshtml", model);
        }

        [HttpPost]
        public IActionResult SubmitConfirmChangeRoute(ConfirmChangeRouteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ErrorMessages = new List<ValidationErrorDetail>();

                var modelErrors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var modelError in modelErrors)
                {
                    model.ErrorMessages.Add(new ValidationErrorDetail
                    {
                        Field = "ConfirmChangeRoute",
                        ErrorMessage = modelError.ErrorMessage
                    });
                }

                return View("~/Views/Roatp/ConfirmChangeRoute.cshtml", model);
            }

            if (model.ConfirmChangeRoute == "Y")
            {
                return RedirectToAction("ChangeApplicationProviderRoute", new { applicationId = model.ApplicationId });
            }
            else
            {
                return RedirectToAction("TaskList", "RoatpApplication", new { applicationId = model.ApplicationId});
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChangeApplicationProviderRoute(Guid applicationId)
        {
            var model = new SelectApplicationRouteViewModel { ApplicationId = applicationId };
            PopulateGetHelpWithQuestion(model, "ApplicationRoute");
            model.ApplicationRoutes = await GetApplicationRoutesForOrganisation(applicationId);
            var applicationRoute = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.ProviderRoute);
            model.ApplicationRouteId = Convert.ToInt32(applicationRoute.Value);

            return View("~/Views/Roatp/SelectApplicationRoute.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateApplicationProviderRoute(SelectApplicationRouteViewModel model)
        {
            if (model.ApplicationRouteId == ApplicationRoute.EmployerProviderApplicationRoute && model.LevyPayingEmployer != "Y")
            {
                var ukprnAnswer = await _qnaApiClient.GetAnswerByTag(model.ApplicationId, RoatpWorkflowQuestionTags.UKPRN);

                return RedirectToAction("ConfirmLevyStatus", new
                {
                    applicationId = model.ApplicationId,
                    ukprn = ukprnAnswer.Value,
                    applicationRouteId = model.ApplicationRouteId
                });
            }

            var providerRouteAnswer = new List<Answer>
            {
                new Answer
                {
                    QuestionId = RoatpPreambleQuestionIdConstants.ApplyProviderRoute,
                    Value = model.ApplicationRouteId.ToString()
                }
            };

            var result = await _qnaApiClient.UpdatePageAnswers(model.ApplicationId, RoatpWorkflowSequenceIds.Preamble, RoatpWorkflowSectionIds.Preamble, RoatpWorkflowPageIds.ProviderRoute, providerRouteAnswer);

            var providerRoutes = await _roatpApiClient.GetApplicationRoutes();
            var selectedProviderRoute = providerRoutes.FirstOrDefault(x => x.Id == model.ApplicationRouteId);

            if (result.ValidationPassed)
            {
                await _applicationApiClient.ChangeProviderRoute(new ChangeProviderRouteRequest
                {
                    ApplicationId = model.ApplicationId,
                    ProviderRoute = model.ApplicationRouteId,
                    ProviderRouteName = selectedProviderRoute?.RouteName
                });
                
                await _resetRouteQuestionsService.ResetRouteQuestions(model.ApplicationId, model.ApplicationRouteId);
            }
            return RedirectToAction("TermsAndConditions", new { applicationId = model.ApplicationId, applicationRouteId = model.ApplicationRouteId });
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

        private static bool ProviderEligibleToChangeRoute(OrganisationRegisterStatus roatpRegisterStatus)
        {
            var eligibleStatusIds = new List<int?> { OrganisationStatus.Active, OrganisationStatus.ActiveNotTakingOnApprentices, OrganisationStatus.Onboarding };

            return roatpRegisterStatus.UkprnOnRegister && eligibleStatusIds.Contains(roatpRegisterStatus.StatusId);
        }

        private static bool IsEnglandAndWalesCharityCommissionNumber(string charityNumber)
        {
            var excludedCharityCommissionPrefixes = new[] { "SC", "NI" };

            if (string.IsNullOrWhiteSpace(charityNumber))
            {
                return false;
            }

            foreach (var prefix in excludedCharityCommissionPrefixes)
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
            return await GetApplicationRoutesForOrganisation(Guid.Empty);
        }

        private async Task<List<ApplicationRoute>> GetApplicationRoutesForOrganisation(Guid applicationId)
        {
            ApplicationRoute existingRoute = null;
            var applicationRoutes = (await _roatpApiClient.GetApplicationRoutes()).ToList();

            if (applicationId == Guid.Empty)
            {
                var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

                if (applicationDetails?.RoatpRegisterStatus != null
                && applicationDetails.RoatpRegisterStatus.UkprnOnRegister
                && applicationDetails.RoatpRegisterStatus.StatusId != OrganisationStatus.Removed)
                {
                    existingRoute = applicationRoutes.FirstOrDefault(x => x.Id == applicationDetails.RoatpRegisterStatus.ProviderTypeId);
                }
            }
            else
            {
                var ukprn = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.UKPRN);
                var roatpRegisterStatus = await _roatpApiClient.GetOrganisationRegisterStatus(Convert.ToInt32(ukprn.Value));

                if (roatpRegisterStatus != null
                    && roatpRegisterStatus.UkprnOnRegister
                    && roatpRegisterStatus.StatusId != OrganisationStatus.Removed)
                {
                    existingRoute = applicationRoutes.FirstOrDefault(x => x.Id == roatpRegisterStatus.ProviderTypeId);
                }
            }

            if (existingRoute != null)
            {
                applicationRoutes.Remove(existingRoute);
            }

            return applicationRoutes;
        }
    }
}