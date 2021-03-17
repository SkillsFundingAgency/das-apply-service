using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    public interface IOversightReviewQueries
    {
        Task<PendingOversightReviews> GetPendingOversightReviews();
        Task<CompletedOversightReviews> GetCompletedOversightReviews();
        Task<ApplicationOversightDetails> GetOversightDetails(Guid applicationId);
        Task<OversightReview> GetOversightReview(Guid applicationId);
    }
}
