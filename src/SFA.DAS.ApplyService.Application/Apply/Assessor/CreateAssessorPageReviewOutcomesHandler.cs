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

            foreach (var outcome in request.AssessorPageReviewOutcomes)
            {
                await _repository.SubmitAssessorPageOutcome(outcome.ApplicationId,
                    outcome.SequenceNumber,
                    outcome.SectionNumber,
                    outcome.PageId,
                    outcome.UserId,
                    outcome.Status,
                    outcome.Comment);
            }

            return Unit.Value;
        }
    }
}
