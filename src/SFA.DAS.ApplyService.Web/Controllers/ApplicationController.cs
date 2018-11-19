using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using SFA.DAS.ApplyService.Application.Apply.Upload;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.Infrastructure;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    [Authorize]
    public class ApplicationController : Controller
    {
        private readonly IApplicationApiClient _apiClient;

        public ApplicationController(IApplicationApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        
        [HttpGet("/Applications")]
        public async Task<IActionResult> Applications()
        {
            var applications = await _apiClient.GetApplicationsFor(Guid.Parse(User.FindFirstValue("UserId")));
            
            return View(applications);
        }
        
        [HttpPost("/Applications")]
        public async Task<IActionResult> StartApplication()
        {
            await _apiClient.StartApplication(Guid.Parse(User.FindFirstValue("UserId")));

            return RedirectToAction("Applications");
        }
        
        
        
        
        [HttpGet("/Applications/{applicationId}")]
        public async Task<IActionResult> Sections(Guid applicationId)
        {
            var sections = await _apiClient.GetSections(applicationId, sequenceId: 1, userId: Guid.Parse(User.FindFirstValue("UserId")));
            
            return View(sections);
        }
        
        [HttpGet("/Applications/{applicationId}/Sections/{sectionId}")]
        public async Task<IActionResult> Section(Guid applicationId, int sectionId)
        {
            var sections = await _apiClient.GetSection(applicationId, sequenceId: 1, sectionId: sectionId, userId: Guid.Parse(User.FindFirstValue("UserId")));
            
            return View(sections);
        }
        
        [HttpGet("/Application/{applicationId}/Sequences/{sequenceId}/Sections/{sectionId}/Pages/{pageId}")]
        public async Task<IActionResult> Page(string applicationId, int sequenceId, int sectionId, string pageId)
        {
            var page = await _apiClient.GetPage(Guid.Parse(applicationId), sequenceId,sectionId, pageId, Guid.Parse(User.FindFirstValue("UserId")));
            var pageVm = new PageViewModel(page, Guid.Parse(applicationId));

            pageVm.SectionId = sectionId;

            if (page.AllowMultipleAnswers)
            {
                return View("~/Views/Application/Pages/MultipleAnswers.cshtml", pageVm);
            }
            return View("~/Views/Application/Pages/Index.cshtml", pageVm);
        }


        [HttpPost("/Application/{applicationId}/Sequences/{sequenceId}/Sections/{sectionId}/Pages/{pageId}/NextPage")]
        public async Task<IActionResult> NextPage(Guid applicationId, int sequenceId, int sectionId, string pageId)
        {
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
                    return RedirectToAction("Page", new {applicationId, sequenceId = thisPage.SequenceId, sectionId = thisPage.SectionId, pageId = next.ReturnId});
                }
                    
                return next.Action == "ReturnToSection"
                    ? RedirectToAction("Section", "Application", new {applicationId, sectionId = next.ReturnId})
                    : RedirectToAction("Sections", "Application", new {applicationId});
            }
            return RedirectToAction("Page", new {applicationId, pageId = thisPage.PageId});
        }
        
        [HttpPost("/Application/{applicationId}/Sequences/{sequenceId}/Sections/{sectionId}/Pages/{pageId}")]
        public async Task<IActionResult> SaveAnswers(string applicationId, int sequenceId, int sectionId, string pageId)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId"));
            
            var answers = new List<Answer>();

            if (HttpContext.Request.Form.Files.Any())
            {
                var uploadedFile = await _apiClient.Upload(applicationId, userId.ToString(), pageId,
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
                            sectionId = updatePageResult.Page.SectionId, pageId = updatePageResult.Page.PageId
                        });
                }
                
                var nextActions = updatePageResult.Page.Next;

                if (nextActions.Count == 1)
                {
                    var pageNext = nextActions[0];
                    if (pageNext.Action == "NextPage" && pageNext.ConditionMet)
                    {
                        return RedirectToAction("Page", new {applicationId, pageId = pageNext.ReturnId});
                    }
                    
                    return pageNext.Action == "ReturnToSection"
                        ? RedirectToAction("Section", "Application", new {applicationId, sectionId = pageNext.ReturnId})
                        : RedirectToAction("Sections", "Application", new {applicationId});
                }
                else
                {
                    var nextConditionMet = nextActions.FirstOrDefault(na => na.ConditionMet);
                    if (nextConditionMet != null)
                    {
                        if (nextConditionMet.Action == "NextPage")
                        {
                            return RedirectToAction("Page", new {applicationId, pageId = nextConditionMet.ReturnId});
                        }   
                        
                        return nextConditionMet.Action == "ReturnToSequence"
                            ? RedirectToAction("Section", "Application", new {applicationId, sectionId = nextConditionMet.ReturnId})
                            : RedirectToAction("Sections", "Application", new {applicationId});
                    }
                }

                foreach (var nextAction in nextActions)
                {
                    if (nextAction.Condition.MustEqual == answers.Single(a => a.QuestionId == nextAction.Condition.QuestionId).Value)
                    {
                        return RedirectToAction("Index", new {pageId = nextAction.ReturnId});
                    }
                }
                return RedirectToAction("Sections", "Application", new {applicationId});
            }

            foreach (var error in updatePageResult.ValidationErrors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }

            var pageVm = new PageViewModel(updatePageResult.Page, Guid.Parse(applicationId));
            return View("~/Views/Application/Pages/Index.cshtml", pageVm);
        }

        [HttpGet("/Application/{applicationId}/Page/{pageId}/Question/{questionId}/File/{filename}/Download")]
        public async Task<IActionResult> Download(Guid applicationId, string pageId, string questionId, string filename)
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId"));

            var file = await _apiClient.Download(applicationId, userId, pageId, questionId, filename);

            return File(file, "file/file", filename);
        }
    }
}