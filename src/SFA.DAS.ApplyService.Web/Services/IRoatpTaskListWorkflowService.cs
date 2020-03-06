using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface IRoatpTaskListWorkflowService
    {
        string SectionStatus(Guid applicationId, int sequenceId, int sectionId, List<ApplicationSequence> applicationSequences);
        bool PreviousSectionCompleted(Guid applicationId, int sequenceId, int sectionId, List<ApplicationSequence> applicationSequences);
        bool ApplicationSequencesCompleted(Guid applicationId, List<ApplicationSequence> applicationSequences);
        List<ApplicationSequence> GetApplicationSequences(Guid applicationId);
    }
}
