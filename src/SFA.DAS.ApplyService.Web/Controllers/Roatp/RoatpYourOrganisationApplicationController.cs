
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
        private readonly IQnaApiClient _qnaApiClient;
        private readonly IOrganisationApiClient _organisationApiClient;

        public RoatpYourOrganisationApplicationController(ILogger<RoatpYourOrganisationApplicationController> logger,
            IQnaApiClient qnaApiClient)
        {
            _logger = logger;
            _qnaApiClient = qnaApiClient;
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
            int providerTypeId = 1;
            string pageId = RoatpWorkflowPageIds.YourOrganisationIntroductionMain;

            var providerTypeAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.ProviderRoute);
            if (providerTypeAnswer != null && !String.IsNullOrWhiteSpace(providerTypeAnswer.Value))
            {
                int.TryParse(providerTypeAnswer.Value, out providerTypeId);
            }

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
