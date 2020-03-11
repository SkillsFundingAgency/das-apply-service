using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class EvaluateGatewayHandler : IRequestHandler<EvaluateGatewayRequest>
    {
        private readonly IApplyRepository _applyRepository;

        public EvaluateGatewayHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<Unit> Handle(EvaluateGatewayRequest request, CancellationToken cancellationToken)
        {
            await _applyRepository.EvaluateGateway(request.ApplicationId, request.IsGatewayApproved, request.EvaluatedBy);

            return Unit.Value;
        }
    }
}
