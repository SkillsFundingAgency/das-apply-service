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
        private readonly IApplicationApiClient _apiClient;

        public StandardController(IApplicationApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpGet("Standard/{applicationId}")]
        public async Task<IActionResult> Index(Guid applicationId)
        {
            var standardViewModel = new StandardViewModel {ApplicationId = applicationId};
            return View("~/Views/Application/Standard/FindStandard.cshtml", standardViewModel);
        }

        [HttpPost("Standard/{applicationId}")]
        public async Task<IActionResult> Search(Guid applicationId, string standardToFind, StandardViewModel model)
        {
            // TODO: Check standard is valid.

            var standardApplicationData = new StandardApplicationData {StandardName = standardToFind};

            await _apiClient.UpdateApplicationData(standardApplicationData, applicationId);

            //var sequence = _apiClient.GetSequence(applicationId, Guid.Parse(User.FindFirstValue("UserId")));

            return RedirectToAction("Sequence", "Application", new {applicationId});

            //return View("~/Views/Application/Standard/FindStandard.cshtml", applicationId);
        }

        [HttpGet("Standard/Search/{search}")]
        public async Task<ActionResult> Search(string search)
        {
            var results = new[]
            {
                new {StandardName = "Able Seafarer", PreRequesites = "<b>Some pre-reqs</b><ul><li>Must do this...</li></ul>"},
                new {StandardName = "Lion tamer", PreRequesites = "<b>Different pre-reqs</b><ul><li>Maybe this...</li><li>And this...</li></ul>"},
            };

            results = results.Where(r => r.StandardName.ToLower().Contains(search.ToLower())).ToArray();
            
            return Ok(results);
        }
    }
}