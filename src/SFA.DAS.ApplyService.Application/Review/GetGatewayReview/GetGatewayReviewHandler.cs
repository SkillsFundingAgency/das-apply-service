using MediatR;
using SFA.DAS.ApplyService.Domain.Entities.Review;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Review.GetGatewayReview
{
    public class GetGatewayReviewHandler : IRequestHandler<GetGatewayReviewRequest, Gateway>
    {
        private readonly IReviewRepository _reviewRepository;

        public GetGatewayReviewHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public Task<Gateway> Handle(GetGatewayReviewRequest request, CancellationToken cancellationToken)
        {
            return _reviewRepository.GetGatewayReviewAsync(request.ApplicationId);
        }
    }
}
