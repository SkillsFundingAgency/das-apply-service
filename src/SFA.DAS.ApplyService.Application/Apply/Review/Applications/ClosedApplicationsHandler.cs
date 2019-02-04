using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Review.Applications
{
    public class ClosedApplicationsHandler : IRequestHandler<ClosedApplicationsRequest, List<ApplicationSummaryItem>>
    {
        private readonly IApplyRepository _repository;

        public ClosedApplicationsHandler(IApplyRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ApplicationSummaryItem>> Handle(ClosedApplicationsRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetClosedApplications();
        }
    }
}
