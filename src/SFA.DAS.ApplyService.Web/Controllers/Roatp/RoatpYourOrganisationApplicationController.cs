
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
        private readonly IGetCurrentApplicationDetailsService _getApplicationDetailsService;

        public RoatpYourOrganisationApplicationController(ILogger<RoatpYourOrganisationApplicationController> logger,
             IGetCurrentApplicationDetailsService getApplicationDetailsService)
        {
            _logger = logger;
            _getApplicationDetailsService = getApplicationDetailsService;
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
            var providerTypeId = await _getApplicationDetailsService.GetProviderTypeId(applicationId);

            string pageId = RoatpWorkflowPageIds.YourOrganisationIntroductionMain;

            switch (providerTypeId)
            {
                case ApplicationRoute.MainProviderApplicationRoute:
                    pageId = RoatpWorkflowPageIds.YourOrganisationIntroductionMain;
                    break;
                case ApplicationRoute.EmployerProviderApplicationRoute:
                    pageId = RoatpWorkflowPageIds.YourOrganisationIntroductionEmployer;
                    break;
                case ApplicationRoute.SupportingProviderApplicationRoute:
                    pageId = RoatpWorkflowPageIds.YourOrganisationIntroductionSupporting;
                    break;
            }

            return await Task.FromResult(pageId);
        }
    }
}
