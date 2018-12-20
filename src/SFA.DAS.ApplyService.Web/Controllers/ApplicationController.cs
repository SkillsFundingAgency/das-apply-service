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
 using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
 
 namespace SFA.DAS.ApplyService.Web.Controllers
 {
     [Authorize]
     public class ApplicationController : Controller
     {
         private readonly IApplicationApiClient _apiClient;
         private readonly ILogger<ApplicationController> _logger;
         private readonly ISessionService _sessionService;

         public ApplicationController(IApplicationApiClient apiClient, ILogger<ApplicationController> logger, ISessionService sessionService)
         {
             _apiClient = apiClient;
             _logger = logger;
             _sessionService = sessionService;
         }
         
         [HttpGet("/Applications")]
         public async Task<IActionResult> Applications()
         {
             var user = _sessionService.Get("LoggedInUser");
             _logger.LogInformation($"Got LoggedInUser from Session: {user}");
             
             var applications = await _apiClient.GetApplicationsFor(Guid.Parse(User.FindFirstValue("UserId")));

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
                     return View("~/Views/Application/FeedbackIntro.cshtml", application.Id );
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
         public async Task<IActionResult> Sequence(Guid applicationId)
         {
             // Break this out into a "Signpost" action.
             var sequence = await _apiClient.GetSequence(applicationId, userId: Guid.Parse(User.FindFirstValue("UserId")));

                return View(sequence);
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
                 applicationData = JsonConvert.DeserializeObject<StandardApplicationData>(application.ApplicationData);
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
             else if (section.DisplayType == SectionDisplayType.Questions)
             {
                 return View("~/Views/Application/SectionQuestions.cshtml", section);
             }
             throw new BadRequestException("Section does not have a valid DisplayType");
         }
         
         [HttpGet("/Application/{applicationId}/Sequences/{sequenceId}/Sections/{sectionId}/Pages/{pageId}/{redirectAction}")]
         public async Task<IActionResult> Page(string applicationId, int sequenceId, int sectionId, string pageId, string redirectAction)
         {
             var page = await _apiClient.GetPage(Guid.Parse(applicationId), sequenceId,sectionId, pageId, Guid.Parse(User.FindFirstValue("UserId")));
             var pageVm = new PageViewModel(page, Guid.Parse(applicationId));
 
             pageVm.SectionId = sectionId;

             pageVm.RedirectAction = redirectAction;
             
             if (page.AllowMultipleAnswers)
             {
                 return View("~/Views/Application/Pages/MultipleAnswers.cshtml", pageVm);
             }
             return View("~/Views/Application/Pages/Index.cshtml", pageVm);
         }
 
 
         [HttpPost("/Application/{applicationId}/Sequences/{sequenceId}/Sections/{sectionId}/Pages/{pageId}/NextPage/{redirectAction}")]
         public async Task<IActionResult> NextPage(Guid applicationId, int sequenceId, int sectionId, string pageId, string redirectAction)
         {
             if (redirectAction == "Feedback")
             {
                 return RedirectToAction("Feedback", new {applicationId});
             }
             
             var thisPage = await _apiClient.GetPage(applicationId, sequenceId, sectionId, pageId,Guid.Parse(User.FindFirstValue("UserId")));
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
             
             var answers = new List<Answer>();
 
             if (HttpContext.Request.Form.Files.Any())
             {
                 var uploadedFile = await _apiClient.Upload(applicationId, userId.ToString(), sequenceId, sectionId, pageId,
                     HttpContext.Request.Form.Files);
 
                 foreach (var file in uploadedFile.Files)
                 {
                     answers.Add(new Answer() {QuestionId = file.QuestionId, Value = file.UploadedFileName});   
                 }
             }
             
             foreach (var keyValuePair in HttpContext.Request.Form.Where(f => !f.Key.StartsWith("__")))
             {
                 answers.Add(new Answer() {QuestionId = keyValuePair.Key, Value = keyValuePair.Value});
             }
 
             
             var updatePageResult = await _apiClient.UpdatePageAnswers(Guid.Parse(applicationId), userId, sequenceId, sectionId, pageId, answers);
 
             if (updatePageResult.ValidationPassed)
             {
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
                 else
                 {
                     var nextConditionMet = nextActions.FirstOrDefault(na => na.ConditionMet);
                     if (nextConditionMet != null)
                     {
                         if (nextConditionMet.Action == "NextPage")
                         {
                             return RedirectToAction("Page", new {applicationId, pageId = nextConditionMet.ReturnId, redirectAction = redirectAction});
                         }   
                         
                         return nextConditionMet.Action == "ReturnToSequence"
                             ? RedirectToAction("Section", "Application", new {applicationId, sectionId = nextConditionMet.ReturnId})
                             : RedirectToAction("Sequence", "Application", new {applicationId});
                     }
                 }
 
//                 foreach (var nextAction in nextActions)
//                 {
//                     if (nextAction.Condition.MustEqual == answers.Single(a => a.QuestionId == nextAction.Condition.QuestionId).Value)
//                     {
//                         return RedirectToAction("Index", new {pageId = nextAction.ReturnId});
//                     }
//                 }
                 return RedirectToAction("Sequence", "Application", new {applicationId});
             }

             var errorMessages = new List<ValidationErrorDetail>();

             foreach (var error in updatePageResult.ValidationErrors)
             {
                 ModelState.AddModelError(error.Key, error.Value);
                errorMessages.Add(new ValidationErrorDetail(error.Key, error.Value));
             }
            var pageVm = new PageViewModel(updatePageResult.Page, Guid.Parse(applicationId), errorMessages);
             
             var page = await _apiClient.GetPage(Guid.Parse(applicationId), sequenceId,sectionId, pageId, Guid.Parse(User.FindFirstValue("UserId")));
             if (page.AllowMultipleAnswers)
             {
                 return View("~/Views/Application/Pages/MultipleAnswers.cshtml", pageVm);
             }
             return View("~/Views/Application/Pages/Index.cshtml", pageVm);
         }
 
         [HttpGet("/Application/{applicationId}/Page/{pageId}/Question/{questionId}/File/{filename}/Download")]
         public async Task<IActionResult> Download(Guid applicationId, string pageId, string questionId, string filename)
         {
             var userId = Guid.Parse(User.FindFirstValue("UserId"));
 
             var file = await _apiClient.Download(applicationId, userId, pageId, questionId, filename);
 
             return File(file, "file/file", filename);
         }
 
         
         [HttpPost("/Applications/Submit")]
         public async Task<IActionResult> Submit(Guid applicationId, int sequenceId)
         {
             await _apiClient.Submit(applicationId, sequenceId, Guid.Parse(User.FindFirstValue("UserId")));
             return RedirectToAction("Submitted");
         }
 
         [HttpPost("/Application/DeleteAnswer")]
         public async Task<IActionResult> DeleteAnswer(Guid applicationId, int sequenceId, int sectionId, string pageId, Guid answerId)
         {
             await _apiClient.DeleteAnswer(applicationId, sequenceId, sectionId, pageId, answerId,
                 Guid.Parse(User.FindFirstValue("UserId")));

             return RedirectToAction("Page", new {applicationId, sequenceId, sectionId, pageId});
         }

         [HttpGet("/Application/{applicationId}/Feedback")]
         public async Task<IActionResult> Feedback(Guid applicationId)
         {
             var sequence = await _apiClient.GetSequence(applicationId, userId: Guid.Parse(User.FindFirstValue("UserId")));

             return View("~/Views/Application/Feedback.cshtml", sequence);
         }

         [HttpGet("/Application/Submitted")]
         public async Task<IActionResult> Submitted()
         {
             return View("~/Views/Application/Submitted.cshtml");
         }
     }
 }