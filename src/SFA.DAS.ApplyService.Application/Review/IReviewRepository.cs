using SFA.DAS.ApplyService.Domain.Entities.Review;
using SFA.DAS.ApplyService.Domain.Review;
using SFA.DAS.ApplyService.Domain.Review.Gateway;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Review
{
    public interface IReviewRepository
    {
        Task<List<Domain.Entities.Application>> GetSubmittedApplicationsAsync();
        Task<GatewayCounts> GetGatewayCountsAsync();
        Task<Gateway> GetGatewayReviewAsync(Guid applicationId);
        Task UpdateGatewayOutcomesAsync(Guid applicationId, string userId, DateTime changedAt, List<Outcome> outcomesDelta);

    }
}
