using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface IRoatpTaskListWorkflowService
    {
        string SectionStatus(Guid applicationId, int sequenceId, int sectionId, IEnumerable<ApplicationSequence> applicationSequences, OrganisationVerificationStatus organisationVerificationStatus);
        Task<string> FinishSectionStatus(Guid applicationId, int sectionId, IEnumerable<ApplicationSequence> applicationSequences, bool applicationSequencesCompleted);
        bool PreviousSectionCompleted(Guid applicationId, int sequenceId, int sectionId, IEnumerable<ApplicationSequence> applicationSequences, OrganisationVerificationStatus organisationVerificationStatus);
        Task<IEnumerable<ApplicationSequence>> GetApplicationSequences(Guid applicationId);
        string SectionQuestionsStatus(Guid applicationId, int sequenceId, int sectionId, IEnumerable<ApplicationSequence> applicationSequences);

        Task RefreshNotRequiredOverrides(Guid applicationId);
        Task<bool> SectionNotRequired(Guid applicationId, int sequenceId, int sectionId);
    }
}
