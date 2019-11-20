using MediatR;
using SFA.DAS.ApplyService.Domain.Review.Gateway;

namespace SFA.DAS.ApplyService.Application.Review.GetGatewayCounts
{
    public class GetGatewayCountsRequest : IRequest<GatewayCounts>
    {
    }
}
