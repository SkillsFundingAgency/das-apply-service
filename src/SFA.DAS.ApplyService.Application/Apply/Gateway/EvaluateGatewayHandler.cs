using MediatR;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class EvaluateGatewayHandler : IRequestHandler<EvaluateGatewayRequest, bool>
    {
        private readonly IGatewayRepository _repository;

        public EvaluateGatewayHandler(IGatewayRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(EvaluateGatewayRequest request, CancellationToken cancellationToken)
        {
            return await _repository.EvaluateGateway(request.ApplicationId, request.IsGatewayApproved, request.UserId, request.UserName);
        }
    }
}
