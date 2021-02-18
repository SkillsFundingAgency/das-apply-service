using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    public interface IAppealsQueries
    {
        Task<AppealFiles> GetStagedAppealFiles(Guid applicationId);
    }
}
