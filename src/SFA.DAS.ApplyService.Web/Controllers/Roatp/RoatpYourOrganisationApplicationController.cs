
using SFA.DAS.ApplyService.Web.Infrastructure.Interfaces;

namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    using System;
    using System.Threading.Tasks;
    using Application.Apply.Roatp;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Authorize]
    public class RoatpYourOrganisationApplicationController : Controller
    {
        private readonly ILogger<RoatpYourOrganisationApplicationController> _logger;
        private readonly IProcessPageFlowService _processPageFlowService;

        public RoatpYourOrganisationApplicationController(ILogger<RoatpYourOrganisationApplicationController> logger,
             IProcessPageFlowService processPageFlowService)
        {
            _logger = logger;
            _processPageFlowService = processPageFlowService;
        }
        
        public async Task<IActionResult> WhatYouWillNeed(Guid applicationId)
        {
            var pageId = await GetIntroductionPageForApplication(applicationId);

            return RedirectToAction("Page", "RoatpApplication",
                new
                {
                    applicationId = applicationId,
                    sequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                    sectionId = RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed,
                    pageId = pageId,
                    redirectAction = "TaskList"
                });
        }

        public async Task<IActionResult> ExperienceAccreditation(Guid applicationId)
        {
            // TECH DEBT: This should be driven from QnA Config and not be hard coded like this.
            var providerTypeId = await _processPageFlowService.GetApplicationProviderTypeId(applicationId);

            string startingPage;
            switch (providerTypeId) 
            {
                case 3:
                    startingPage = RoatpWorkflowPageIds.ExperienceAndAccreditations.SubcontractorDeclaration;
                    break;
                case 2:
                    startingPage = RoatpWorkflowPageIds.ExperienceAndAccreditations.InitialTeacherTraining;
                    break;
                default:
                    startingPage = RoatpWorkflowPageIds.ExperienceAndAccreditations.OfficeForStudents;
                    break;
            }
            
            return RedirectToAction("Page", "RoatpApplication",
                new
                {
                    applicationId = applicationId,
                    sequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                    sectionId = RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    pageId = startingPage,
                    redirectAction = "TaskList"
                });
        }

        private async Task<string> GetIntroductionPageForApplication(Guid applicationId)
        {
            var providerTypeId = await _processPageFlowService.GetApplicationProviderTypeId(applicationId);

            var introductionPageId = await
                _processPageFlowService.GetIntroductionPageIdForSequence(RoatpWorkflowSequenceIds.YourOrganisation, providerTypeId);

            return introductionPageId ?? RoatpWorkflowPageIds.YourOrganisationIntroductionMain;
        }
    }
}
