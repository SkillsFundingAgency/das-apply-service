using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.Services.Assessor
{
    public interface IAssessorSectorDetailsService
    {
        Task<AssessorSectorDetails> GetSectorDetails(Guid applicationId, string pageId);
    }
}

