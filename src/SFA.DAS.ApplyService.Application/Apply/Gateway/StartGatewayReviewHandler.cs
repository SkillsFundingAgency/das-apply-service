using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class StartGatewayReviewHandler : IRequestHandler<StartGatewayReviewRequest>
    {
        private readonly IApplyRepository _applyRepository;

        public StartGatewayReviewHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<Unit> Handle(StartGatewayReviewRequest request, CancellationToken cancellationToken)
        {
            await _applyRepository.StartGatewayReview(request.ApplicationId, request.Reviewer);

            return Unit.Value;
        }
    }
}
