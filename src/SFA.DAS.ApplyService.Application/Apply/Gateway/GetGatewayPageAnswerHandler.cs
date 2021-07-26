using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class GetGatewayPageAnswerHandler : IRequestHandler<GetGatewayPageAnswerRequest, GatewayPageAnswer>
    {
        private readonly IGatewayRepository _repository;

        public GetGatewayPageAnswerHandler(IGatewayRepository repository)
        {
            _repository = repository;
        }

        public async Task<GatewayPageAnswer> Handle(GetGatewayPageAnswerRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetGatewayPageAnswer(request.ApplicationId, request.PageId);
        }
    }
}
