using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Review.GetSubmittedApplications
{
    public class GetSubmittedApplicationsHandler : IRequestHandler<GetSubmittedApplicationsRequest, List<Domain.Entities.Application>>
    {
        private readonly IReviewRepository _reviewRepository;

        public GetSubmittedApplicationsHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public Task<List<Domain.Entities.Application>> Handle(GetSubmittedApplicationsRequest request, CancellationToken cancellationToken)
        {
            return _reviewRepository.GetSubmittedApplicationsAsync();
        }
    }
}
