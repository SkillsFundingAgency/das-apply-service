using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class CreateAssessorPageReviewOutcomesHandler : IRequestHandler<CreateAssessorPageReviewOutcomesRequest>
    {
        private readonly IAssessorRepository _repository;
        private readonly ILogger<CreateAssessorPageReviewOutcomesHandler> _logger;

        public CreateAssessorPageReviewOutcomesHandler(IAssessorRepository repository, ILogger<CreateAssessorPageReviewOutcomesHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateAssessorPageReviewOutcomesRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CreateAssessorPageReviewOutcomes for ApplicationId '{request.AssessorPageReviewOutcomes.First().ApplicationId}'");
            await _repository.CreateAssessorPageOutcomes(request.AssessorPageReviewOutcomes);
            return Unit.Value;
        }
    }
}
