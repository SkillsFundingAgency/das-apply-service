using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Infrastructure.Interfaces;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    using Configuration;
    using Microsoft.Extensions.Options;
    using MoreLinq;
    using SFA.DAS.ApplyService.EmailService;
    using SFA.DAS.ApplyService.Web.Controllers.Roatp;
    using SFA.DAS.ApplyService.Web.Infrastructure.Validations;
    using SFA.DAS.ApplyService.Web.Services;
    using ViewModels.Roatp;

    [Authorize]
    public class RoatpApplicationController : RoatpApplyControllerBase
    {
        private readonly IApplicationApiClient _apiClient;
        private readonly ILogger<RoatpApplicationController> _logger;
        private readonly IUsersApiClient _usersApiClient;
        private readonly IConfigurationService _configService;
        private readonly IUserService _userService;
        private readonly IQnaApiClient _qnaApiClient;
        private readonly IProcessPageFlowService _processPageFlowService;
        private readonly IQuestionPropertyTokeniser _questionPropertyTokeniser;
        private readonly List<TaskListConfiguration> _configuration;
        private readonly IPageNavigationTrackingService _pageNavigationTrackingService;
        private readonly List<QnaPageOverrideConfiguration> _pageOverrideConfiguration;
        private readonly List<QnaLinksConfiguration> _qnaLinks;
        private readonly List<NotRequiredOverrideConfiguration> _notRequiredOverrides;
        private readonly ICustomValidatorFactory _customValidatorFactory;
        private readonly IRoatpApiClient _roatpApiClient;
        private readonly ISubmitApplicationConfirmationEmailService _submitApplicationEmailService;
        private readonly ITabularDataRepository _tabularDataRepository;
        private readonly IPagesWithSectionsFlowService _pagesWithSectionsFlowService;

        private const string InputClassUpperCase = "app-uppercase";
        private const string NotApplicableAnswerText = "None of the above";
        private const string InvalidCheckBoxListSelectionErrorMessage = "If your answer is 'none of the above', you must only select that option";

        public RoatpApplicationController(IApplicationApiClient apiClient, ILogger<RoatpApplicationController> logger,
            ISessionService sessionService, IConfigurationService configService, IUserService userService, IUsersApiClient usersApiClient,
            IQnaApiClient qnaApiClient, IOptions<List<TaskListConfiguration>> configuration, IProcessPageFlowService processPageFlowService,
            IPagesWithSectionsFlowService pagesWithSectionsFlowService,
        IQuestionPropertyTokeniser questionPropertyTokeniser, IOptions<List<QnaPageOverrideConfiguration>> pageOverrideConfiguration, 
            IPageNavigationTrackingService pageNavigationTrackingService, IOptions<List<QnaLinksConfiguration>> qnaLinks, 
            ICustomValidatorFactory customValidatorFactory, IOptions<List<NotRequiredOverrideConfiguration>> notRequiredOverrides, 
            IRoatpApiClient roatpApiClient, ISubmitApplicationConfirmationEmailService submitApplicationEmailService,
            ITabularDataRepository tabularDataRepository)
            :base(sessionService)
        {
            _apiClient = apiClient;
            _logger = logger;
            _sessionService = sessionService;
            _configService = configService;
            _userService = userService;
            _usersApiClient = usersApiClient;
            _qnaApiClient = qnaApiClient;
            _processPageFlowService = processPageFlowService;
            _pagesWithSectionsFlowService = pagesWithSectionsFlowService;
            _configuration = configuration.Value;
            _questionPropertyTokeniser = questionPropertyTokeniser;
            _pageNavigationTrackingService = pageNavigationTrackingService;
            _qnaLinks = qnaLinks.Value;
            _pageOverrideConfiguration = pageOverrideConfiguration.Value;
            _customValidatorFactory = customValidatorFactory;
            _notRequiredOverrides = notRequiredOverrides.Value;
            _roatpApiClient = roatpApiClient;
            _submitApplicationEmailService = submitApplicationEmailService;
            _tabularDataRepository = tabularDataRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Applications()
        {
            var user = User.Identity.Name;

            if (!await _userService.ValidateUser(user))
                return RedirectToAction("PostSignIn", "Users");

            _logger.LogInformation($"Got LoggedInUser from Session: {user}");

            var applyUser = await _usersApiClient.GetUserBySignInId((await _userService.GetSignInId()).ToString());
            var userId = applyUser?.Id ?? Guid.Empty;

            var applications = await _apiClient.GetApplications(userId, false);
            applications = applications.Where(app => app.ApplicationStatus != ApplicationStatus.Rejected).ToList();

            if (!applications.Any())
            {              
                return await StartApplication(userId);
            }
            
            if (applications.Count > 1)
                return View(applications);

            var application = applications.First();
            
            switch (application.ApplicationStatus)
            {
                case ApplicationStatus.Cancelled:
                    return RedirectToAction("EnterApplicationUkprn", "RoatpApplicationPreamble");
                case ApplicationStatus.Approved:
                    return View("~/Views/Application/Approved.cshtml", application);
                case ApplicationStatus.Rejected:
                    return View("~/Views/Application/Rejected.cshtml", application);
                case ApplicationStatus.FeedbackAdded:
                    return View("~/Views/Application/FeedbackIntro.cshtml", application.ApplicationId);
                case ApplicationStatus.Submitted:
                    return RedirectToAction("ApplicationSubmitted", new { applicationId = application.ApplicationId });
                default:
                    return RedirectToAction("TaskList", new {applicationId = application.ApplicationId });
            }
        }

        private async Task<IActionResult> StartApplication(Guid userId)
        {
            var applicationType = ApplicationTypes.RegisterTrainingProviders;
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

            _logger.LogInformation($"Application Details:: Ukprn: [{applicationDetails?.UKPRN}], ProviderName: [{applicationDetails?.UkrlpLookupDetails?.ProviderName}], RouteId: [{applicationDetails?.ApplicationRoute?.Id}]");
            var providerRoute = applicationDetails.ApplicationRoute.Id;

            var startApplicationData = new JObject
            {
                ["OrganisationReferenceId"] = applicationDetails.UKPRN.ToString(),
                ["OrganisationName"] = applicationDetails.UkrlpLookupDetails.ProviderName,
                ["ApplyProviderRoute"] = providerRoute.ToString()
            };

            var startApplicationJson = JsonConvert.SerializeObject(startApplicationData);
            _logger.LogInformation($"RoatpApplicationController.StartApplication:: Checking applicationStartResponse PRE: userid: [{userId.ToString()}], applicationType: [{applicationType}], startApplicationJson: [{startApplicationJson}]");
            var qnaResponse = await _qnaApiClient.StartApplication(userId.ToString(), applicationType, startApplicationJson);
            _logger.LogInformation($"RoatpApplicationController.StartApplication:: Checking applicationStartResponse POST: applicationId: [{qnaResponse ?.ApplicationId}]");

            if (qnaResponse != null)
            {
                var allQnaSequences = await _qnaApiClient.GetSequences(qnaResponse.ApplicationId);
                var allQnaSections = await _qnaApiClient.GetSections(qnaResponse.ApplicationId);

                var startApplicationRequest = BuildStartApplicationRequest(qnaResponse.ApplicationId, userId, providerRoute, allQnaSequences, allQnaSections);

                var applicationId = await _apiClient.StartApplication(startApplicationRequest);
                _logger.LogInformation($"RoatpApplicationController.StartApplication:: Checking response from StartApplication POST: applicationId: [{applicationId}]");

                if (applicationId != Guid.Empty)
                {
                    await SavePreambleInformation(applicationId, applicationDetails);

                    if (applicationDetails.UkrlpLookupDetails.VerifiedByCompaniesHouse)
                    {
                        await SaveCompaniesHouseInformation(applicationId, applicationDetails);
                    }

                    if (applicationDetails.UkrlpLookupDetails.VerifiedbyCharityCommission)
                    {
                        await SaveCharityCommissionInformation(applicationId, applicationDetails);
                    }
                }
            }

            return RedirectToAction("Applications", new { applicationType });
        }

        private Application.Apply.Start.StartApplicationRequest BuildStartApplicationRequest(Guid qnaApplicationId, Guid creatingContactId, int providerRoute, IEnumerable<ApplicationSequence> qnaSequences, IEnumerable<ApplicationSection> qnaSections)
        {
            var providerRoutes = _roatpApiClient.GetApplicationRoutes().GetAwaiter().GetResult();
            var selectedProviderRoute = providerRoutes.FirstOrDefault(p => p.Id == providerRoute);

            return new Application.Apply.Start.StartApplicationRequest
            {
                ApplicationId = qnaApplicationId,
                CreatingContactId = creatingContactId,
                ProviderRoute = providerRoute,
                ProviderRouteName = selectedProviderRoute?.RouteName,
                ApplySequences = qnaSequences.Select(sequence => new ApplySequence
                {
                    SequenceId = sequence.Id,
                    SequenceNo = sequence.SequenceId,
                    Sections = qnaSections.Where(x => x.SequenceId == sequence.SequenceId).Select(section => new ApplySection
                    {
                        SectionId = section.Id,
                        SectionNo = section.SectionId,
                        //Status = "Draft",
                        //RequestedFeedbackAnswered = false
                    }).OrderBy(section => section.SectionNo).ToList(),
                    //Status = "Draft",
                    //IsActive = sequence.IsActive,                    
                    //Sequential = false
                }).OrderBy(sequence => sequence.SequenceNo).ToList()
            };
        }

        [HttpGet]
        public async Task<IActionResult> SequenceSignPost(Guid applicationId)
        {
            var application = await _apiClient.GetApplication(applicationId);
            if(application is null)
            {
                return RedirectToAction("Applications");
            }
            
            if (application.ApplicationStatus == ApplicationStatus.Approved)
            {
                return View("~/Views/Application/Approved.cshtml", application);
            }

            if (application.ApplicationStatus == ApplicationStatus.Rejected)
            {
                return View("~/Views/Application/Rejected.cshtml", application);
            }

            if (application.ApplicationStatus == ApplicationStatus.FeedbackAdded)
            {
                return View("~/Views/Application/FeedbackIntro.cshtml", application.ApplicationId);
            }

            return RedirectToAction("TaskList", new { applicationId = application.ApplicationId });
        }

        [HttpGet]
        public async Task<IActionResult> Back(Guid applicationId, int sequenceId, int sectionId, string pageId, string redirectAction)
        {
            string previousPageId = await _pageNavigationTrackingService.GetBackNavigationPageId(applicationId, sequenceId, sectionId, pageId);

            if (previousPageId == null)
            {
                return RedirectToAction("TaskList", new { applicationId });
            }

            return RedirectToAction("Page", new { applicationId, sequenceId, sectionId, pageId = previousPageId, redirectAction });
        }

        [HttpGet]
        public async Task<IActionResult> Section(Guid applicationId, int sequenceId, int sectionId)
        {
            var canUpdate = await CanUpdateApplication(applicationId, sequenceId, sectionId);
            if (!canUpdate)
            {
                return RedirectToAction("TaskList", new { applicationId });
            }

            var sequences = await _qnaApiClient.GetSequences(applicationId);
            var selectedSequence = sequences.Single(x => x.SequenceId == sequenceId);
            var sections = await _qnaApiClient.GetSections(applicationId, selectedSequence.Id);
            var selectedSection = sections.Single(x => x.SectionId == sectionId);

            var section = await _qnaApiClient.GetSection(applicationId, selectedSection.Id);

            if (sequenceId == RoatpWorkflowSequenceIds.YourOrganisation && sectionId == RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails)
            {
                await RemoveIrrelevantQuestions(applicationId, section);
            }

            if (section?.DisplayType == SectionDisplayType.PagesWithSections)
            {
                var applicationSection = _pagesWithSectionsFlowService.ProcessPagesInSectionsForStatusText(selectedSection);
                return View("~/Views/Application/PagesWithSections.cshtml", applicationSection);
            }

            var pageId = section.QnAData.Pages.FirstOrDefault()?.PageId;

            return await Page(applicationId, sequenceId, sectionId, pageId, "TaskList",null);
        }

        [HttpGet]
        public async Task<IActionResult> Page(Guid applicationId, int sequenceId, int sectionId, string pageId, string redirectAction, List<Question> answeredQuestions)
        {
            var canUpdate = await CanUpdateApplication(applicationId, sequenceId, sectionId, pageId);
            if (!canUpdate)
            {
                return RedirectToAction("TaskList", new { applicationId });
            }

            _pageNavigationTrackingService.AddPageToNavigationStack(pageId);

            var sequences = await _qnaApiClient.GetSequences(applicationId);
            var selectedSequence = sequences.Single(x => x.SequenceId == sequenceId);
            var sections = await _qnaApiClient.GetSections(applicationId, selectedSequence.Id);
            var selectedSection = sections.Single(x => x.SectionId == sectionId);
            var sectionTitle = selectedSection.LinkTitle;

            var sequence = await _qnaApiClient.GetSequence(applicationId, selectedSequence.Id);
            
            PageViewModel viewModel = null;
            var returnUrl = Request.Headers["Referer"].ToString();

            string pageContext = string.Empty;
            
            var section = await _qnaApiClient.GetSection(applicationId, selectedSection.Id);
            
            if (!ModelState.IsValid)
            {
                // when the model state has errors the page will be displayed with the values which failed validation
                var page = JsonConvert.DeserializeObject<Page>((string) this.TempData["InvalidPage"]);

                var peopleInControlDetails = await GetPeopleInControlDetails(applicationId, sequenceId, sectionId);

                var errorMessages = !ModelState.IsValid
                    ? ModelState.SelectMany(k => k.Value.Errors.Select(e => new ValidationErrorDetail()
                    {
                        ErrorMessage = e.ErrorMessage,
                        Field = k.Key
                    })).DistinctBy(f => f.Field).ToList()
                    : null;

                if (page.ShowTitleAsCaption)
                {
                    page.Title = section.Title;
                }
                
                viewModel = new PageViewModel(applicationId, sequenceId, sectionId, pageId, page, pageContext, redirectAction,
                    returnUrl, errorMessages, _pageOverrideConfiguration, _qnaLinks, sectionTitle, peopleInControlDetails);
            }
            else
            {
                // when the model state has no errors the page will be displayed with the last valid values which were saved
                // AM: I have commented out the code below as we are making identical calls and checks twice
                //var page = await _qnaApiClient.GetPage(applicationId, selectedSection.Id, pageId);
                
                //if (page == null)
                //{
                //    return RedirectToAction("TaskList", new {applicationId = applicationId});
                //}

                var page = await _qnaApiClient.GetPage(applicationId, selectedSection.Id, pageId);
                if (page == null || page.Questions == null)
                {
                    return await TaskList(applicationId);
                }

                page = await GetDataFedOptions(applicationId, page);
                var peopleInControlDetails = await GetPeopleInControlDetails(applicationId, sequenceId, sectionId);

                if (page.ShowTitleAsCaption)
                {
                    page.Title = section.Title;
                }
                
                viewModel = new PageViewModel(applicationId, sequenceId, sectionId, pageId, page, pageContext, redirectAction,
                    returnUrl, null, _pageOverrideConfiguration, _qnaLinks, sectionTitle, peopleInControlDetails);

            }

            viewModel = await TokeniseViewModelProperties(viewModel);

            if (viewModel.AllowMultipleAnswers)
            {
                return View("~/Views/Application/Pages/MultipleAnswers.cshtml", viewModel);
            }

            PopulateGetHelpWithQuestion(viewModel, pageId);         

            return View("~/Views/Application/Pages/Index.cshtml", viewModel);            
        }

        [HttpGet]
        public async Task<IActionResult> Skip(Guid applicationId, int sequenceId, int sectionId, string pageId, string redirectAction)
        {
            var canUpdate = await CanUpdateApplication(applicationId, sequenceId, sectionId, pageId);
            if (!canUpdate)
            {
                return RedirectToAction("TaskList", new { applicationId });
            }

            var nextAction = await _qnaApiClient.SkipPageBySectionNo(applicationId, sequenceId, sectionId, pageId);
            var nextPageId = nextAction?.NextActionId;

            // Note that SkipPage could have updated the section within QnA, so you must get the latest version!
            var section = await _qnaApiClient.GetSectionBySectionNo(applicationId, sequenceId, sectionId);

            if (sequenceId == RoatpWorkflowSequenceIds.YourOrganisation &&
                sectionId == RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails)
            {
                await RemoveIrrelevantQuestions(applicationId, section);
            }

            if (nextPageId == null || section.QnAData.Pages.FirstOrDefault(x => x.PageId == nextPageId) == null)
                return await TaskList(applicationId);

            return RedirectToAction("Page", new
            {
                applicationId,
                sequenceId ,
                sectionId,
                pageId = nextPageId,
                redirectAction
            });
        }

        [HttpPost]
        public async Task<IActionResult> SaveAnswers(PageViewModel vm, Guid applicationId)
        {
            vm.ApplicationId = applicationId; // why is this being assigned??? TODO: Fix in View so it's part of the ViewModel
            var sequenceId = int.Parse(vm.SequenceId); // TODO: SequenceId should be an int, not a string
            var sectionId = vm.SectionId;
            var pageId = vm.PageId;
            var redirectAction = vm.RedirectAction;


            var canUpdate = await CanUpdateApplication(applicationId, sequenceId, sectionId, pageId);
            if (!canUpdate)
            {
                return RedirectToAction("TaskList", new { applicationId });
            }

            _pageNavigationTrackingService.AddPageToNavigationStack(pageId);

            var sequences = await _qnaApiClient.GetSequences(applicationId);
            var selectedSequence = sequences.Single(x => x.SequenceId == sequenceId);
            var sections = await _qnaApiClient.GetSections(applicationId, selectedSequence.Id);
            var selectedSection = sections.Single(x => x.SectionId == sectionId);

            var sequence = await _qnaApiClient.GetSequence(applicationId, selectedSequence.Id);

            PageViewModel viewModel = null;
            var returnUrl = Request.Headers["Referer"].ToString();

            string pageContext = string.Empty;

            if (!ModelState.IsValid)
            {
                // when the model state has errors the page will be displayed with the values which failed validation
                var pageInvalid = JsonConvert.DeserializeObject<Page>((string)this.TempData["InvalidPage"]);

                var errorMessages = !ModelState.IsValid
                    ? ModelState.SelectMany(k => k.Value.Errors.Select(e => new ValidationErrorDetail()
                    {
                        ErrorMessage = e.ErrorMessage,
                        Field = k.Key
                    })).DistinctBy(f => f.Field).ToList()
                    : null;

                var peopleInControlDetails = await GetPeopleInControlDetails(applicationId, sequenceId, sectionId);

                viewModel = new PageViewModel(applicationId, sequenceId, sectionId, pageId, pageInvalid, pageContext, redirectAction,
                    returnUrl, errorMessages, _pageOverrideConfiguration, _qnaLinks, selectedSection.Title, peopleInControlDetails);


                viewModel = await TokeniseViewModelProperties(viewModel);

                if (viewModel.AllowMultipleAnswers)
                {
                    return View("~/Views/Application/Pages/MultipleAnswers.cshtml", viewModel);
                }

                return View("~/Views/Application/Pages/Index.cshtml", viewModel);
            }

                // when the model state has no errors the page will be displayed with the last valid values which were saved
                var page = await _qnaApiClient.GetPage(applicationId, selectedSection.Id, pageId);

                if (page == null)
                {
                    return RedirectToAction("TaskList", new { applicationId = applicationId });
                }

                if (IsFileUploadWithNonEmptyValue(page))
                {
                    var nextActionResult =
                    await _qnaApiClient.SkipPageBySectionNo(applicationId, sequenceId, sectionId, pageId);

                    if (nextActionResult?.NextAction == "NextPage")
                    {

                        return RedirectToAction("Page", new
                        {
                            applicationId,
                            sequenceId = selectedSequence.SequenceId,
                            sectionId = selectedSection.SectionId,
                            pageId = nextActionResult.NextActionId,
                            redirectAction
                        });
                    } 
                    else
                    {
                        return RedirectToAction("TaskList", new {applicationId = applicationId});
                    }
                }

                return await SaveAnswersGiven(applicationId, sequenceId, sectionId, pageId, redirectAction,
                    string.Empty);
        }

        [Route("apply-training-provider-tasklist")]
        [HttpGet]
        public async Task<IActionResult> TaskList(Guid applicationId)
        {
            var canUpdate = await CanUpdateApplication(applicationId);
            if (!canUpdate)
            {
                return RedirectToAction("Applications");
            }

            var sequences = await _qnaApiClient.GetSequences(applicationId);

            PopulateAdditionalSequenceFields(sequences);

            var filteredSequences = sequences.Where(x => x.SequenceId != RoatpWorkflowSequenceIds.Preamble && x.SequenceId != RoatpWorkflowSequenceIds.ConditionsOfAcceptance).OrderBy(y => y.SequenceId);
            
            foreach (var sequence in filteredSequences)
            {
                var sections = await _qnaApiClient.GetSections(applicationId, sequence.Id);                
                sequence.Sections = sections.ToList();
            }

            var organisationDetails = await _apiClient.GetOrganisationByUserId(User.GetUserId());

            var preambleSequence = sequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.Preamble);
            var preambleSections = await _qnaApiClient.GetSections(applicationId, preambleSequence.Id);
            var preambleSection = preambleSections.FirstOrDefault();
            var verifiedCompaniesHouse = await _qnaApiClient.GetAnswer(applicationId, preambleSection.Id, RoatpWorkflowPageIds.Preamble, RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany);
            var companiesHouseManualEntry = await _qnaApiClient.GetAnswer(applicationId, preambleSection.Id, RoatpWorkflowPageIds.Preamble, RoatpPreambleQuestionIdConstants.CompaniesHouseManualEntryRequired);
            var verifiedCharityCommission = await _qnaApiClient.GetAnswer(applicationId, preambleSection.Id, RoatpWorkflowPageIds.Preamble, RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity);
            var charityCommissionManualEntry = await _qnaApiClient.GetAnswer(applicationId, preambleSection.Id, RoatpWorkflowPageIds.Preamble, RoatpPreambleQuestionIdConstants.CharityCommissionTrusteeManualEntry);

            var providerRoute = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.ProviderRoute);

            var populatedNotRequiredOverrides = await PopulateNotRequiredOverridesWithApplicationData(applicationId, _notRequiredOverrides);

            var yourOrganisationSequence =
                sequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);
            var yourOrganisationSections = await _qnaApiClient.GetSections(applicationId, yourOrganisationSequence.Id);
            var whosInControlSection = yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

            var companiesHouseDataConfirmed = await _qnaApiClient.GetAnswer(applicationId, whosInControlSection.Id, RoatpWorkflowPageIds.WhosInControl.CompaniesHouseStartPage, RoatpYourOrganisationQuestionIdConstants.CompaniesHouseDetailsConfirmed);
            var charityCommissionDataConfirmed = await _qnaApiClient.GetAnswer(applicationId, whosInControlSection.Id, RoatpWorkflowPageIds.WhosInControl.CharityCommissionStartPage, RoatpYourOrganisationQuestionIdConstants.CharityCommissionDetailsConfirmed);

            var whosInControlConfirmed = false;

            var soleTraderDateOfBirthAnswer = await _qnaApiClient.GetAnswer(applicationId, whosInControlSection.Id, RoatpWorkflowPageIds.WhosInControl.AddSoleTraderDob, RoatpYourOrganisationQuestionIdConstants.AddSoleTradeDob);
            if (soleTraderDateOfBirthAnswer != null && !String.IsNullOrEmpty(soleTraderDateOfBirthAnswer.Value))
            {
                whosInControlConfirmed = true;
            }
            var partnersDetailsAnswer = await _qnaApiClient.GetAnswer(applicationId, whosInControlSection.Id, RoatpWorkflowPageIds.WhosInControl.AddPartners, RoatpYourOrganisationQuestionIdConstants.AddPartners);
            if (partnersDetailsAnswer != null && !String.IsNullOrEmpty(partnersDetailsAnswer.Value))
            {
                whosInControlConfirmed = true;
            }
            var pscsDetailsAnswer = await _qnaApiClient.GetAnswer(applicationId, whosInControlSection.Id, RoatpWorkflowPageIds.WhosInControl.AddPeopleInControl, RoatpYourOrganisationQuestionIdConstants.AddPeopleInControl);
            if (pscsDetailsAnswer != null && !String.IsNullOrEmpty(pscsDetailsAnswer.Value))
            {
                whosInControlConfirmed = true;
            }

            var model = new TaskListViewModel(_qnaApiClient)
            {
                ApplicationId = applicationId,
                ApplicationSequences = filteredSequences,
                NotRequiredOverrides = populatedNotRequiredOverrides,
                UKPRN = organisationDetails.OrganisationUkprn?.ToString(),
                OrganisationName = organisationDetails.Name,
                TradingName = organisationDetails.OrganisationDetails?.TradingName,
                VerifiedCompaniesHouse = (verifiedCompaniesHouse.Value == "TRUE"),
                VerifiedCharityCommission = (verifiedCharityCommission.Value == "TRUE"),
                CompaniesHouseManualEntry = (companiesHouseManualEntry.Value == "TRUE"),
                CharityCommissionManualEntry = (charityCommissionManualEntry.Value == "TRUE"),
                CompaniesHouseDataConfirmed = (companiesHouseDataConfirmed != null && companiesHouseDataConfirmed.Value == "Y"),
                CharityCommissionDataConfirmed = (charityCommissionDataConfirmed != null && charityCommissionDataConfirmed.Value == "TRUE"),
                WhosInControlConfirmed = whosInControlConfirmed,
                ApplicationRouteId = providerRoute.Value
            };

            return View("~/Views/Roatp/TaskList.cshtml", model);
        }

        private async Task<List<NotRequiredOverrideConfiguration>> PopulateNotRequiredOverridesWithApplicationData(Guid applicationId, List<NotRequiredOverrideConfiguration> notRequiredOverrides)
        {
            var applicationData = await _qnaApiClient.GetApplicationData(applicationId) as JObject;

            if (applicationData == null) 
            { 
                return notRequiredOverrides; 
            }
            
            foreach (var overrideConfig in notRequiredOverrides)
            {
                foreach (var condition in overrideConfig.Conditions)
                {
                    var applicationDataValue = applicationData[condition.ConditionalCheckField];
                    condition.Value = applicationDataValue != null ? applicationDataValue.Value<string>() : string.Empty;
                }
            }

            return notRequiredOverrides;
        }

        private async Task RemoveIrrelevantQuestions(Guid applicationId, ApplicationSection section)
        {
            const int DefaultSectionId = 1;
            var sequences = await _qnaApiClient.GetSequences(applicationId);
            var preambleSequence = sequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.Preamble);
            var preambleSections = await _qnaApiClient.GetSections(applicationId, preambleSequence.Id);
            var preambleSection = preambleSections.FirstOrDefault(x => x.SectionId == DefaultSectionId);
            var isCompanyAnswer = await _qnaApiClient.GetAnswer(applicationId, preambleSection.Id, RoatpWorkflowPageIds.Preamble, RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany);
            if (isCompanyAnswer?.Value == null || isCompanyAnswer.Value.ToUpper() != "TRUE")
            {
                if (section != null)
                {
                    var parentCompanyPages = section.QnAData.Pages.Where(x => x.PageId == RoatpWorkflowPageIds.YourOrganisationParentCompanyCheck
                                                                                              || x.PageId == RoatpWorkflowPageIds.YourOrganisationParentCompanyDetails).ToList();
                    foreach (var page in parentCompanyPages)
                    {
                        section.QnAData.Pages.Remove(page);
                    }
                }
            }
        }
        
        private void PopulateAdditionalSequenceFields(IEnumerable<ApplicationSequence> sequences)
        {
            foreach (var sequence in sequences)
            {
                var selectedSequence = _configuration.FirstOrDefault(x => x.Id == sequence.SequenceId);
                if (selectedSequence != null)
                {
                    sequence.Description = selectedSequence.Title;
                    sequence.Sequential = selectedSequence.Sequential;
                }
            }
        }

        private async Task<bool> CanUpdateApplication(Guid applicationId, int? sequenceId = null, int? sectionId = null, string pageId = null)
        {
            bool canUpdate = false;

            var applyingUser = await _usersApiClient.GetUserBySignInId((await _userService.GetSignInId()).ToString());
            var userId = applyingUser?.Id ?? Guid.Empty;

            var applications = await _apiClient.GetApplications(userId, false);            
            var application = applications?.FirstOrDefault(app => app.ApplicationId == applicationId);

            var validApplicationStatuses = new string[] { ApplicationStatus.InProgress, ApplicationStatus.FeedbackAdded };

            if (application != null && application.ApplyData != null && validApplicationStatuses.Contains(application.ApplicationStatus))
            {
                if (sequenceId.HasValue)
                {
                    var sequence = application.ApplyData.Sequences?.FirstOrDefault(seq => /*!seq.NotRequired && seq.IsActive &&*/ seq.SequenceNo == sequenceId);

                    if (sequence != null)
                    {
                        if (sectionId.HasValue)
                        {
                            var section = sequence.Sections.FirstOrDefault(sec => /*!sec.NotRequired && */ sec.SectionNo == sectionId);

                            if (section != null)
                            {
                                if (!string.IsNullOrWhiteSpace(pageId))
                                {
                                    var page = await _qnaApiClient.GetPage(applicationId, section.SectionId, pageId);
                                    if (page != null && page.Active)
                                    {
                                        canUpdate = true;
                                    }
                                }
                                else
                                {
                                    // No need to check the page
                                    canUpdate = true;
                                }
                            }
                        }
                        else
                        {
                            // No need to check the section
                            canUpdate = true;
                        }
                    }
                }
                else
                {
                    // No need to check the sequence
                    canUpdate = true;
                }
            }

            return canUpdate;
        }

        private async Task<Page> GetDataFedOptions(Guid applicationId, Page page)
        {
            if (page != null)
            {
                foreach (var question in page.Questions)
                {
                    if (question.Input.Type.StartsWith("DataFed_"))
                    {
                        var questionOptions = await _apiClient.GetQuestionDataFedOptions(question.Input.DataEndpoint);
                        // Get data from API using question.Input.DataEndpoint
                        question.Input.Options = questionOptions;
                        question.Input.Type = question.Input.Type.Replace("DataFed_", "");
                    }
                    if (question.Input.Type == "TabularData")
                    {
                        var answer = await _qnaApiClient.GetAnswerByTag(applicationId, question.QuestionTag, question.QuestionId);
                        if (page.PageOfAnswers == null || page.PageOfAnswers.Count < 1)
                        {
                            page.PageOfAnswers = new List<PageOfAnswers>();

                            var pageOfAnswers = new PageOfAnswers { Id = Guid.NewGuid(), Answers = new List<Answer>() };
                            page.PageOfAnswers.Add(pageOfAnswers);
                        }
                        var autoFilledAnswer = new Answer { QuestionId = question.QuestionId, Value = answer.Value };
                        var answersCollection = page.PageOfAnswers.First().Answers;
                        if (answersCollection.FirstOrDefault(x => x.QuestionId == question.QuestionId) == null)
                        {
                            answersCollection.Add(autoFilledAnswer);
                        }
                    }
                }
                
            }

            return page;
        }
        
        private async Task<IActionResult> SaveAnswersGiven(Guid applicationId, int sequenceId, int sectionId, string pageId, string redirectAction, string __formAction)
        {
            var sequences = await _qnaApiClient.GetSequences(applicationId);
            var selectedSequence = sequences.Single(x => x.SequenceId == sequenceId);
            var sections = await _qnaApiClient.GetSections(applicationId, selectedSequence.Id);
            var selectedSection = sections.Single(x => x.SectionId == sectionId);

            var page = await _qnaApiClient.GetPage(applicationId, selectedSection.Id, pageId);

            var errorMessages = new List<ValidationErrorDetail>();
            var answers = new List<Answer>();

            answers.AddRange(GetAnswersFromForm(page));

            // We need to back fill files as GetAnswersFromForm will place blank answers. This won't be a problem when we've fully moved over to the EPAO's way of saving answers
            foreach(var fileUploadAnswer in GetAnswersFromFiles())
            {
                var answer = answers.FirstOrDefault(a => a.QuestionId == fileUploadAnswer.QuestionId);

                if(answer != null)
                {
                    answer.Value = fileUploadAnswer.Value;
                }
                else
                {
                    answers.Add(fileUploadAnswer);
                }
            }

            ApplyFormattingToAnswers(answers, page);
            
            RunCustomValidations(page, answers);
            if(ModelState.IsValid == false)
            {
                page = await _qnaApiClient.GetPage(applicationId, selectedSection.Id, pageId);


                this.TempData["InvalidPage"] = JsonConvert.SerializeObject(page);
                return await Page(applicationId, sequenceId, sectionId, pageId, redirectAction,null);
            }

            //todo: Should we convert this to a custom validation?
            var checkBoxListQuestions = PageContainsCheckBoxListQuestions(page);
            if (checkBoxListQuestions.Any())
            {
                var checkBoxListQuestionId = CheckBoxListHasInvalidSelections(checkBoxListQuestions, answers);
                if (!String.IsNullOrWhiteSpace(checkBoxListQuestionId))
                {
                    ModelState.AddModelError(checkBoxListQuestionId, InvalidCheckBoxListSelectionErrorMessage);

                    //Can this be made common? What about DataFedOptions?
                    page = await _qnaApiClient.GetPage(applicationId, selectedSection.Id, pageId);
                    this.TempData["InvalidPage"] = JsonConvert.SerializeObject(page);
                    return await Page(applicationId, sequenceId, sectionId, pageId, redirectAction,null);
                }
            }

            var isFileUploadPage = page.Questions.Any(q => "FileUpload".Equals(q.Input.Type, StringComparison.InvariantCultureIgnoreCase));

            bool validationPassed;
            List<KeyValuePair<string, string>> validationErrors;
            string nextAction;
            string nextActionId;

            if (isFileUploadPage)
            {
                var uploadFileResult = await _qnaApiClient.Upload(applicationId, selectedSection.Id, pageId, HttpContext.Request.Form.Files);
                validationPassed = uploadFileResult.ValidationPassed;
                validationErrors = uploadFileResult.ValidationErrors;
                nextAction = uploadFileResult.NextAction;
                nextActionId = uploadFileResult.NextActionId;
            }
            else
            {
                var updatePageResult = await _qnaApiClient.UpdatePageAnswers(applicationId, selectedSection.Id, pageId, answers);
                validationPassed = updatePageResult.ValidationPassed;
                validationErrors = updatePageResult.ValidationErrors;
                nextAction = updatePageResult.NextAction;
                nextActionId = updatePageResult.NextActionId;
            }

            if (validationPassed)
            {
                if (__formAction == "Add" && page.AllowMultipleAnswers)
                {
                    return RedirectToAction("Page", new {applicationId, sequenceId = selectedSequence.SequenceId,
                        sectionId = selectedSection.SectionId, pageId = nextActionId, redirectAction});
                }

                if (redirectAction == "Feedback")
                {
                    return RedirectToAction("Feedback", new {applicationId});
                }

                if ("ReturnToSection".Equals(nextAction, StringComparison.InvariantCultureIgnoreCase) && (page.DisplayType==SectionDisplayType.PagesWithSections || page.DisplayType =="OtherPagesInPagesWithSections"))
                {
                    return await Section(applicationId, selectedSequence.SequenceId,selectedSection.SectionId);
                }

                if (string.IsNullOrEmpty(nextActionId) || !"NextPage".Equals(nextAction, StringComparison.InvariantCultureIgnoreCase))
                {
                    return await TaskList(applicationId);
                }
        
                return RedirectToAction("Page", new {applicationId, sequenceId = selectedSequence.SequenceId,
                    sectionId = selectedSection.SectionId, pageId = nextActionId, redirectAction});                                   
            }

            if (validationErrors != null)
            {
                foreach (var error in validationErrors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                    var valid = ModelState.IsValid;
                }
            }

            page = await _qnaApiClient.GetPage(applicationId, selectedSection.Id, pageId);

            if (isFileUploadPage != true)
            {
                page = StoreEnteredAnswers(answers, page);
            }

            var invalidPage = await GetDataFedOptions(applicationId, page);
            this.TempData["InvalidPage"] = JsonConvert.SerializeObject(invalidPage);

            return await Page(applicationId, sequenceId, sectionId, pageId, redirectAction, page?.Questions);
        }

        private static Page StoreEnteredAnswers(List<Answer> answers, Page page)
        {
            if (answers != null && answers.Any())
            {
                if (page.PageOfAnswers is null || !page.PageOfAnswers.Any())
                {
                    page.PageOfAnswers = new List<PageOfAnswers> { new PageOfAnswers { Answers = new List<Answer>() } };
                }

                page.PageOfAnswers.Add(new PageOfAnswers { Answers = answers });
            }

            return page;
        }

        private static void ApplyFormattingToAnswers(List<Answer> answers, Page page)
        {
            foreach (var answer in answers)
            {
                var question = page.Questions.FirstOrDefault(x => x.QuestionId == answer.QuestionId);
                if (question != null && question.Input != null 
                                     && !String.IsNullOrWhiteSpace(question.Input.InputClasses)
                                     && question.Input.InputClasses.Contains(InputClassUpperCase))
                {
                    answer.Value = answer.Value.ToUpper();
                }
            }
        }

        private static IEnumerable<Question> PageContainsCheckBoxListQuestions(Page page)
        {
            return page.Questions.Where(q => q.Input.Type == "CheckBoxList" || q.Input.Type=="ComplexCheckBoxList");
        }

        private static string CheckBoxListHasInvalidSelections(IEnumerable<Question> checkBoxListQuestions, List<Answer> answers)
        {
            foreach (var question in checkBoxListQuestions)
            {
                var checkBoxListAnswer = answers.FirstOrDefault(x => x.QuestionId == question.QuestionId);
                if (checkBoxListAnswer != null)
                {
                    if (checkBoxListAnswer.Value.Contains(NotApplicableAnswerText)
                        && checkBoxListAnswer.Value != NotApplicableAnswerText)
                    {
                        return checkBoxListAnswer.QuestionId;
                    }
                }
            }

            return null;
        }

        // TODO This needs isolating into a testable service
        private List<Answer> GetAnswersFromForm(Page page)
        {
            List<Answer> answers = new List<Answer>();

            var excludedInputs = new List<string> { "postcodeSearch", "checkAll", "ApplicationId", "RedirectAction" };

            // Add answers from the Form post
            foreach (var keyValuePair in HttpContext.Request.Form.Where(f => !f.Key.StartsWith("__") && !excludedInputs.Contains(f.Key, StringComparer.InvariantCultureIgnoreCase)))
            {
                answers.Add(new Answer() { QuestionId = keyValuePair.Key, Value = keyValuePair.Value });
            }

            // Check if any Page Question is missing and add the default answer
            foreach (var questionId in page.Questions.Select(q => q.QuestionId))
            {
                if (!answers.Any(a => a.QuestionId == questionId))
                {
                    // Add default answer if it's missing
                    answers.Add(new Answer { QuestionId = questionId, Value = string.Empty });
                }
            }

            #region FurtherQuestion_Processing
            // Get all questions that have FurtherQuestions in a ComplexRadio
            var questionsWithFutherQuestions = page.Questions.Where(x => (x.Input.Type == "ComplexRadio" || x.Input.Type == "ComplexCheckBoxList") && x.Input.Options != null && x.Input.Options.Any(o => o.FurtherQuestions != null && o.FurtherQuestions.Any()));

       
            foreach (var question in questionsWithFutherQuestions)
            {
                if (question.Input.Type == "ComplexRadio")
                {
                    var answerForQuestion = answers.FirstOrDefault(a => a.QuestionId == question.QuestionId);


                    // Remove FurtherQuestion answers to all other Options as they were not selected and thus should not be stored
                    foreach (var furtherQuestion in question.Input.Options
                        .Where(opt => opt.Value != answerForQuestion?.Value && opt.FurtherQuestions != null)
                        .SelectMany(opt => opt.FurtherQuestions))
                    {
                        foreach (var answer in answers.Where(a => a.QuestionId == furtherQuestion.QuestionId))
                        {
                            answer.Value = string.Empty;
                        }
                    }
                }

                if (question.Input.Type == "ComplexCheckBoxList")
                {
                    var answerForQuestion = answers.FirstOrDefault(a => a.QuestionId == question.QuestionId);

                    if (answerForQuestion?.Value == null) continue;
                    
                    // This different funcationality required as the checkbox may and will return a comma delimited list of responses
                    var splitAnswers = answerForQuestion.Value.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    // Remove FurtherQuestion answers to all other Options as they were not selected and thus should not be stored
                    foreach (var option in question.Input.Options
                        .Where(opt => opt.FurtherQuestions != null))
                    {
                        foreach (var furtherQuestion in option.FurtherQuestions)
                            if (!splitAnswers.Contains(option.Value))
                            {
                                foreach (var answer in answers.Where(a => a.QuestionId == furtherQuestion.QuestionId))
                                {
                                    answer.Value = string.Empty;
                                }
                            }
                    }
                }
            }
            #endregion FurtherQuestion_Processing

            // Address inputs require special processing
            if (page.Questions.Any(x => x.Input.Type == "Address"))
            {
                answers = ProcessPageVmQuestionsForAddress(page, answers);
            }

            return answers;
        }

        private List<Answer> GetAnswersFromFiles()
        {
            List<Answer> answers = new List<Answer>();

            // Add answers from the Files sent within the Form post
            if (HttpContext.Request.Form.Files != null)
            {
                foreach (var file in HttpContext.Request.Form.Files)
                {
                    answers.Add(new Answer() { QuestionId = file.Name, Value = file.FileName });
                }

            }

            return answers;
        }

        private static List<Answer> ProcessPageVmQuestionsForAddress(Page page, List<Answer> answers)
        {

            if (page.Questions.Any(x => x.Input.Type == "Address"))
            {
                Dictionary<string, JObject> answerValueDictionary = new Dictionary<string, JObject>();

                // Address input fields will contain _Key_
                foreach (var formVariable in answers.Where(x => x.QuestionId.Contains("_Key_")))
                {
                    var answerKey = formVariable.QuestionId.Split("_Key_");
                    if (!answerValueDictionary.ContainsKey(answerKey[0]))
                    {
                        answerValueDictionary.Add(answerKey[0], new JObject());
                    }

                    answerValueDictionary[answerKey[0]].Add(
                        answerKey.Count() == 1 ? string.Empty : answerKey[1],
                        formVariable.Value.ToString());
                }

                // Remove anything that contains _Key_ as it has now been processed correctly
                answers = answers.Where(x => !x.QuestionId.Contains("_Key_")).ToList();

                foreach (var answerValue in answerValueDictionary)
                {
                    if (answerValue.Value.Count > 1)
                    {
                        var answer = answers.FirstOrDefault(a => a.QuestionId == answerValue.Key);

                        if (answer is null)
                        {
                            answers.Add(new Answer() { QuestionId = answerValue.Key, Value = answerValue.Value.ToString() });
                        }
                        else
                        {
                            answer.Value = answerValue.Value.ToString();
                        }
                    }
                }

            }

            return answers;
        }

        private bool FileValidationPassed(List<Answer> answers, Page page, List<ValidationErrorDetail> errorMessages)
        {
            var fileValidationPassed = true;
            if (!HttpContext.Request.Form.Files.Any()) return true;

            foreach (var file in HttpContext.Request.Form.Files)
            {
                
                var typeValidation = page.Questions.First(q => q.QuestionId == file.Name).Input.Validations.FirstOrDefault(v => v.Name == "FileType");
                if (typeValidation != null)
                {
                    var extension = typeValidation.Value.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries)[0];
                    var mimeType = typeValidation.Value.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries)[1];

                    if (file.FileName.Substring(file.FileName.IndexOf(".") + 1, (file.FileName.Length - 1) - file.FileName.IndexOf(".")).ToLower() != extension || file.ContentType.ToLower() != mimeType)
                    {
                        ModelState.AddModelError(file.Name, typeValidation.ErrorMessage);
                        errorMessages.Add(new ValidationErrorDetail(file.Name, typeValidation.ErrorMessage));
                        fileValidationPassed = false;
                    }
                    else
                    {
                        // Only add to answers if type validation passes.
                        answers.Add(new Answer() {QuestionId = file.Name, Value = file.FileName});
                    }
                }
                else
                {
                    // Only add to answers if type validation passes.
                    answers.Add(new Answer() {QuestionId = file.Name, Value = file.FileName});
                }
            }

            return fileValidationPassed;
        }

        [HttpPost]
        public async Task<IActionResult> Submit(Guid applicationId)
        {
            var canUpdate = await CanUpdateApplication(applicationId);
            if (!canUpdate)
            {
                return RedirectToAction("TaskList", new { applicationId });
            }

            var activeSequence = await _apiClient.GetSequence(applicationId, User.GetUserId());
            var errors = ValidateSubmit(activeSequence);
            if (errors.Any())
            {
                var sequenceVm = new SequenceViewModel(activeSequence, applicationId, errors);

                if (activeSequence.Status == ApplicationSequenceStatus.FeedbackAdded)
                {
                    return View("~/Views/Application/Feedback.cshtml", sequenceVm);
                }
                else
                {
                    return View("~/Views/Application/Sequence.cshtml", sequenceVm);
                }
            }

            var organisationDetails = await _apiClient.GetOrganisationByUserId(User.GetUserId());
            var providerRoute = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.ProviderRoute);

            var submitApplicationRequest = new Application.Apply.Submit.SubmitApplicationRequest
            {
                ApplicationId = applicationId,
                ProviderRoute = Convert.ToInt32(providerRoute.Value),
                SubmittingContactId = User.GetUserId()
            };

            if (await _apiClient.SubmitApplication(submitApplicationRequest))
            {
                return RedirectToAction("Submitted", new { applicationId });
            }
            else
            {
                // unable to submit
                return RedirectToAction("NotSubmitted", new { applicationId });
            }
        }

        private List<ValidationErrorDetail> ValidateSubmit(ApplicationSequence sequence)
        {
            var validationErrors = new List<ValidationErrorDetail>();

            if (sequence?.Sections is null)
            {
                var validationError = new ValidationErrorDetail(string.Empty, $"Cannot submit empty sequence");
                validationErrors.Add(validationError);
            }
            else if (sequence.Sections.Where(sec => sec.PagesComplete != sec.PagesActive).Any())
            {
                foreach (var sectionQuestionsNotYetCompleted in sequence.Sections.Where(sec => sec.PagesComplete != sec.PagesActive))
                {
                    var validationError = new ValidationErrorDetail(sectionQuestionsNotYetCompleted.Id.ToString(), $"You need to complete the '{sectionQuestionsNotYetCompleted.LinkTitle}' section");
                    validationErrors.Add(validationError);
                }
            }
            else if(sequence.Sections.Where(sec => sec.QnAData.RequestedFeedbackAnswered is false || sec.QnAData.Pages.Any(p => !p.AllFeedbackIsCompleted)).Any())
            {
                foreach (var sectionFeedbackNotYetCompleted in sequence.Sections.Where(sec => sec.QnAData.RequestedFeedbackAnswered is false || sec.QnAData.Pages.Any(p => !p.AllFeedbackIsCompleted)))
                {
                    var validationError = new ValidationErrorDetail(sectionFeedbackNotYetCompleted.Id.ToString(), $"You need to complete the '{sectionFeedbackNotYetCompleted.LinkTitle}' section");
                    validationErrors.Add(validationError);
                }
            }

            return validationErrors;
        }

        [HttpGet]  
        public async Task<IActionResult> DeleteAnswer(Guid applicationId, int sequenceId, int sectionId, string pageId, Guid answerId, string redirectAction)
        {
            await _apiClient.DeleteAnswer(applicationId, sequenceId, sectionId, pageId, answerId, User.GetUserId());
            
            return RedirectToAction("Page", new {applicationId, sequenceId, sectionId, pageId, redirectAction});
        }

        [HttpGet]
        public async Task<IActionResult> Feedback(Guid applicationId)
        {
            var sequence = await _apiClient.GetSequence(applicationId, User.GetUserId());
            var sequenceVm = new SequenceViewModel(sequence, applicationId, null);
            return View("~/Views/Application/Feedback.cshtml", sequenceVm);
        }

        public async Task<IActionResult> Submitted(Guid applicationId)
        {
            var application = await _apiClient.GetApplication(applicationId);
            var config = await _configService.GetConfig();
            return View("~/Views/Application/Submitted.cshtml", new SubmittedViewModel
            {
                ReferenceNumber = application?.ApplyData?.ApplyDetails?.ReferenceNumber,
                FeedbackUrl = config.FeedbackUrl,
                //StandardName = application?.ApplicationData?.StandardName,
                //StandardReference = application?.ApplicationData?.StandardReference,
                //StandardLevel = application?.ApplicationData?.StandardLevel
            });
        }

        [HttpGet]
        public async Task<IActionResult> NotSubmitted(Guid applicationId)
        {
            var application = await _apiClient.GetApplication(applicationId);
            var config = await _configService.GetConfig();
            return View("~/Views/Application/NotSubmitted.cshtml", new SubmittedViewModel
            {
                ReferenceNumber = application?.ApplyData?.ApplyDetails?.ReferenceNumber,
                FeedbackUrl = config.FeedbackUrl,
                //StandardName = application?.ApplicationData?.StandardName
            });
        }

        private async Task UpdateQuestionsWithinQnA(Guid applicationId, List<PreambleAnswer> questions)
        {
            if (questions != null)
            {
                // Many answers for a particular page could have been added, so we need to group them up
                var pageAnswersDictionary = questions.GroupBy(q => q.PageId).ToDictionary(g => g.Key, g => g.ToList());

                // Now we can process each page
                foreach (var entry in pageAnswersDictionary)
                {
                    var answers = entry.Value; // Note: All answers have been grouped up

                    // Make sure we have answers within the page to update
                    if (answers != null && answers.Any())
                    {
                        var sectionNo = answers[0].SectionId;
                        var sequenceNo = answers[0].SequenceId;
                        var pageId = answers[0].PageId;

                        var section = await _qnaApiClient.GetSectionBySectionNo(applicationId, sequenceNo, sectionNo);

                        if (section != null)
                        {
                            await _qnaApiClient.UpdatePageAnswers(applicationId, section.Id, pageId, answers.ToList<Answer>());
                        }
                    }
                }
            }
        }

        [HttpGet]
        [Route("submit-application")]
        public async Task<IActionResult> SubmitApplication(Guid applicationId)
        {
            var model = new SubmitApplicationViewModel { ApplicationId = applicationId };

            var organisationName = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.UkrlpLegalName);
            model.OrganisationName = organisationName.Value;

            return View("~/Views/Roatp/SubmitApplication.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmSubmitApplication(SubmitApplicationViewModel model)
        {
            var canUpdate = await CanUpdateApplication(model.ApplicationId);
            if (!canUpdate)
            {
                return RedirectToAction("TaskList", new { applicationId = model.ApplicationId });
            }


            if (!ModelState.IsValid)
            {
                model.ErrorMessages = new List<ValidationErrorDetail>();

                foreach (var modelStateKey in ViewData.ModelState.Keys)
                {
                    var value = ViewData.ModelState[modelStateKey];
                    foreach (var error in value.Errors)
                    {
                        model.ErrorMessages.Add(new ValidationErrorDetail
                        {
                            Field = modelStateKey,
                            ErrorMessage = error.ErrorMessage
                        });
                    }
                }

                return View("~/Views/Roatp/SubmitApplication.cshtml", model);
            }

            // TODO: Validate all sections are completed (i.e all questions answered)
            // FUTURE WORK: Validate all sections have had requested feedback answered


            var organisationDetails = await _apiClient.GetOrganisationByUserId(User.GetUserId());
            var providerRoute = await _qnaApiClient.GetAnswerByTag(model.ApplicationId, RoatpWorkflowQuestionTags.ProviderRoute);

            var application = await _apiClient.GetApplication(model.ApplicationId);

            var roatpSequences = await _apiClient.GetRoatpSequences();

            foreach (var sequence in application.ApplyData.Sequences)
            {
                var applicationSequence = await _qnaApiClient.GetSequenceBySequenceNo(model.ApplicationId, sequence.SequenceNo);
                var sections = await _qnaApiClient.GetSections(model.ApplicationId, applicationSequence.Id);
                applicationSequence.Sections = sections.ToList();
                foreach(var section in sections)
                {
                    var applySection = sequence.Sections.FirstOrDefault(x => x.SectionNo == section.SectionId);
                    if (applySection != null)
                    {
                        applySection.NotRequired = await SectionNotRequired(applicationSequence, _notRequiredOverrides, section.SectionId, roatpSequences);
                    }
                }
                
                applicationSequence.Sections = sections.ToList();
                sequence.NotRequired = SequenceNotRequired(applicationSequence, _notRequiredOverrides);
            }
            var providerRoutes = _roatpApiClient.GetApplicationRoutes().GetAwaiter().GetResult();
            var selectedProviderRoute = providerRoutes.FirstOrDefault(p => p.Id.ToString() == providerRoute.Value);

            var submitApplicationRequest = new Application.Apply.Submit.SubmitApplicationRequest
            {
                ApplicationId = model.ApplicationId,
                ProviderRoute = Convert.ToInt32(providerRoute.Value),
                ProviderRouteName = selectedProviderRoute?.RouteName,   
                SubmittingContactId = User.GetUserId(),
                ApplyData = application.ApplyData    
            };

            var submitResult = await _apiClient.SubmitApplication(submitApplicationRequest);

            if (submitResult)
            {
                var userDetails = await _usersApiClient.GetUserBySignInId(User.GetSignInId());
                var applicationSubmitConfirmation = new ApplicationSubmitConfirmation
                {
                    ApplicantFullName = $"{userDetails.GivenNames} {userDetails.FamilyName}",
                    ApplicationRouteId = providerRoute.Value,
                    EmailAddress = User.GetEmail()
                };

                await _submitApplicationEmailService.SendGetHelpWithQuestionEmail(applicationSubmitConfirmation);
                return RedirectToAction("ApplicationSubmitted", new { model.ApplicationId });
            }
            else
            {
                return RedirectToAction("TaskList", new { model.ApplicationId });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ApplicationSubmitted(Guid applicationId)
        {
            var application = await _apiClient.GetApplication(applicationId);           
            var applicationData = application.ApplyData.ApplyDetails;

            var model = new ApplicationSummaryViewModel
            {
                ApplicationId = application.ApplicationId,
                UKPRN = applicationData.UKPRN,
                OrganisationName = applicationData.OrganisationName,
                TradingName = applicationData.TradingName,
                ApplicationRouteId = applicationData.ProviderRoute.ToString(),
                ApplicationReference = applicationData.ReferenceNumber
            };

            return View("~/Views/Roatp/ApplicationSubmitted.cshtml", model);
        }

        private async Task SavePreambleInformation(Guid applicationId, ApplicationDetails applicationDetails)
        {
            var preambleQuestions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(applicationDetails);
            await UpdateQuestionsWithinQnA(applicationId, preambleQuestions);
        }

        private async Task SaveCompaniesHouseInformation(Guid applicationId, ApplicationDetails applicationDetails)
        {
            var directorsPscsQuestions = RoatpPreambleQuestionBuilder.CreateCompaniesHouseWhosInControlQuestions(applicationDetails);
            await UpdateQuestionsWithinQnA(applicationId, directorsPscsQuestions);
        }

        private async Task SaveCharityCommissionInformation(Guid applicationId, ApplicationDetails applicationDetails)
        {
            var trusteesQuestions = RoatpPreambleQuestionBuilder.CreateCharityCommissionWhosInControlQuestions(applicationDetails);
            await UpdateQuestionsWithinQnA(applicationId, trusteesQuestions);
        }

        private async Task<PageViewModel> TokeniseViewModelProperties(PageViewModel viewModel)
        {
            viewModel.Title =
                await _questionPropertyTokeniser.GetTokenisedValue(viewModel.ApplicationId, viewModel.Title);

            viewModel.BodyText =
                await _questionPropertyTokeniser.GetTokenisedValue(viewModel.ApplicationId, viewModel.BodyText);

        foreach(var questionModel in viewModel.Questions)
            {
                questionModel.Hint = await _questionPropertyTokeniser.GetTokenisedValue(viewModel.ApplicationId, questionModel.Hint);
                questionModel.Label = await _questionPropertyTokeniser.GetTokenisedValue(viewModel.ApplicationId, questionModel.Label);
                questionModel.QuestionBodyText = await _questionPropertyTokeniser.GetTokenisedValue(viewModel.ApplicationId, questionModel.QuestionBodyText);
                questionModel.ShortLabel = await _questionPropertyTokeniser.GetTokenisedValue(viewModel.ApplicationId, questionModel.ShortLabel);
            }

            return viewModel;
        }

        private bool IsFileUploadWithNonEmptyValue(Page page)
        {
            if (page.PageOfAnswers == null || page.PageOfAnswers.Count == 0 ||  page.Questions == null || page.Questions.Count == 0 || page.Questions[0].Input.Type != "FileUpload")
                return false;

            var fileUploadAnswerValue = string.Empty;

            foreach (var question in page.Questions)
            {
                if (fileUploadAnswerValue==string.Empty)
                    fileUploadAnswerValue= page.PageOfAnswers[0].Answers.FirstOrDefault(x => x.QuestionId == question.QuestionId)?.Value;
            }
        
            return !string.IsNullOrEmpty(fileUploadAnswerValue);
        }

        private void RunCustomValidations(Page page, List<Answer> answers)
        {
            foreach (var answer in answers)
            {
                var customValidations = _customValidatorFactory.GetCustomValidationsForQuestion(page.PageId, answer.QuestionId, HttpContext.Request.Form.Files);

                foreach (var customValidation in customValidations)
                {
                    var result = customValidation.Validate();

                    if(result.IsValid == false)
                    {
                        ModelState.AddModelError(result.QuestionId, result.ErrorMessage);
                    }
                }
            }
        }

        private async Task<List<TabularData>> GetPeopleInControlDetails(Guid applicationId, int sequenceId, int sectionId)
        {
            var result = new List<TabularData>();

            if (sequenceId == RoatpWorkflowSequenceIds.CriminalComplianceChecks && sectionId == RoatpWorkflowSectionIds.CriminalComplianceChecks.CheckOnWhosInControl)
            {
                var personData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.AddPeopleInControl);
                if (personData != null && personData.DataRows != null && personData.DataRows.Count > 0)
                {
                    result.Add(personData);
                }

                var directorsData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.CompaniesHouseDirectors);
                if (directorsData != null && directorsData.DataRows != null && directorsData.DataRows.Count > 0)
                {
                    result.Add(directorsData);
                }

                var pscsData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.CompaniesHousePscs);
                if (pscsData != null && pscsData.DataRows != null && pscsData.DataRows.Count > 0)
                {
                    result.Add(pscsData);
                }

                var trusteesData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.CharityCommissionTrustees);
                if (trusteesData != null && trusteesData.DataRows != null && trusteesData.DataRows.Count > 0)
                {
                    result.Add(trusteesData);
                }
            }

            return result;
        }

        private static bool SequenceNotRequired(ApplicationSequence sequence, List<NotRequiredOverrideConfiguration> notRequiredOverrides)
        {
            var sectionCount = sequence.Sections.Count;
            var notRequiredCount = 0;
            var sequences = new List<ApplicationSequence>
            {
                sequence
            };

            foreach(var section in sequence.Sections)
            {
                if (RoatpTaskListWorkflowService.SectionStatus(sequences, notRequiredOverrides, sequence.SequenceId, section.SectionId) == TaskListSectionStatus.NotRequired) 
                {
                    notRequiredCount++;
                }
            }

            return (sectionCount == notRequiredCount);
        }

        private async Task<bool> SectionNotRequired(ApplicationSequence sequence, List<NotRequiredOverrideConfiguration> notRequiredOverrides, 
                                                    int sectionId, IEnumerable<RoatpSequences> roatpSequences)
        {
            var sequences = new List<ApplicationSequence>
            {
                sequence
            };

            if (RoatpTaskListWorkflowService.SectionStatus(sequences, notRequiredOverrides, sequence.SequenceId, sectionId) == TaskListSectionStatus.NotRequired)
            {
                return true;
            }

            var currentSequence = roatpSequences.FirstOrDefault(x => x.Id == sequence.SequenceId);
            if (currentSequence?.ExcludeSections != null && currentSequence.ExcludeSections.Contains(sectionId.ToString()))
            {
                return true;
            }

            return false;
        }
    }
}