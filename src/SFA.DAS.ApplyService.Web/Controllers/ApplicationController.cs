using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application.Apply.GetPage;
using SFA.DAS.ApplyService.Application.Apply.Upload;
using SFA.DAS.ApplyService.Application.Apply.Validation;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
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
        private readonly ISessionService _sessionService;
        private readonly IConfigurationService _configService;
        private readonly UserService _userService;

        public ApplicationController(IApplicationApiClient apiClient, ILogger<ApplicationController> logger, ISessionService sessionService, IConfigurationService configService, UserService userService)
        {
            _apiClient = apiClient;
            _logger = logger;
            _sessionService = sessionService;
            _configService = configService;
            _userService = userService;
        }

        [HttpGet("/Applications")]
        public async Task<IActionResult> Applications()
        {
            var user = _sessionService.Get("LoggedInUser");
            _logger.LogInformation($"Got LoggedInUser from Session: {user}");

            //var applications = await _apiClient.GetApplicationsFor(Guid.Parse(User.FindFirstValue("UserId")));
            var applications = await _apiClient.GetApplicationsFor(Guid.Parse(await _userService.GetClaim("UserId")));

            if (!applications.Any())
            {
                await _apiClient.StartApplication(Guid.Parse(User.FindFirstValue("UserId")));
                applications = await _apiClient.GetApplicationsFor(Guid.Parse(User.FindFirstValue("UserId")));
                return RedirectToAction("SequenceSignPost", new {applicationId = applications.First().Id});
            }

            if (applications.Count() > 1)
            {
                return View(applications);
            }
            else
            {
                var application = applications.First();

                if (application.ApplicationStatus == ApplicationStatus.FeedbackAdded)
                {
                    return View("~/Views/Application/FeedbackIntro.cshtml", application.Id);
                }
                else if (application.ApplicationStatus == ApplicationStatus.Rejected)
                {
                    return View(applications);
                }
                else if (application.ApplicationStatus == ApplicationStatus.Approved)
                {
                    return View(applications);
                }

                return RedirectToAction("SequenceSignPost", new {applicationId = application.Id});
            }
        }

        [HttpPost("/Applications")]
        public async Task<IActionResult> StartApplication()
        {
            await _apiClient.StartApplication(Guid.Parse(User.FindFirstValue("UserId")));

            return RedirectToAction("Applications");
        }

        [HttpGet("/Applications/{applicationId}/Sequence")]
        public async Task<IActionResult> Sequence(Guid applicationId, bool notAcceptedTermsAndConditions)
        {
            // Break this out into a "Signpost" action.
            var sequence = await _apiClient.GetSequence(applicationId, userId: Guid.Parse(User.FindFirstValue("UserId")));

            var errorMessages = new List<ValidationErrorDetail>();

            if (notAcceptedTermsAndConditions)
            {
                string key = "terms-and-conditions";
                string errorMessage = "You must accept the terms and conditions to proceed";
                ModelState.AddModelError(key, errorMessage);
                errorMessages.Add(new ValidationErrorDetail(key, errorMessage));
            }

            var sequenceVm = new SequenceViewModel(sequence, applicationId, errorMessages);
            return View(sequenceVm);
        }

        [HttpGet("/Applications/{applicationId}")]
        public async Task<IActionResult> SequenceSignPost(Guid applicationId)
        {
            var application = await _apiClient.GetApplication(applicationId);
            if (application.ApplicationStatus == ApplicationStatus.Approved)
            {
                return View("~/Views/Application/Approved.cshtml", application);
            }

            if (application.ApplicationStatus == ApplicationStatus.Rejected)
            {
                return View("~/Views/Application/Rejected.cshtml", application);
            }

            var sequence = await _apiClient.GetSequence(applicationId, userId: Guid.Parse(User.FindFirstValue("UserId")));

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
                //return View("", sequence);
            }
            else if (sequence.SequenceId == SequenceId.Stage2 && string.IsNullOrWhiteSpace(applicationData?.StandardName))
            {
                //return RedirectToAction("Search", "Standard", new {applicationId});
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
            var section = await _apiClient.GetSection(applicationId, sequenceId, sectionId, userId: Guid.Parse(User.FindFirstValue("UserId")));

            if (section.DisplayType == SectionDisplayType.Pages)
            {
                return View("~/Views/Application/Section.cshtml", section);
            }
            if (section.DisplayType == SectionDisplayType.Questions)
            {
                return View("~/Views/Application/SectionQuestions.cshtml", section);
            }
            if (section.DisplayType == SectionDisplayType.PagesWithSections)
            {
                return View("~/Views/Application/PagesWithSections.cshtml", section);
            }

            throw new BadRequestException("Section does not have a valid DisplayType");
        }

        [HttpGet("/Application/{applicationId}/Sequences/{sequenceId}/Sections/{sectionId}/Pages/{pageId}/{redirectAction}")]
        public async Task<IActionResult> Page(string applicationId, int sequenceId, int sectionId, string pageId, string redirectAction)
        {
            var page = await _apiClient.GetPage(Guid.Parse(applicationId), sequenceId, sectionId, pageId, Guid.Parse(User.FindFirstValue("UserId")));

            page = await GetDataFedOptions(page);
            
            var pageVm = new PageViewModel(page, Guid.Parse(applicationId));

            pageVm.SectionId = sectionId;
            pageVm.RedirectAction = redirectAction;

     
            ProcessPageVmQuestionsForStandardName(pageVm.Questions, applicationId);

            if (page.AllowMultipleAnswers)
            {
                return View("~/Views/Application/Pages/MultipleAnswers.cshtml", pageVm);
            }

            return View("~/Views/Application/Pages/Index.cshtml", pageVm);
        }

        private async Task<Page> GetDataFedOptions(Page page)
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

            return page;
        }

        private void ProcessPageVmQuestionsForStandardName(List<QuestionViewModel> pageVmQuestions, string applicationId)
         {
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

             var application = _apiClient.GetApplication(Guid.Parse(applicationId)).Result;
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


        [HttpPost("/Application/{applicationId}/Sequences/{sequenceId}/Sections/{sectionId}/Pages/{pageId}/NextPage/{redirectAction}")]
        public async Task<IActionResult> NextPage(Guid applicationId, int sequenceId, int sectionId, string pageId, string redirectAction)
        {
            if (redirectAction == "Feedback")
            {
                return RedirectToAction("Feedback", new {applicationId});
            }

            var thisPage = await _apiClient.GetPage(applicationId, sequenceId, sectionId, pageId, Guid.Parse(User.FindFirstValue("UserId")));
            if (thisPage.PageOfAnswers.Any())
            {
                var next = thisPage.Next.FirstOrDefault();
                if (next == null)
                {
                    RedirectToAction("Section", "Application", new {applicationId, sectionId = thisPage.SectionId});
                }

                if (next.Action == "NextPage")
                {
                    return RedirectToAction("Page", new {applicationId, sequenceId = thisPage.SequenceId, sectionId = thisPage.SectionId, pageId = next.ReturnId, redirectAction});
                }

                return next.Action == "ReturnToSection"
                    ? RedirectToAction("Section", "Application", new {applicationId, sectionId = next.ReturnId})
                    : RedirectToAction("Sequence", "Application", new {applicationId});
            }

            return RedirectToAction("Page", new {applicationId, sequenceId = thisPage.SequenceId, sectionId = thisPage.SectionId, pageId = thisPage.PageId, redirectAction});
        }

        [HttpPost("/Application/{applicationId}/Sequences/{sequenceId}/Sections/{sectionId}/Pages/{pageId}/{redirectAction}")]
        public async Task<IActionResult> SaveAnswers(string applicationId, int sequenceId, int sectionId, string pageId, string redirectAction)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId"));

            var page = await _apiClient.GetPage(Guid.Parse(applicationId), sequenceId, sectionId, pageId, Guid.Parse(User.FindFirstValue("UserId")));

            var errorMessages = new List<ValidationErrorDetail>();
            var answers = new List<Answer>();
            
            var fileValidationPassed = FileValidationPassed(answers, page, errorMessages);
            GetAnswersFromForm(answers);

            var updatePageResult = await _apiClient.UpdatePageAnswers(Guid.Parse(applicationId), userId, sequenceId, sectionId, pageId, answers);


            if (updatePageResult.ValidationPassed && fileValidationPassed)
            {
                await UploadFilesToStorage(applicationId, sequenceId, sectionId, pageId, userId);

                if (updatePageResult.Page.AllowMultipleAnswers)
                {
                    return RedirectToAction("Page",
                        new
                        {
                            applicationId, sequenceId = updatePageResult.Page.SequenceId,
                            sectionId = updatePageResult.Page.SectionId, pageId = updatePageResult.Page.PageId, redirectAction = redirectAction
                        });
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
                        return RedirectToAction("Page", new {applicationId, pageId = pageNext.ReturnId, redirectAction = redirectAction});
                    }

                    return pageNext.Action == "ReturnToSection"
                        ? RedirectToAction("Section", "Application", new {applicationId, sectionId = pageNext.ReturnId})
                        : RedirectToAction("Sequence", "Application", new {applicationId});
                }

                var nextConditionMet = nextActions.FirstOrDefault(na => na.ConditionMet);
                if (nextConditionMet == null) return RedirectToAction("Sequence", "Application", new {applicationId});
                
                if (nextConditionMet.Action == "NextPage")
                {
                    return RedirectToAction("Page", new {applicationId, pageId = nextConditionMet.ReturnId, redirectAction = redirectAction});
                }

                return nextConditionMet.Action == "ReturnToSequence"
                    ? RedirectToAction("Section", "Application", new {applicationId, sectionId = nextConditionMet.ReturnId})
                    : RedirectToAction("Sequence", "Application", new {applicationId});
            }

            if (updatePageResult.ValidationErrors != null)
            {
                foreach (var error in updatePageResult.ValidationErrors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                    errorMessages.Add(new ValidationErrorDetail(error.Key, error.Value));
                }
            }

            var pageVm = new PageViewModel(updatePageResult.Page, Guid.Parse(applicationId), errorMessages);


            if (page.AllowMultipleAnswers)
            {
                return View("~/Views/Application/Pages/MultipleAnswers.cshtml", pageVm);
            }

            return View("~/Views/Application/Pages/Index.cshtml", pageVm);
        }

        private async Task UploadFilesToStorage(string applicationId, int sequenceId, int sectionId, string pageId, Guid userId)
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
                answers.Add(new Answer() {QuestionId = file.Name, Value = file.FileName});
                var typeValidation = page.Questions.First(q => q.QuestionId == file.Name).Input.Validations.FirstOrDefault(v => v.Name == "FileType");
                if (typeValidation == null) continue;
                var extension = typeValidation.Value.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries)[0];
                var mimeType = typeValidation.Value.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries)[1];

                if (file.FileName.Substring(file.FileName.IndexOf(".") + 1, (file.FileName.Length - 1) - file.FileName.IndexOf(".")).ToLower() == extension && file.ContentType.ToLower() == mimeType) continue;

                ModelState.AddModelError(file.Name, typeValidation.ErrorMessage);
                errorMessages.Add(new ValidationErrorDetail(file.Name, typeValidation.ErrorMessage));
                fileValidationPassed = false;
            }

            return fileValidationPassed;
        }

        [HttpGet("/Application/{applicationId}/Page/{pageId}/Question/{questionId}/File/{filename}/Download")]
        public async Task<IActionResult> Download(Guid applicationId, string pageId, string questionId, string filename)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId"));

            var file = await _apiClient.Download(applicationId, userId, pageId, questionId, filename);

            return File(file, "file/file", filename);
        }


        [HttpPost("/Applications/Submit")]
        public async Task<IActionResult> Submit(Guid applicationId, int sequenceId, bool acceptedTermsAndConditions)
        {
            if (!acceptedTermsAndConditions)
            {
                return RedirectToAction("Sequence", new {applicationId, notAcceptedTermsAndConditions = true});
            }

            await _apiClient.Submit(applicationId, sequenceId, Guid.Parse(User.FindFirstValue("UserId")), User.FindFirstValue("Email"));
            return RedirectToAction("Submitted", new {applicationId});
        }

        [HttpPost("/Application/DeleteAnswer")]
        public async Task<IActionResult> DeleteAnswer(Guid applicationId, int sequenceId, int sectionId, string pageId, Guid answerId, string redirectAction)
        {
            await _apiClient.DeleteAnswer(applicationId, sequenceId, sectionId, pageId, answerId,
                Guid.Parse(User.FindFirstValue("UserId")));
            
            return RedirectToAction("Page", new {applicationId, sequenceId, sectionId, pageId, redirectAction});
            
            //return RedirectToAction("Page", new {applicationId, sequenceId, sectionId, pageId});
        }

        [HttpGet("/Application/{applicationId}/Feedback")]
        public async Task<IActionResult> Feedback(Guid applicationId)
        {
            var sequence = await _apiClient.GetSequence(applicationId, userId: Guid.Parse(User.FindFirstValue("UserId")));

            return View("~/Views/Application/Feedback.cshtml", sequence);
        }

        [HttpGet("/Application/{applicationId}/Submitted")]
        public async Task<IActionResult> Submitted(Guid applicationId)
        {
            var application = await _apiClient.GetApplication(applicationId);
            var config = await _configService.GetConfig();
            return View("~/Views/Application/Submitted.cshtml", new SubmittedViewModel
            {
                ReferenceNumber = application.ApplicationData.ReferenceNumber,
                FeedbackUrl = config.FeedbackUrl
            });
        }
    }
}