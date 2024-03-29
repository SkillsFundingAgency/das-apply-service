﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class ResetCompleteFlagService : IResetCompleteFlagService
    {
        private readonly IQnaApiClient _qnaApiClient;

        public ResetCompleteFlagService(IQnaApiClient qnaApiClient)
        {
            _qnaApiClient = qnaApiClient;
        }

        public async Task ResetPagesComplete(Guid applicationId, string pageId)
        {
            const string entryPageForMainRoute = RoatpWorkflowPageIds.ExperienceAndAccreditations.OfficeForStudents;
            const string entryPageForEmployerRoute = RoatpWorkflowPageIds.ExperienceAndAccreditations.InitialTeacherTraining;
            const string entryPageForSupportingRoute = RoatpWorkflowPageIds.ExperienceAndAccreditations.SubcontractorDeclaration;

            var pageIdsToExcludeFromCompleteReset = new List<string>
            {
                entryPageForMainRoute,
                entryPageForEmployerRoute,
                entryPageForSupportingRoute
            };

            if (pageIdsToExcludeFromCompleteReset.Contains(pageId))
                await _qnaApiClient.ResetCompleteFlag(applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    pageIdsToExcludeFromCompleteReset);
        }
    }
}