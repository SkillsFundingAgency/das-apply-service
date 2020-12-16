using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class GetGatewayPageAnswerHandler : IRequestHandler<GetGatewayPageAnswerRequest, GatewayPageAnswer>
    {
        private readonly IApplyRepository _applyRepository;

        public GetGatewayPageAnswerHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<GatewayPageAnswer> Handle(GetGatewayPageAnswerRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.GetGatewayPageAnswer(request.ApplicationId, request.PageId);
        }
    }
}
