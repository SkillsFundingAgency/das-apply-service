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
        Task CreateGateway(Guid id, Guid applicationId, string status, string applicationStatus, DateTime createdAt, string createdBy, DateTime assignedAt, string assignedTo, string assignedToName);
        Task<GatewayCounts> GetGatewayCountsAsync();
        Task<Domain.Entities.Review.Gateway> GetGatewayReviewAsync(Guid applicationId);
        Task UpdateGatewayOutcomesAsync(Guid applicationId, string userId, DateTime changedAt, List<Outcome> outcomesDelta);
        Task<List<Domain.Entities.Review.Gateway>> GetGatewayInProgressAsync();
    }
}
