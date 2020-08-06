using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public interface ISectorDetailsOrchestratorService
    {
        Task<SectorDetails> GetSectorDetails(Guid applicationId, string pageId);
    }
}

