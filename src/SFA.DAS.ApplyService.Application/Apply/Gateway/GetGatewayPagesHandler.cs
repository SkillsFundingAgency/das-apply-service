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
        private readonly IApplyRepository _applyRepository;

        public GetGatewayPagesHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<List<GatewayPageAnswerSummary>> Handle(GetGatewayPagesRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.GetGatewayPageAnswers(request.ApplicationId);
        }
    }
}
