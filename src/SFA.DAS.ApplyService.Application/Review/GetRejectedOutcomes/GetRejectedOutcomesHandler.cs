using MediatR;
using SFA.DAS.ApplyService.Domain.Review;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Review.GetRejectedOutcomes
{
    class GetRejectedOutcomesHandler : IRequestHandler<GetRejectedOutcomesRequest, List<Outcome>>
    {
        private readonly IReviewRepository _reviewRepository;

        public GetRejectedOutcomesHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<List<Outcome>> Handle(GetRejectedOutcomesRequest request, CancellationToken cancellationToken)
        {
            var gatewayReview = await _reviewRepository.GetGatewayReviewAsync(request.ApplicationId);

            if (gatewayReview.Outcomes == null)
                return new List<Outcome>();

            var rejectedOutcomes = gatewayReview.Outcomes.Where(o => 
                o.Result == "Reject" &&
                o.SectionId == request.SectionId &&
                o.PageId == request.PageId
                ).ToList();

            return rejectedOutcomes;
        }
    }
}
