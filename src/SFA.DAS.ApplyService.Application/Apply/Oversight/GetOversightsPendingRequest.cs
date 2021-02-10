using MediatR;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class GetOversightsPendingRequest : IRequest<PendingOversightReviews>
    {
    }
}
