using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class StandardController : Controller
    {
        private readonly AssessorServiceApiClient _assessorServiceApiClient;
        private readonly IApplicationApiClient _apiClient;

        public StandardController(IApplicationApiClient apiClient, AssessorServiceApiClient assessorServiceApiClient)
        {
            _apiClient = apiClient;
            _assessorServiceApiClient = assessorServiceApiClient;
        }

        [HttpGet("Standard/{applicationId}")]
        public async Task<IActionResult> Index(Guid applicationId)
        {
            var standardViewModel = new StandardViewModel {ApplicationId = applicationId};
            return View("~/Views/Application/Standard/FindStandard.cshtml", standardViewModel);
        }

        //[HttpPost("Standard/{applicationId}")]
        //public async Task<IActionResult> Search(Guid applicationId, string standardToFind, StandardViewModel model)
        //{
        //    // TODO: Check standard is valid.

        //    var standardApplicationData = new StandardApplicationData {StandardName = standardToFind};

        //    //await _apiClient.UpdateApplicationData(standardApplicationData, applicationId);

        //    ////var sequence = _apiClient.GetSequence(applicationId, Guid.Parse(User.FindFirstValue("UserId")));

        //    //return RedirectToAction("Sequence", "Application", new {applicationId});

        //    return View("~/Views/Application/Standard/FindStandard.cshtml", applicationId);
        //}

        [HttpPost("Standard/{applicationId}")]
        public async Task<IActionResult> Search(StandardViewModel model)
        {
            //            var results = new[]
            //            {
            //                new {StandardName = "Able Seafarer", PreRequesites = "<b>Some pre-reqs</b><ul><li>Must do this...</li></ul>"},
            //                new {StandardName = "Lion tamer", PreRequesites = "<b>Different pre-reqs</b><ul><li>Maybe this...</li><li>And this...</li></ul>"},
            //            };

        if (string.IsNullOrEmpty(model.StandardToFind) || model.StandardToFind.Length < 2)
            {
                ModelState.AddModelError(nameof(model.StandardToFind), "Enter a valid search string (more than 2 characters)");
                TempData["ShowErrors"] = true;
                return RedirectToAction(nameof(Index));
            }

            var results = await _assessorServiceApiClient.GetStandards();

            model.Results = results.Where(r => r.Title.ToLower().Contains(model.StandardToFind.ToLower())).ToList();

            return View("~/Views/Application/Standard/FindStandardResults.cshtml", model);


        }


        [HttpGet("Standard/confirm-standard/{applicationId}/standard/{standardCode}")]
        public async Task<IActionResult> Index(Guid applicationId, int standardCode)
        {
            var standardViewModel = new StandardViewModel { ApplicationId = applicationId, StandardCode = standardCode};
            var results = await _assessorServiceApiClient.GetStandards();
            standardViewModel.SelectedStandard = results.FirstOrDefault(r => r.StandardId == standardCode);

            return View("~/Views/Application/Standard/ConfirmStandard.cshtml", standardViewModel);
        }
    }
}