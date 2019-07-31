using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application.Apply.GetPage;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Application.Apply.Validation;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    [Authorize]
    public class ApplicationController : Controller
    {
        private readonly IApplicationApiClient _apiClient;
        private readonly ILogger<ApplicationController> _logger;
        private readonly IUsersApiClient _usersApiClient;
        private readonly ISessionService _sessionService;
        private readonly IConfigurationService _configService;
        private readonly IUserService _userService;

        private const string ApplicationDetailsKey = "Roatp_Application_Details";

        public ApplicationController(IApplicationApiClient apiClient, ILogger<ApplicationController> logger,
            ISessionService sessionService, IConfigurationService configService, IUserService userService, IUsersApiClient usersApiClient)
        {
            _apiClient = apiClient;
            _logger = logger;
            _sessionService = sessionService;
            _configService = configService;
            _userService = userService;
            _usersApiClient = usersApiClient;
        }

        [HttpGet("/Applications")]
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
                if (applicationType == ApplicationTypes.EndpointAssessor)
                {
                    if (org != null && org.RoEPAOApproved)
                    {
                        return await StartApplication(userId, applicationType);
                    }
                }

                if (applicationType == ApplicationTypes.RegisterTrainingProviders)
                {
                    return await StartApplication(userId, applicationType);
                }

                //ON-2068 Registered org  with no application created via digital service then
                //display empty list of application screen
                if (org != null)
                    return org.RoEPAOApproved ? View(applications) : View("~/Views/Application/Declaration.cshtml");

                if (await _userService.AssociateOrgFromClaimWithUser())
					return await StartApplication(userId, ApplicationTypes.EndpointAssessor);

                return View("~/Views/Application/Declaration.cshtml", applicationType);

            }

            //ON-2068 If there is an existing application for an org that is registered then display it
            //in a list of application screen
            if (applications.Count() == 1 && (org != null && org.RoEPAOApproved))
                return View(applications);

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
                    return RedirectToAction("SequenceSignPost", new {applicationId = application.Id});
            }
        }


        private async Task<IActionResult> StartApplication(Guid userId, string applicationType)
        {
            var response = await _apiClient.StartApplication(userId, applicationType);

            if (applicationType == ApplicationTypes.RegisterTrainingProviders)
            {
                var applicationDetails = _sessionService.Get<ApplicationDetails>(ApplicationDetailsKey);

                var preambleQuestions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(applicationDetails);

                await SavePreambleQuestions(response.ApplicationId, userId, preambleQuestions);
            }

            return RedirectToAction("Applications");
        }

        [HttpPost("/Applications")]
        public async Task<IActionResult> StartApplication(string applicationType)
        {
            var response = await _apiClient.StartApplication(await _userService.GetUserId(), applicationType);

            if (applicationType == ApplicationTypes.RegisterTrainingProviders)
            {
                return RedirectToAction("TaskList", new {applicationId = response.ApplicationId});
            }
            return RedirectToAction("SequenceSignPost", new {applicationId = response.ApplicationId});
        }

        [HttpGet("/Applications/{applicationId}/Sequence")]
        public async Task<IActionResult> Sequence(Guid applicationId)
        {
            // Break this out into a "Signpost" action.
            var sequence = await _apiClient.GetSequence(applicationId, User.GetUserId());
            var sequenceVm = new SequenceViewModel(sequence, applicationId, null);
            return View(sequenceVm);
        }

        [HttpGet("/Applications/{applicationId}")]
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

        [HttpGet("/Applications/{applicationId}/Sequences/{sequenceId}/Sections/{sectionId}")]
        public async Task<IActionResult> Section(Guid applicationId, int sequenceId, int sectionId)
        {
            var canUpdate = await CanUpdateApplication(applicationId, sequenceId);
            if (!canUpdate)
            {
                return RedirectToAction("Sequence", new { applicationId });
            }

            var section = await _apiClient.GetSection(applicationId, sequenceId, sectionId, User.GetUserId());

            switch(section?.DisplayType)
            {
                case null:
                case SectionDisplayType.Pages:
                    return View("~/Views/Application/Section.cshtml", section);
                case SectionDisplayType.Questions:
                    return View("~/Views/Application/Section.cshtml", section);
                case SectionDisplayType.PagesWithSections:
                    return View("~/Views/Application/PagesWithSections.cshtml", section);
                default:
                    throw new BadRequestException("Section does not have a valid DisplayType");
            }
        }

        [HttpGet("/Application/{applicationId}/Sequences/{sequenceId}/Sections/{sectionId}/Pages/{pageId}"), ModelStatePersist(ModelStatePersist.RestoreEntry)]
        public async Task<IActionResult> Page(Guid applicationId, int sequenceId, int sectionId, string pageId, string redirectAction)
        {
            var sequence = await _apiClient.GetSequence(applicationId, User.GetUserId());
            if (!CanUpdateApplication(sequence, sequenceId))
            {
                return RedirectToAction("Sequence", new { applicationId });
            }

            PageViewModel viewModel = null;
            var returnUrl = Request.Headers["Referer"].ToString();

            string pageContext = string.Empty;
            if (sequence.SequenceId == SequenceId.Stage2)
            {
                var application = await _apiClient.GetApplication(applicationId);
                pageContext = $"{application?.ApplicationData?.StandardReference } {application?.ApplicationData?.StandardName}";
            }

            if (!ModelState.IsValid)
            {
                // when the model state has errors the page will be displayed with the values which failed validation
                var page = JsonConvert.DeserializeObject<Page>((string) this.TempData["InvalidPage"]);

                var errorMessages = !ModelState.IsValid
                    ? ModelState.SelectMany(k => k.Value.Errors.Select(e => new ValidationErrorDetail()
                    {
                        ErrorMessage = e.ErrorMessage,
                        Field = k.Key
                    })).ToList()
                    : null;

                viewModel = new PageViewModel(applicationId, sequenceId, sectionId, pageId, page, pageContext, redirectAction,
                    returnUrl, errorMessages);
            }
            else
            {
                // when the model state has no errors the page will be displayed with the last valid values which were saved
                var page = await _apiClient.GetPage(applicationId, sequenceId, sectionId, pageId, User.GetUserId());

                if (page != null && (!page.Active || page.NotRequired))
                {
                    var nextPage = page.Next.FirstOrDefault(p => p.Condition is null);

                    if (nextPage?.ReturnId != null && nextPage?.Action == "NextPage")
                    {
                        pageId = nextPage.ReturnId;
                        return RedirectToAction("Page",
                            new {applicationId, sequenceId, sectionId, pageId, redirectAction});
                    }
                    else
                    {
                        return RedirectToAction("Section", new {applicationId, sequenceId, sectionId});
                    }
                }

                page = await GetDataFedOptions(page);

                viewModel = new PageViewModel(applicationId, sequenceId, sectionId, pageId, page, pageContext, redirectAction,
                    returnUrl, null);

                ProcessPageVmQuestionsForStandardName(viewModel.Questions, applicationId);
            }

            if (viewModel.AllowMultipleAnswers)
            {
                return View("~/Views/Application/Pages/MultipleAnswers.cshtml", viewModel);
            }

            return View("~/Views/Application/Pages/Index.cshtml", viewModel);
        }

        [Route("task-list")]
        public async Task<IActionResult> TaskList(Guid applicationId)
        {
            return View("~/Views/Roatp/TaskList.cshtml");
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

        private async Task<Page> GetDataFedOptions(Page page)
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
                }
            }

            return page;
        }

        private void ProcessPageVmQuestionsForStandardName(List<QuestionViewModel> pageVmQuestions, Guid applicationId)
         {
            if (pageVmQuestions == null) return;

             var placeholderString = "StandardName";
             var isPlaceholderPresent = false;

             foreach (var question in pageVmQuestions)
             
                 if (question.Label.Contains($"[{placeholderString}]") ||
                    question.Hint.Contains($"[{placeholderString}]") ||
                     question.QuestionBodyText.Contains($"[{placeholderString}]") ||
                     question.ShortLabel.Contains($"[{placeholderString}]")
                    )
                    isPlaceholderPresent=true;

             if (!isPlaceholderPresent) return;

             var application = _apiClient.GetApplication(applicationId).Result;
             var standardName = application?.ApplicationData?.StandardName;


             if (string.IsNullOrEmpty(standardName)) standardName = "the standard to be selected";

            foreach (var question in pageVmQuestions)
             {
                question.Label = question.Label?.Replace($"[{placeholderString}]", standardName);
                question.Hint = question.Hint?.Replace($"[{placeholderString}]", standardName);
                question.QuestionBodyText = question.QuestionBodyText?.Replace($"[{placeholderString}]", standardName);
                question.ShortLabel = question.Label?.Replace($"[{placeholderString}]", standardName);
            }     
         }
       
        private async Task<bool> CheckIfAnswersMustBeValidated(Guid applicationId, int sequenceId, int sectionId,
            string pageId, List<Answer> answers, Regex inputEnteredRegex)
        {
            if (answers.Exists(p => inputEnteredRegex.IsMatch(p.Value) && p.QuestionId != "RedirectAction"))
            {
                return true;
            }
  
            var page = await _apiClient.GetPage(applicationId, sequenceId, sectionId, pageId, User.GetUserId());
            return !page.PageOfAnswers.Any();
        }

        [HttpPost("/Application/{applicationId}/Sequences/{sequenceId}/Sections/{sectionId}/Pages/{pageId}"), ModelStatePersist(ModelStatePersist.Store)]
        public async Task<IActionResult> SaveAnswers(Guid applicationId, int sequenceId, int sectionId, string pageId, string redirectAction, string __formAction)
        {
            var canUpdate = await CanUpdateApplication(applicationId, sequenceId);
            if (!canUpdate)
            {
                return RedirectToAction("Sequence", new { applicationId });
            }

            var userId = User.GetUserId();

            var page = await _apiClient.GetPage(applicationId, sequenceId, sectionId, pageId, userId);

            var errorMessages = new List<ValidationErrorDetail>();
            var answers = new List<Answer>();

            var fileValidationPassed = FileValidator.FileValidationPassed(answers, page, errorMessages, ModelState, HttpContext.Request.Form.Files);
            GetAnswersFromForm(answers);

            var answersMustBeValidated = await CheckIfAnswersMustBeValidated(applicationId, sequenceId, sectionId, pageId, answers, new Regex(@"\w+"));
            var saveNewAnswers = (__formAction == "Add" || answersMustBeValidated);

            var updatePageResult = await _apiClient.UpdatePageAnswers(applicationId, userId, sequenceId, sectionId, pageId, answers, saveNewAnswers);

            if (updatePageResult.ValidationPassed && fileValidationPassed)
            {
                await UploadFilesToStorage(applicationId, sequenceId, sectionId, pageId, userId);

                if (__formAction == "Add" && updatePageResult.Page.AllowMultipleAnswers)
                {
                    return RedirectToAction("Page", new {applicationId, sequenceId = updatePageResult.Page.SequenceId,
                        sectionId = updatePageResult.Page.SectionId, pageId = updatePageResult.Page.PageId, redirectAction});
                }

                if (redirectAction == "Feedback")
                {
                    return RedirectToAction("Feedback", new {applicationId});
                }

                var nextActions = updatePageResult.Page.Next;

                if (nextActions.Count == 1)
                {
                    var pageNext = nextActions[0];

                    if (pageNext.Action == "NextPage" && pageNext.ConditionMet)
                    {
                        return RedirectToAction("Page", new {applicationId, sequenceId = updatePageResult.Page.SequenceId,
                            sectionId = updatePageResult.Page.SectionId, pageId = pageNext.ReturnId, redirectAction});
                    }

                    return pageNext.Action == "ReturnToSection"
                        ? RedirectToAction("Section", "Application", new {applicationId, sequenceId = updatePageResult.Page.SequenceId, sectionId = pageNext.ReturnId})
                        : RedirectToAction("Sequence", "Application", new {applicationId});
                }

                var nextConditionMet = nextActions.FirstOrDefault(na => na.ConditionMet);

                if (nextConditionMet == null) return RedirectToAction("Sequence", "Application", new {applicationId});

                if (nextConditionMet.Action == "NextPage")
                {
                    return RedirectToAction("Page", new {applicationId, sequenceId = updatePageResult.Page.SequenceId,
                        sectionId = updatePageResult.Page.SectionId, pageId = nextConditionMet.ReturnId, redirectAction});
                }

                return nextConditionMet.Action == "ReturnToSequence"
                    ? RedirectToAction("Section", "Application", new {applicationId, sequenceId = updatePageResult.Page.SequenceId, sectionId = nextConditionMet.ReturnId})
                    : RedirectToAction("Sequence", "Application", new {applicationId});
            }

            if (updatePageResult.ValidationErrors != null)
            {
                foreach (var error in updatePageResult.ValidationErrors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            var invalidPage = await GetDataFedOptions(updatePageResult.Page);
            this.TempData["InvalidPage"] = JsonConvert.SerializeObject(invalidPage);

            return RedirectToAction("Page", new { applicationId, sequenceId, sectionId, pageId, redirectAction });
        }

        private async Task UploadFilesToStorage(Guid applicationId, int sequenceId, int sectionId, string pageId, Guid userId)
        {
            if (HttpContext.Request.Form.Files.Any())
            {
                await _apiClient.Upload(applicationId, userId.ToString(), sequenceId, sectionId, pageId,
                    HttpContext.Request.Form.Files);
            }
        }

        private void GetAnswersFromForm(List<Answer> answers)
        {
            foreach (var keyValuePair in HttpContext.Request.Form.Where(f => !f.Key.StartsWith("__")))
            {
                answers.Add(new Answer() {QuestionId = keyValuePair.Key, Value = keyValuePair.Value});
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

        [HttpGet("Application/{applicationId}/Sequence/{sequenceId}/Section/{sectionId}/Page/{pageId}/Question/{questionId}/{filename}/Download")]
        
        //[HttpGet("/Application/{applicationId}/Page/{pageId}/Question/{questionId}/File/{filename}/Download")]
        public async Task<IActionResult> Download(Guid applicationId, int sequenceId, int sectionId, string pageId, string questionId, string filename)
        {
            var userId = User.GetUserId();

            var fileInfo = await _apiClient.FileInfo(applicationId, userId, sequenceId, sectionId, pageId, questionId, filename);
            
            var file = await _apiClient.Download(applicationId, userId, sequenceId,sectionId, pageId, questionId, filename);

            var fileStream = await file.Content.ReadAsStreamAsync();
            
            return File(fileStream, fileInfo.ContentType, fileInfo.Filename);
        }

        [HttpGet("Application/{applicationId}/Sequence/{sequenceId}/Section/{sectionId}/Page/{pageId}/Question/{questionId}/{redirectAction}/Delete")]
        public async Task<IActionResult> DeleteFile(Guid applicationId, int sequenceId, int sectionId, string pageId, string questionId, string redirectAction)
        {
            await _apiClient.DeleteFile(applicationId, User.GetUserId(), sequenceId, sectionId, pageId, questionId);
            
            return RedirectToAction("Page", new {applicationId, sequenceId, sectionId, pageId, redirectAction});
        }


        [HttpPost("/Applications/Submit")]
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

        [HttpPost("/Application/DeleteAnswer")]
        public async Task<IActionResult> DeleteAnswer(Guid applicationId, int sequenceId, int sectionId, string pageId, Guid answerId, string redirectAction)
        {
            await _apiClient.DeleteAnswer(applicationId, sequenceId, sectionId, pageId, answerId, User.GetUserId());
            
            return RedirectToAction("Page", new {applicationId, sequenceId, sectionId, pageId, redirectAction});
        }

        [HttpGet("/Application/{applicationId}/Feedback")]
        public async Task<IActionResult> Feedback(Guid applicationId)
        {
            var sequence = await _apiClient.GetSequence(applicationId, User.GetUserId());
            var sequenceVm = new SequenceViewModel(sequence, applicationId, null);
            return View("~/Views/Application/Feedback.cshtml", sequenceVm);
        }

        [HttpGet("/Application/{applicationId}/Submitted")]
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

        [HttpGet("/Application/{applicationId}/NotSubmitted")]
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

        private async Task SavePreambleQuestions(Guid applicationId, Guid userId, List<PreambleAnswer> questions)
        {
            const int DefaultSectionId = 1;

            var preambleAnswers = questions
                .Where(x => x.SequenceId == RoatpWorkflowSequenceIds.Preamble).AsEnumerable<Answer>()
                .ToList();

            var updateResult = await _apiClient.UpdatePageAnswers(applicationId, userId,
                RoatpWorkflowSequenceIds.Preamble, DefaultSectionId, RoatpWorkflowPageIds.Preamble, preambleAnswers, true);
            
            var yourOrganisationAnswers = questions
                .Where(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation).AsEnumerable<Answer>()
                .ToList();

            updateResult = await _apiClient.UpdatePageAnswers(applicationId, userId,
                RoatpWorkflowSequenceIds.YourOrganisation, DefaultSectionId, RoatpWorkflowPageIds.YourOrganisation, yourOrganisationAnswers, true);
            
            var conditionsOfAcceptanceAnswers = questions
                .Where(x => x.SequenceId == RoatpWorkflowSequenceIds.ConditionsOfAcceptance).AsEnumerable<Answer>()
                .ToList();

            updateResult = await _apiClient.UpdatePageAnswers(applicationId, userId,
                RoatpWorkflowSequenceIds.ConditionsOfAcceptance, DefaultSectionId, RoatpWorkflowPageIds.ConditionsOfAcceptance, conditionsOfAcceptanceAnswers, true);
        }
    }
}