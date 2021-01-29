using MediatR;
using SFA.DAS.ApplyService.InternalApi.Types.QueryResults;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class GetOversightsPendingRequest : IRequest<PendingOversightReviews>
    {
    }
}
