using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class StartGatewayReviewHandler : IRequestHandler<StartGatewayReviewRequest, bool>
    {
        private readonly IGatewayRepository _repository;

        public StartGatewayReviewHandler(IGatewayRepository applyRepository)
        {
            _repository = applyRepository;
        }

        public async Task<bool> Handle(StartGatewayReviewRequest request, CancellationToken cancellationToken)
        {
            return await _repository.StartGatewayReview(request.ApplicationId, request.UserId, request.UserName);
        }
    }
}
