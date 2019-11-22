using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Review.UpdateGatewayOutcomes
{
    public class UpdateGatewayOutcomesHandler : IRequestHandler<UpdateGatewayOutcomesCommand>
    {
        private readonly IReviewRepository _reviewRepository;

        public UpdateGatewayOutcomesHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<Unit> Handle(UpdateGatewayOutcomesCommand request, CancellationToken cancellationToken)
        {
            await _reviewRepository.UpdateGatewayOutcomesAsync(
                request.ApplicationId,
                request.UserId,
                request.ChangedAt,
                request.OutcomesDelta);

            return Unit.Value;
        }
    }
}
