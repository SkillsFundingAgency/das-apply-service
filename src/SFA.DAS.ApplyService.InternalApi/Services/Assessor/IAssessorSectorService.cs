using SFA.DAS.ApplyService.InternalApi.Types.Assessor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Services.Assessor
{
    public interface IAssessorSectorService
    {
        Task<List<AssessorSector>> GetSectorsForAssessor(Guid applicationId, string userId);
        Task<List<AssessorSector>> GetSectorsForModerator(Guid applicationId, string userId);

        List<AssessorSector> GetSectorsForEmptyReview(AssessorSection section);
    }
}