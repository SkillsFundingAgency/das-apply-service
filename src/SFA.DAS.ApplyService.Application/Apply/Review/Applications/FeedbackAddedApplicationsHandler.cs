using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Review.Applications
{
    public class FeedbackAddedApplicationsHandler : IRequestHandler<FeedbackAddedApplicationsRequest, List<ApplicationSummaryItem>>
    {
        private readonly IApplyRepository _repository;

        public FeedbackAddedApplicationsHandler(IApplyRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ApplicationSummaryItem>> Handle(FeedbackAddedApplicationsRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetFeedbackAddedApplications();
        }
    }
}
