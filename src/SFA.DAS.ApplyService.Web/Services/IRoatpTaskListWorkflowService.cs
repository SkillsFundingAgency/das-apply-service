using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface IRoatpTaskListWorkflowService
    {
        string SectionStatus(IEnumerable<ApplicationSequence> applicationSequences, int sequenceId, int sectionId,
            string applicationRouteId, bool sequential = false);

        bool PreviousSectionCompleted(ApplicationSequence sequence, int sectionId, bool sequential);
    }
}
