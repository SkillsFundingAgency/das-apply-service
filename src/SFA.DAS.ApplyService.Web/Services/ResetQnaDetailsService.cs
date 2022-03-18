using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class ResetQnaDetailsService : IResetQnaDetailsService
    {
        private readonly IQnaApiClient _qnaApiClient;

        public ResetQnaDetailsService(IQnaApiClient qnaApiClient)
        {
            _qnaApiClient = qnaApiClient;
        }

        public async Task ResetPagesComplete(Guid applicationId, int sequenceNo, int sectionNo, string pageId)
        {
            await ResetSection1_5(applicationId, sequenceNo, sectionNo, pageId);
        }

        private async Task ResetSection1_5(Guid applicationId, int sequenceNo, int sectionNo, string pageId)
        {
            if (sequenceNo == RoatpWorkflowSequenceIds.YourOrganisation &&
                sectionNo == RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations)
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
                    await _qnaApiClient.ResetSectionPagesIncomplete(applicationId,
                        RoatpWorkflowSequenceIds.YourOrganisation,
                        RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                        pageIdsToExcludeFromCompleteReset);
            }
        }
    }
}