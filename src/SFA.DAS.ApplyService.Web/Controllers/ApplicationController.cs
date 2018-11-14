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
        
        public async Task<IActionResult> Index()
        {
            var applications = await _apiClient.GetApplicationsFor(Guid.Parse(User.FindFirstValue("UserId")));
            
            return View(applications);
        }

//        [HttpGet("/Application/{applicationId}/Sequence/{sequenceId}/Sections")]
//        public async Task<IActionResult> Sections(string applicationId, int sequenceId)
//        {
//            var sections = await _apiClient.GetSections(Guid.Parse(applicationId), sequenceId, Guid.Parse(User.FindFirstValue("UserId")));
//
//            var sequencesViewModel = sections.Select(s => new SequenceViewModel(s, Guid.Parse(applicationId))).ToList();
//            
//            return View("~/Views/Application/Sequences/Index.cshtml", sequencesViewModel);
//        }
//        
//        [HttpGet("/Application/{applicationId}/Sequences/{sequenceId}")]
//        public async Task<IActionResult> Sequence(string applicationId, string sequenceId)
//        {
//            var sequence = await _apiClient.GetSequence(Guid.Parse(applicationId), sequenceId,
//                Guid.Parse(User.FindFirstValue("UserId")));
//
//            var sequenceViewModel = new SequenceViewModel(sequence, Guid.Parse(applicationId));
//            
//            return View("~/Views/Application/Sequences/Sequence.cshtml", sequenceViewModel);
//        }

        [HttpGet("/Application/{applicationId}/Pages/{pageId}")]
        public async Task<IActionResult> Page(string applicationId, string pageId)
        {
            var page = await _apiClient.GetPage(Guid.Parse(applicationId), pageId, Guid.Parse(User.FindFirstValue("UserId")));
            var pageVm = new PageViewModel(page, Guid.Parse(applicationId));   
            
            return View("~/Views/Application/Pages/Index.cshtml", pageVm);
        }
        
        [HttpPost("/Application/{applicationId}/Pages/{pageId}")]
        public async Task<IActionResult> SaveAnswers(string applicationId, string pageId)
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

            
            var updatePageResult = await _apiClient.UpdatePageAnswers(Guid.Parse(applicationId), userId, pageId, answers);

            if (updatePageResult.ValidationPassed)
            {
                var nextActions = updatePageResult.Page.Next;

                if (nextActions.Count == 1)
                {
                    var pageNext = nextActions[0];
                    if (pageNext.Action == "NextPage" && pageNext.ConditionMet)
                    {
                        return RedirectToAction("Page", new {applicationId, pageId = pageNext.ReturnId});
                    }
                    
                    return pageNext.Action == "ReturnToSequence"
                        ? RedirectToAction("Sequence", "Application", new {applicationId, sequenceId = pageNext.ReturnId})
                        : RedirectToAction("Sequences", "Application", new {applicationId});
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
                            ? RedirectToAction("Sequence", "Application", new {applicationId, sequenceId = nextConditionMet.ReturnId})
                            : RedirectToAction("Sequences", "Application", new {applicationId});
                    }
                }

                foreach (var nextAction in nextActions)
                {
                    if (nextAction.Condition.MustEqual == answers.Single(a => a.QuestionId == nextAction.Condition.QuestionId).Value)
                    {
                        return RedirectToAction("Index", new {pageId = nextAction.ReturnId});
                    }
                }
                return RedirectToAction("Sequences", "Application", new {applicationId});
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