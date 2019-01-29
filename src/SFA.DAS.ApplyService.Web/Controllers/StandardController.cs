using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    [Authorize]
    public class StandardController : Controller
    {
        private readonly IApplicationApiClient _apiClient;
        private readonly ISessionService _sessionService;
        public StandardController(IApplicationApiClient apiClient, ISessionService sessionService)
        {
            _apiClient = apiClient;
            _sessionService = sessionService;
        }

        [HttpGet("Standard/{applicationId}")]
        public async Task<IActionResult> Index(Guid applicationId)
        {
            var standardViewModel = new StandardViewModel {ApplicationId = applicationId};
            return View("~/Views/Application/Standard/FindStandard.cshtml", standardViewModel);
        }

        [HttpPost("Standard/{applicationId}")]
        public async Task<IActionResult> Search(StandardViewModel model)
        {
        if (string.IsNullOrEmpty(model.StandardToFind) || model.StandardToFind.Length < 2)
            {
                ModelState.AddModelError(nameof(model.StandardToFind), "Enter a valid search string (more than 2 characters)");
                TempData["ShowErrors"] = true;
                return RedirectToAction(nameof(Index));
            }

            var results = await _apiClient.GetStandards();

            model.Results = results.Where(r => r.Title.ToLower().Contains(model.StandardToFind.ToLower())).ToList();

            return View("~/Views/Application/Standard/FindStandardResults.cshtml", model);
        }

        [HttpGet("standard/{applicationId}/confirm-standard/{standardCode}")]
        public async Task<IActionResult> StandardConfirm(Guid applicationId, int standardCode)
        {
            var standardViewModel = new StandardViewModel { ApplicationId = applicationId, StandardCode = standardCode};
            var results = await _apiClient.GetStandards();
            standardViewModel.SelectedStandard = results.FirstOrDefault(r => r.StandardId == standardCode);
            standardViewModel.ApplicationStatus = await _apiClient.GetApplicationStatus(applicationId, standardCode);
            return View("~/Views/Application/Standard/ConfirmStandard.cshtml", standardViewModel);
        }

        [HttpPost("standard/{applicationId}/confirm-standard/{standardCode}")]
        public async Task<IActionResult> StandardConfirm(StandardViewModel model, Guid applicationId, int standardCode)
        {
            var results = await _apiClient.GetStandards();
            model.SelectedStandard = results.FirstOrDefault(r => r.StandardId == standardCode);

            model.ApplicationStatus = await _apiClient.GetApplicationStatus(applicationId, standardCode);

            if (!model.IsConfirmed)
            {
                ModelState.AddModelError(nameof(model.IsConfirmed), "Please tick to confirm");
                TempData["ShowErrors"] = true;
                return View("~/Views/Application/Standard/ConfirmStandard.cshtml", model);
            }

            if (!string.IsNullOrEmpty(model.ApplicationStatus))
            {
                return View("~/Views/Application/Standard/ConfirmStandard.cshtml", model);
            }
            var applicationData =
                new StandardApplicationData
                {
                    StandardName = model.SelectedStandard?.Title,
                    StandardCode = standardCode,
                    UserEmail = User.GetEmail()
                };

            await _apiClient.UpdateApplicationData(applicationData, model.ApplicationId);

            return RedirectToAction("Applications", "Application");
        }
    }
}