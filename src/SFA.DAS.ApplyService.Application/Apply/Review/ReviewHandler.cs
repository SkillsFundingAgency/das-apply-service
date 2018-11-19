using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Review
{
    public class ReviewHandler : IRequestHandler<ReviewRequest, List<Domain.Entities.Application>>
    {
        private readonly IApplyRepository _repository;

        public ReviewHandler(IApplyRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<List<Domain.Entities.Application>> Handle(ReviewRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetApplicationsToReview();
        }
    }
}