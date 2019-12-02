using MediatR;
using SFA.DAS.ApplyService.Domain.Entities.Review;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Review.GetGatewayInProgress
{
    public class GetGatewayInProgressHandler : IRequestHandler<GetGatewayInProgressRequest, List<Gateway>>
    {
        private readonly IReviewRepository _reviewRepository;

        public GetGatewayInProgressHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public Task<List<Gateway>> Handle(GetGatewayInProgressRequest request, CancellationToken cancellationToken)
        {
            return _reviewRepository.GetGatewayInProgressAsync();
        }
    }
}
