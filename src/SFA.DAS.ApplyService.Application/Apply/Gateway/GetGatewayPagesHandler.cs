using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class GetGatewayPagesHandler : IRequestHandler<GetGatewayPagesRequest, List<GatewayPageAnswerSummary>>
    {
        private readonly IGatewayRepository _repository;

        public GetGatewayPagesHandler(IGatewayRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GatewayPageAnswerSummary>> Handle(GetGatewayPagesRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetGatewayPageAnswers(request.ApplicationId);
        }
    }
}
