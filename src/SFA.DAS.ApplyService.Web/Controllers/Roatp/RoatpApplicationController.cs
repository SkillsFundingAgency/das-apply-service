using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SFA.DAS.ApplyService.Application.Apply.GetAnswers;
using SFA.DAS.ApplyService.Application.Apply.GetPage;
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
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using MoreLinq;
    using SFA.DAS.ApplyService.Web.Infrastructure.Validations;
    using SFA.DAS.ApplyService.Web.Services;
    using ViewModels.Roatp;

    [Authorize]
    public class RoatpApplicationController : Controller
    {
        private readonly IApplicationApiClient _apiClient;
        private readonly ILogger<RoatpApplicationController> _logger;
        private readonly IUsersApiClient _usersApiClient;
        private readonly ISessionService _sessionService;
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
        private readonly IRoatpTaskListWorkflowService _roatpTaskListWorkflowService;

        private const string ApplicationDetailsKey = "Roatp_Application_Details";
        private const string InputClassUpperCase = "app-uppercase";
        private const int Section1Id = 1;
        private const string NotApplicableAnswerText = "None of the above";
        private const string InvalidCheckBoxListSelectionErrorMessage = "If your answer is 'none of the above', you must only select that option";

        public RoatpApplicationController(IApplicationApiClient apiClient, ILogger<RoatpApplicationController> logger,
            ISessionService sessionService, IConfigurationService configService, IUserService userService, IUsersApiClient usersApiClient,
            IQnaApiClient qnaApiClient, IOptions<List<TaskListConfiguration>> configuration, IProcessPageFlowService processPageFlowService,
            IQuestionPropertyTokeniser questionPropertyTokeniser, IOptions<List<QnaPageOverrideConfiguration>> pageOverrideConfiguration, 
            IPageNavigationTrackingService pageNavigationTrackingService, IOptions<List<QnaLinksConfiguration>> qnaLinks, ICustomValidatorFactory customValidatorFactory, IRoatpTaskListWorkflowService roatpTaskListWorkflowService, IOptions<List<NotRequiredOverrideConfiguration>> notRequiredOverrides)
        {
            _apiClient = apiClient;
            _logger = logger;
            _sessionService = sessionService;
            _configService = configService;
            _userService = userService;
            _usersApiClient = usersApiClient;
            _qnaApiClient = qnaApiClient;
            _processPageFlowService = processPageFlowService;
            _configuration = configuration.Value;
            _questionPropertyTokeniser = questionPropertyTokeniser;
            _pageNavigationTrackingService = pageNavigationTrackingService;
            _qnaLinks = qnaLinks.Value;
            _pageOverrideConfiguration = pageOverrideConfiguration.Value;
            _customValidatorFactory = customValidatorFactory;
            _roatpTaskListWorkflowService = roatpTaskListWorkflowService;
            _notRequiredOverrides = notRequiredOverrides.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Applications(string applicationType)
        {
            var user = User.Identity.Name;

            if (!await _userService.ValidateUser(user))
                return RedirectToAction("PostSignIn", "Users");

            _logger.LogInformation($"Got LoggedInUser from Session: {user}");

            var applyUser = await _usersApiClient.GetUserBySignInId((await _userService.GetSignInId()).ToString());
            var userId = applyUser?.Id ?? Guid.Empty;

            var org = await _apiClient.GetOrganisationByUserId(userId);
            var applications = await _apiClient.GetApplications(userId, false);
            applications = applications.Where(app => app.ApplicationStatus != ApplicationStatus.Rejected).ToList();

            if (!applications.Any())
            {              
                return await StartApplication(userId, applicationType);
            }
            
            if (applications.Count() > 1)
                return View(applications);

            var application = applications.First();
            
            switch (application.ApplicationStatus)
            {
                case ApplicationStatus.FeedbackAdded:
                    return View("~/Views/Application/FeedbackIntro.cshtml", application.Id);
                case ApplicationStatus.Rejected:
                case ApplicationStatus.Approved:
                    return View(applications);
                default:
                    return RedirectToAction("TaskList", new {applicationId = application.Id});
            }
        }

        private async Task<IActionResult> StartApplication(Guid userId, string applicationType)
        {
            var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);
            
            var startApplicationData = new JObject
            {
                ["OrganisationReferenceId"] = applicationDetails.UKPRN.ToString(),
                ["OrganisationName"] = applicationDetails.UkrlpLookupDetails.ProviderName,
                ["Apply-ProviderRoute"] = applicationDetails.ApplicationRoute.Id.ToString()
            };

            var startApplicationJson = JsonConvert.SerializeObject(startApplicationData);

            var applicationStartResponse =
                await _qnaApiClient.StartApplication(userId.ToString(), applicationType, startApplicationJson);
            
            var response = await _apiClient.StartApplication(applicationStartResponse.ApplicationId, userId, applicationType);
            
            var preambleQuestions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(applicationDetails);

            await SavePreambleQuestions(response.ApplicationId, userId, preambleQuestions);

            if (applicationDetails.UkrlpLookupDetails.VerifiedByCompaniesHouse)
            {
                var directorsPscsQuestions = RoatpPreambleQuestionBuilder.CreateCompaniesHouseWhosInControlQuestions(applicationDetails);

                await SaveCompaniesHouseWhosInControlQuestions(response.ApplicationId, userId, directorsPscsQuestions);
            }
            
            if (applicationDetails.UkrlpLookupDetails.VerifiedbyCharityCommission)
            {
                var trusteesQuestions = RoatpPreambleQuestionBuilder.CreateCharityCommissionWhosInControlQuestions(applicationDetails);

                await SaveCharityCommissionWhosInControlQuestions(response.ApplicationId, userId, trusteesQuestions);
            }

            return RedirectToAction("Applications", new { applicationType = applicationType });
        }

        [HttpGet]
        public async Task<IActionResult> StartApplication(string applicationType)
        {
            var response = await _apiClient.StartApplication(await _userService.GetUserId(), applicationType);
            
            return RedirectToAction("TaskList", new { applicationId = response.ApplicationId });            
        }

        [HttpGet]
        public async Task<IActionResult> Sequence(Guid applicationId)
        {
            // Break this out into a "Signpost" action.
            var sequence = await _apiClient.GetSequence(applicationId, User.GetUserId());
            var sequenceVm = new SequenceViewModel(sequence, applicationId, null);
            return View(sequenceVm);
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
                return View("~/Views/Application/FeedbackIntro.cshtml", application.Id);
            }

            var sequence = await _apiClient.GetSequence(applicationId, User.GetUserId());

            StandardApplicationData applicationData = null;

            if (application.ApplicationData != null)
            {
                applicationData = new StandardApplicationData
                {
                    StandardName = application.ApplicationData.StandardName
                };
            }

            // Only go to search if application hasn't got a selected standard?
            if (sequence.SequenceId == SequenceId.Stage1)
            {
                return RedirectToAction("Sequence", new {applicationId});
            }
            else if (sequence.SequenceId == SequenceId.Stage2 && string.IsNullOrWhiteSpace(applicationData?.StandardName))
            {
                var org = await _apiClient.GetOrganisationByUserId(User.GetUserId());
                if (org.RoEPAOApproved)
                {
                    return RedirectToAction("Index", "Standard", new {applicationId});  
                }

                return View("~/Views/Application/Stage2Intro.cshtml", applicationId);
            }
            else if (sequence.SequenceId == SequenceId.Stage2)
            {
                return RedirectToAction("Sequence", new {applicationId});
            }

            throw new BadRequestException("Section does not have a valid DisplayType");
        }

        [HttpGet]
        public async Task<IActionResult> Section(Guid applicationId, int sequenceId, int sectionId)
        {
            var sequences = await _qnaApiClient.GetSequences(applicationId);
            var selectedSequence = sequences.Single(x => x.SequenceId == sequenceId);
            var sections = await _qnaApiClient.GetSections(applicationId, selectedSequence.Id);
            var selectedSection = sections.Single(x => x.SectionId == sectionId);

            var section = await _qnaApiClient.GetSection(applicationId, selectedSection.Id);

            if (sequenceId == RoatpWorkflowSequenceIds.YourOrganisation && sectionId == RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails)
            {
                await RemoveIrrelevantQuestions(applicationId, section);
            }

            var pageId = section.QnAData.Pages.FirstOrDefault()?.PageId;

            return await Page(applicationId, sequenceId, sectionId, pageId, "TaskList",null);
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
        public async Task<IActionResult> Page(Guid applicationId, int sequenceId, int sectionId, string pageId, string redirectAction, List<Question> answeredQuestions)
        {
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
                var page = JsonConvert.DeserializeObject<Page>((string) this.TempData["InvalidPage"]);

                var errorMessages = !ModelState.IsValid
                    ? ModelState.SelectMany(k => k.Value.Errors.Select(e => new ValidationErrorDetail()
                    {
                        ErrorMessage = e.ErrorMessage,
                        Field = k.Key
                    })).DistinctBy(f => f.Field).ToList()
                    : null;

                viewModel = new PageViewModel(applicationId, sequenceId, sectionId, pageId, page, pageContext, redirectAction,
                    returnUrl, errorMessages, _pageOverrideConfiguration, _qnaLinks);
            }
            else
            {
                // when the model state has no errors the page will be displayed with the last valid values which were saved
                var page = await _qnaApiClient.GetPage(applicationId, selectedSection.Id, pageId);
                
                if (page == null)
                {
                    return RedirectToAction("TaskList", new {applicationId = applicationId});
                }

                var section = await _qnaApiClient.GetSection(applicationId, selectedSection.Id);

                page = await _qnaApiClient.GetPage(applicationId, selectedSection.Id, pageId);
                if (page == null || page.Questions == null)
                {
                    var pageInSectionId = section.QnAData.Pages.FirstOrDefault(x => x.PageId == pageId);
                    if (pageInSectionId == null)
                    {
                        await _apiClient.MarkSectionAsCompleted(applicationId, selectedSection.Id);
                    }
                    return await TaskList(applicationId);
                }

                page = await GetDataFedOptions(applicationId, page);

                viewModel = new PageViewModel(applicationId, sequenceId, sectionId, pageId, page, pageContext, redirectAction,
                    returnUrl, null, _pageOverrideConfiguration, _qnaLinks);

            }

            viewModel = await TokeniseViewModelProperties(viewModel);

            if (viewModel.AllowMultipleAnswers)
            {
                return View("~/Views/Application/Pages/MultipleAnswers.cshtml", viewModel);
            }

            return View("~/Views/Application/Pages/Index.cshtml", viewModel);

            
        }
        
                
        [HttpGet]
        public async Task<IActionResult> Skip(Guid applicationId, int sequenceId, int sectionId, string pageId, string redirectAction)
        {
            var sequences = await _qnaApiClient.GetSequences(applicationId);
            var selectedSequence = sequences.Single(x => x.SequenceId == sequenceId);
            var sections = await _qnaApiClient.GetSections(applicationId, selectedSequence.Id);

            var currentSection = sections.Single(x => x.SectionId == sectionId);

            var section = await _qnaApiClient.GetSection(applicationId, currentSection.Id);

            if (sequenceId == RoatpWorkflowSequenceIds.YourOrganisation &&
                sectionId == RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails)
            {
                await RemoveIrrelevantQuestions(applicationId, section);
            }

            var currentPage = section.QnAData.Pages.First(x => x.PageId == pageId);
            var nextPageId = currentPage.Next.FirstOrDefault(x => x.Conditions == null || x.Conditions.Count==0)?.ReturnId;

            if (string.IsNullOrEmpty(nextPageId))
            {
                nextPageId = CalculateNextPageId(currentPage);
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

        private string CalculateNextPageId(Page currentPage)
        {
            var conditionalSet = currentPage.Next.Where(x => x.Conditions?.Count > 0);
            foreach (var conditionals in conditionalSet.Where(c => c.Action == "NextPage"))
            {
                foreach (var condition in conditionals.Conditions)
                {
                    foreach (var answerSet in currentPage.PageOfAnswers)
                    {
                        foreach (var answer in answerSet.Answers)
                        {
                            if (answer.QuestionId == condition.QuestionId && answer.Value == condition.MustEqual)
                               return conditionals.ReturnId;
                        }
                    }
                }
            }

            return string.Empty;
        }
        [HttpPost]
        public async Task<IActionResult> SaveAnswers(PageViewModel vm, Guid applicationId)
        {
            vm.ApplicationId = applicationId;
            var sequenceId = int.Parse(vm.SequenceId);
            var sectionId = vm.SectionId;
            var pageId = vm.PageId;
            var redirectAction = vm.RedirectAction;
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

                viewModel = new PageViewModel(applicationId, sequenceId, sectionId, pageId, pageInvalid, pageContext, redirectAction,
                    returnUrl, errorMessages, _pageOverrideConfiguration, _qnaLinks);


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

                var section = await _qnaApiClient.GetSection(applicationId, selectedSection.Id);

                if (IsFileUploadWithNonEmptyValue(page))
                {
                    var nextActionResult =
                        await _qnaApiClient.GetNextActionBySectionNo(applicationId, sequenceId, sectionId, pageId);

                    if (nextActionResult != null && nextActionResult.NextAction == "NextPage")
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
                }

                return await SaveAnswersGiven(applicationId, sequenceId, sectionId, pageId, redirectAction,
                    string.Empty);
        }

        [Route("apply-training-provider-tasklist")]
        [HttpGet]
        public async Task<IActionResult> TaskList(Guid applicationId)
        {
            var sequences = await _qnaApiClient.GetSequences(applicationId);

            PopulateSequenceDescriptions(sequences);

            var filteredSequences = sequences.Where(x => x.SequenceId != RoatpWorkflowSequenceIds.Preamble && x.SequenceId != RoatpWorkflowSequenceIds.ConditionsOfAcceptance).OrderBy(y => y.SequenceId);
            
            foreach (var sequence in filteredSequences)
            {
                var sections = await _qnaApiClient.GetSections(applicationId, sequence.Id);
                foreach(var section in sections)
                {
                    section.SectionCompleted = await _apiClient.IsSectionCompleted(applicationId, section.Id);
                }
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
            var providerRoute = await _qnaApiClient.GetAnswerByTag(applicationId, "Apply-ProviderRoute");

            var model = new TaskListViewModel(_roatpTaskListWorkflowService)
            {
                ApplicationId = applicationId,
                ApplicationSequences = filteredSequences,
                NotRequiredOverrides = _notRequiredOverrides,
                UKPRN = organisationDetails.OrganisationUkprn?.ToString(),
                OrganisationName = organisationDetails.Name,
                TradingName = organisationDetails.OrganisationDetails?.TradingName,
                VerifiedCompaniesHouse = (verifiedCompaniesHouse.Value == "TRUE"),
                VerifiedCharityCommission = (verifiedCharityCommission.Value == "TRUE"),
                CompaniesHouseManualEntry = (companiesHouseManualEntry.Value == "TRUE"),
                CharityCommissionManualEntry = (charityCommissionManualEntry.Value == "TRUE"),
                ApplicationRouteId = providerRoute.Value
            };

            return View("~/Views/Roatp/TaskList.cshtml", model);
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
        
        private void PopulateSequenceDescriptions(IEnumerable<ApplicationSequence> sequences)
        {
            foreach (var sequence in sequences)
            {
                var sequenceDescription = _configuration.FirstOrDefault(x => x.Id == sequence.SequenceId);
                if (sequenceDescription != null)
                {
                    sequence.Description = sequenceDescription.Title;
                }
            }
        }

        private async Task<bool> CanUpdateApplication(Guid applicationId, int sequenceId)
        {
            var sequence = await _apiClient.GetSequence(applicationId, User.GetUserId());
            return CanUpdateApplication(sequence, sequenceId);
        }

        private bool CanUpdateApplication(ApplicationSequence sequence, int sequenceId)
        {
            bool canUpdate = false;

            if (sequence?.Status != null && (int)sequence.SequenceId == sequenceId)
            {
                canUpdate = sequence.Status == ApplicationSequenceStatus.Draft || sequence.Status == ApplicationSequenceStatus.FeedbackAdded;
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
                        var answer = await _qnaApiClient.GetAnswerByTag(applicationId, question.QuestionTag);
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
        
        private async Task<bool> CheckIfAnswersMustBeValidated(Guid applicationId, int sequenceId, int sectionId,
            string pageId, List<Answer> answers, Regex inputEnteredRegex)
        {
            if(answers.Exists(p => p.HasValue(inputEnteredRegex) && p.QuestionId != "RedirectAction"))
            {
                return true;
            }
            
            var sequences = await _qnaApiClient.GetSequences(applicationId);
            var selectedSequence = sequences.Single(x => x.SequenceId == sequenceId);
            var sections = await _qnaApiClient.GetSections(applicationId, selectedSequence.Id);
            var selectedSection = sections.Single(x => x.SectionId == sectionId);

            var page = await _qnaApiClient.GetPage(applicationId, selectedSection.Id, pageId);
            return (page == null || !page.PageOfAnswers.Any());
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

            GetAnswersFromForm(page, answers);
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

            //todo: do we need these two lines???
            var answersMustBeValidated = await CheckIfAnswersMustBeValidated(applicationId, sequenceId, sectionId, pageId, answers, new Regex(@"\w+"));
            var saveNewAnswers = (__formAction == "Add" || answersMustBeValidated);

            var updatePageResult = await _qnaApiClient.UpdatePageAnswers(applicationId, selectedSection.Id, pageId, answers); 

            if (updatePageResult.ValidationPassed)
            {
                await UploadFilesToStorage(applicationId, selectedSection.Id,pageId);

                if (__formAction == "Add" && page.AllowMultipleAnswers)
                {
                    return RedirectToAction("Page", new {applicationId, sequenceId = selectedSequence.SequenceId,
                        sectionId = selectedSection.SectionId, pageId = updatePageResult.NextActionId, redirectAction});
                }

                if (redirectAction == "Feedback")
                {
                    return RedirectToAction("Feedback", new {applicationId});
                }

                var nextActionsPage = updatePageResult.NextActionId;
                if (nextActionsPage == null)
                {
                    await _apiClient.MarkSectionAsCompleted(applicationId, selectedSection.Id);
                    return await TaskList(applicationId);
                }
        
                return RedirectToAction("Page", new {applicationId, sequenceId = selectedSequence.SequenceId,
                    sectionId = selectedSection.SectionId, pageId = updatePageResult.NextActionId, redirectAction});
                                   
            }

            if (updatePageResult.ValidationErrors != null)
            {
                foreach (var error in updatePageResult.ValidationErrors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                    var valid = ModelState.IsValid;
                }
            }

            page = await _qnaApiClient.GetPage(applicationId, selectedSection.Id, pageId);
            PreserveAnswersInPage(page, answers);
            var invalidPage = await GetDataFedOptions(applicationId, page);
            this.TempData["InvalidPage"] = JsonConvert.SerializeObject(invalidPage);

            return await Page(applicationId, sequenceId, sectionId, pageId, redirectAction, page?.Questions);
        }

        private void PreserveAnswersInPage(Page page, List<Answer> answers)
        {
            foreach (var question in page.Questions)
            {
                foreach (var answer in answers)
                {
                    if (question.QuestionId == answer.QuestionId)
                    {
                        question.Value = answer.Value;
                    }
                }
            }
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
            return page.Questions.Where(q => q.Input.Type == "CheckBoxList");
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

        private async Task UploadFilesToStorage(Guid applicationId, Guid sectionId, string pageId)
        {
            if (HttpContext.Request.Form.Files.Any())
            {
               await _qnaApiClient.Upload(applicationId, sectionId, pageId, HttpContext.Request.Form.Files);
            }
        }

        private void GetAnswersFromForm(Page page, List<Answer> answers)
        {
            Dictionary<string, JObject> answerValues = new Dictionary<string, JObject>();

            foreach (var formVariable in HttpContext.Request.Form.Where(f => !f.Key.StartsWith("__")))
            {
                var answerKey = formVariable.Key.Split("_Key_");
                if (!answerValues.ContainsKey(answerKey[0]))
                {
                    answerValues.Add(answerKey[0], new JObject());
                }

                answerValues[answerKey[0]].Add(
                    answerKey.Count() == 1 ? string.Empty : answerKey[1],
                    formVariable.Value.ToString());
            }

            foreach(var file in HttpContext.Request.Form.Files)
            {
                if (!answerValues.ContainsKey(file.Name))
                {
                    answerValues.Add(file.Name, new JObject());
                }

                answerValues[file.Name].Add(string.Empty, file.FileName);
            }

            foreach (var answer in answerValues)
            {
                if(answer.Value.Count > 1)
                {
                    answers.Add(new Answer() { QuestionId = answer.Key, JsonValue = answer.Value });
                }
                else
                {
                    answers.Add(new Answer() { QuestionId = answer.Key, Value = answer.Value.Value<string>(string.Empty) });
                }
            }
            var unansweredQuestions = page.Questions.Where(x => !answers.Any(y => y.QuestionId == x.QuestionId));
            foreach(var question in unansweredQuestions)
            {
                answers.Add(new Answer() { QuestionId = question.QuestionId, Value = "" });
            }
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
        public async Task<IActionResult> Submit(Guid applicationId, int sequenceId)
        {
            var canUpdate = await CanUpdateApplication(applicationId, sequenceId);
            if (!canUpdate)
            {
                return RedirectToAction("Sequence", new { applicationId });
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

            if (await _apiClient.Submit(applicationId, sequenceId, User.GetUserId(), User.GetEmail()))
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
                ReferenceNumber = application?.ApplicationData?.ReferenceNumber,
                FeedbackUrl = config.FeedbackUrl,
                StandardName = application?.ApplicationData?.StandardName,
                StandardReference = application?.ApplicationData?.StandardReference,
                StandardLevel = application?.ApplicationData?.StandardLevel
            });
        }

        [HttpGet]
        public async Task<IActionResult> NotSubmitted(Guid applicationId)
        {
            var application = await _apiClient.GetApplication(applicationId);
            var config = await _configService.GetConfig();
            return View("~/Views/Application/NotSubmitted.cshtml", new SubmittedViewModel
            {
                ReferenceNumber = application?.ApplicationData?.ReferenceNumber,
                FeedbackUrl = config.FeedbackUrl,
                StandardName = application?.ApplicationData?.StandardName
            });
        }

        [HttpPost] 
        private async Task SavePreambleQuestions(Guid applicationId, Guid userId, List<PreambleAnswer> questions)
        {
            const int DefaultSectionId = 1;

            var preambleAnswers = questions
                .Where(x => x.SequenceId == RoatpWorkflowSequenceIds.Preamble).AsEnumerable<Answer>()
                .ToList();

            var applicationSequences = await _qnaApiClient.GetSequences(applicationId);
            var preambleSequence =
                applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.Preamble);
            var preambleSections = await _qnaApiClient.GetSections(applicationId, preambleSequence.Id);
            var questionsSection = preambleSections.FirstOrDefault(x => x.SectionId == DefaultSectionId);

            var updateResult = await _qnaApiClient.UpdatePageAnswers(applicationId, questionsSection.Id, RoatpWorkflowPageIds.Preamble, preambleAnswers);
            
            var yourOrganisationAnswers = questions
                .Where(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation).AsEnumerable<Answer>()
                .ToList();

            var yourOrganisationSequence =
                applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);
            var yourOrganisationSections = await _qnaApiClient.GetSections(applicationId, yourOrganisationSequence.Id);
            var yourOrganisationSection =
                yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.YourOrganisation.ProviderRoute);

            updateResult = await _qnaApiClient.UpdatePageAnswers(applicationId, yourOrganisationSection.Id, RoatpWorkflowPageIds.YourOrganisation, yourOrganisationAnswers);

            await _apiClient.MarkSectionAsCompleted(applicationId, yourOrganisationSection.Id);

            var conditionsOfAcceptanceAnswers = questions
                .Where(x => x.SequenceId == RoatpWorkflowSequenceIds.ConditionsOfAcceptance).AsEnumerable<Answer>()
                .ToList();

            var conditionsOfAcceptanceSequence = applicationSequences.FirstOrDefault(x =>
                x.SequenceId == RoatpWorkflowSequenceIds.ConditionsOfAcceptance);
            var conditionsOfAcceptanceSections =
                await _qnaApiClient.GetSections(applicationId, conditionsOfAcceptanceSequence.Id);
            var conditionsOfAcceptanceSection =
                conditionsOfAcceptanceSections.FirstOrDefault(x => x.SectionId == DefaultSectionId);

            updateResult = await _qnaApiClient.UpdatePageAnswers(applicationId, conditionsOfAcceptanceSection.Id, RoatpWorkflowPageIds.ConditionsOfAcceptance, conditionsOfAcceptanceAnswers);
        }

        private async Task SaveCompaniesHouseWhosInControlQuestions(Guid applicationId, Guid userId, List<PreambleAnswer> questions)
        {
            var applicationSequences = await _qnaApiClient.GetSequences(applicationId);
            
            var yourOrganisationSequence =
                applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);
            var yourOrganisationSections = await _qnaApiClient.GetSections(applicationId, yourOrganisationSequence.Id);
            var whosInControlSection = yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);
       
            var whosInControlQuestions = questions.AsEnumerable<Answer>().ToList();                  

            var updateResult = await _qnaApiClient.UpdatePageAnswers(applicationId, whosInControlSection.Id, RoatpWorkflowPageIds.WhosInControl.CompaniesHouseStartPage, whosInControlQuestions);
        }

        private async Task SaveCharityCommissionWhosInControlQuestions(Guid applicationId, Guid userId, List<PreambleAnswer> questions)
        {
            var applicationSequences = await _qnaApiClient.GetSequences(applicationId);

            var yourOrganisationSequence =
                applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);
            var yourOrganisationSections = await _qnaApiClient.GetSections(applicationId, yourOrganisationSequence.Id);
            var whosInControlSection = yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

            var whosInControlQuestions = questions.AsEnumerable<Answer>().ToList();

            var updateResult = await _qnaApiClient.UpdatePageAnswers(applicationId, whosInControlSection.Id, RoatpWorkflowPageIds.WhosInControl.CharityCommissionStartPage, whosInControlQuestions);
        }

        private bool IsPostBack()
        {
            return ControllerContext.HttpContext.Request.Method?.ToUpper() == "POST";
        }
        
        private async Task<PageViewModel> TokeniseViewModelProperties(PageViewModel viewModel)
        {
            viewModel.Title =  await _questionPropertyTokeniser.GetTokenisedValue(viewModel.ApplicationId, viewModel.Title);
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
            if (page.PageOfAnswers == null || page.PageOfAnswers.Count ==0 ||  page.Questions == null || page.Questions.Count == 0 || page.Questions[0].Input.Type != "FileUpload")
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
    }
}