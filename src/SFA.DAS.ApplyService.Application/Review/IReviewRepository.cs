using SFA.DAS.ApplyService.Domain.Review.Gateway;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Review
{
    public interface IReviewRepository
    {
        Task<List<Domain.Entities.Application>> GetSubmittedApplicationsAsync();
        Task<GatewayCounts> GetGatewayCountsAsync();
    }
}
