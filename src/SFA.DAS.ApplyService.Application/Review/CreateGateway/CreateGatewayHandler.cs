using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Review.CreateGateway
{
    public class CreateGatewayHandler : IRequestHandler<CreateGatewayRequest>
    {
        private readonly IReviewRepository _reviewRepository;

        public CreateGatewayHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<Unit> Handle(CreateGatewayRequest request, CancellationToken cancellationToken)
        {
            await _reviewRepository.CreateGateway(
                request.Id,
                request.ApplicationId,
                request.Status,
                request.ApplicationStatus,
                request.CreatedAt,
                request.CreatedBy,
                request.AssignedAt,
                request.AssignedTo,
                request.AssignedToName);

            return Unit.Value;
        }
    }
}
