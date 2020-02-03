using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway.Applications
{
    public class ClosedGatewayApplicationsHandler : IRequestHandler<ClosedGatewayApplicationsRequest, List<RoatpApplicationSummaryItem>>
    {
        private readonly IApplyRepository _repository;

        public ClosedGatewayApplicationsHandler(IApplyRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RoatpApplicationSummaryItem>> Handle(ClosedGatewayApplicationsRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetClosedGatewayApplications();
        }
    }
}
