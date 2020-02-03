using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway.Applications
{
    public class InProgressGatewayApplicationsHandler : IRequestHandler<InProgressGatewayApplicationsRequest, List<RoatpApplicationSummaryItem>>
    {
        private readonly IApplyRepository _repository;

        public InProgressGatewayApplicationsHandler(IApplyRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RoatpApplicationSummaryItem>> Handle(InProgressGatewayApplicationsRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetInProgressGatewayApplications();
        }
    }
}
