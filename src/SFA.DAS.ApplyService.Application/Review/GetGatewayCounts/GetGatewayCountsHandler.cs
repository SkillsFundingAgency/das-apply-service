using MediatR;
using SFA.DAS.ApplyService.Domain.Review.Gateway;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Review.GetGatewayCounts
{
    public class GetGatewayCountsHandler : IRequestHandler<GetGatewayCountsRequest, GatewayCounts>
    {
        private readonly IReviewRepository _reviewRepository;

        public GetGatewayCountsHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public Task<GatewayCounts> Handle(GetGatewayCountsRequest request, CancellationToken cancellationToken)
        {
            return _reviewRepository.GetGatewayCountsAsync();
        }
    }
}
