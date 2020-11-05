using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.ApplyService.Application.Apply.Moderator
{
    public class CreateEmptyModeratorReviewHandler : IRequestHandler<CreateEmptyModeratorReviewRequest>
    {
        private readonly IModeratorRepository _repository;
        private readonly ILogger<CreateEmptyModeratorReviewHandler> _logger;

        public CreateEmptyModeratorReviewHandler(IModeratorRepository repository, ILogger<CreateEmptyModeratorReviewHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateEmptyModeratorReviewRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CreateEmptyModeratorReview for ApplicationId '{request.ApplicationId}'");
            await _repository.CreateEmptyModeratorReview(request.ApplicationId, request.ModeratorUserId, request.ModeratorUserName, request.PageReviewOutcomes);
            return Unit.Value;
        }
    }
}
