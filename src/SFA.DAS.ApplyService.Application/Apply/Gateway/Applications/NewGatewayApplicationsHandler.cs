using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway.Applications
{
    public class NewGatewayApplicationsHandler : IRequestHandler<NewGatewayApplicationsRequest, List<GatewayApplicationSummaryItem>>
    {
        private readonly IApplyRepository _repository;

        public NewGatewayApplicationsHandler(IApplyRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GatewayApplicationSummaryItem>> Handle(NewGatewayApplicationsRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetNewGatewayApplications();
        }
    }
}
