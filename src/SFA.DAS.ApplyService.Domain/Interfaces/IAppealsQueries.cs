using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    // TODO: APPEALREVIEW - Review once appeal work starts
    public interface IAppealsQueries
    {
        Task<AppealFiles> GetStagedAppealFiles(Guid applicationId);
        Task<Appeal> GetAppeal(Guid applicationId, Guid oversightReviewId);
    }
}
