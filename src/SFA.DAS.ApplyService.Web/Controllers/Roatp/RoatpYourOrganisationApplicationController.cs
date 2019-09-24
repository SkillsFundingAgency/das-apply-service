
using SFA.DAS.ApplyService.Web.Infrastructure.Interfaces;

namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    using System;
    using System.Threading.Tasks;
    using Application.Apply.Roatp;
    using Domain.Roatp;
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Authorize]
    public class RoatpYourOrganisationApplicationController : Controller
    {
        private readonly ILogger<RoatpYourOrganisationApplicationController> _logger;
        private readonly IOrganisationApiClient _organisationApiClient;
        private readonly IProcessPageFlowService _processPageFlowService;
        private const int Sequence1Id = 1;

        public RoatpYourOrganisationApplicationController(ILogger<RoatpYourOrganisationApplicationController> logger,
             IProcessPageFlowService processPageFlowService)
        {
            _logger = logger;
            _processPageFlowService = processPageFlowService;
        }

        public async Task<IActionResult> ProviderRoute(Guid applicationId)
        {
            return RedirectToAction("Page", "RoatpApplication",
                new
                {
                    applicationId = applicationId,
                    sequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                    sectionId = RoatpWorkflowSectionIds.YourOrganisation.ProviderRoute,
                    pageId = RoatpWorkflowPageIds.YourOrganisation,
                    redirectAction = "TaskList"
                });
        }

        public async Task<IActionResult> WhatYouWillNeed(Guid applicationId)
        {
            var pageId = await GetIntroductionPageForApplication(applicationId);

            return RedirectToAction("Page", "RoatpApplication",
                new
                {
                    applicationId = applicationId, sequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                    sectionId = RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed, pageId = pageId,
                    redirectAction = "TaskList"
                });
        }

        private async Task<string> GetIntroductionPageForApplication(Guid applicationId)
        {
            var providerTypeId = await _processPageFlowService.GetApplicationProviderTypeId(applicationId);

            var introductionPageId = await
                _processPageFlowService.GetIntroductionPageIdForSequence(applicationId, Sequence1Id, providerTypeId);
            if (introductionPageId != null)
                return await Task.FromResult(introductionPageId);

            return await Task.FromResult(RoatpWorkflowPageIds.YourOrganisationIntroductionMain);

        }
    }
}
