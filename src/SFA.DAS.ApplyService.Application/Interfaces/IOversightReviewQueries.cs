using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.InternalApi.Types.QueryResults;

namespace SFA.DAS.ApplyService.Application.Interfaces
{
    public interface IOversightReviewQueries
    {
        Task<PendingOversightReviews> GetPendingOversightReviews();
        Task<CompletedOversightReviews> GetCompletedOversightReviews();
        Task<ApplicationOversightDetails> GetOversightDetails(Guid applicationId);
    }
}
