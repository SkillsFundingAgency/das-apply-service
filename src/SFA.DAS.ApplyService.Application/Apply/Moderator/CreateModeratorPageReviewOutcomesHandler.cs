using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.ApplyService.Application.Apply.Moderator
{
    public class CreateModeratorPageReviewOutcomesHandler : IRequestHandler<CreateModeratorPageReviewOutcomesRequest>
    {
        private readonly IModeratorRepository _repository;
        private readonly ILogger<CreateModeratorPageReviewOutcomesHandler> _logger;

        public CreateModeratorPageReviewOutcomesHandler(IModeratorRepository repository, ILogger<CreateModeratorPageReviewOutcomesHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateModeratorPageReviewOutcomesRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CreateAssessorPageReviewOutcomes for ApplicationId '{request.ModeratorPageReviewOutcomes.First().ApplicationId}'");
            await _repository.CreateModeratorPageOutcomes(request.ModeratorPageReviewOutcomes);
            return Unit.Value;
        }
    }
}
