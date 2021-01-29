using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class GetAllAssessorPageReviewOutcomesHandler : IRequestHandler<GetAllAssessorPageReviewOutcomesRequest, List<AssessorPageReviewOutcome>>
    {
        private readonly IAssessorRepository _repository;
        private readonly ILogger<GetAllAssessorPageReviewOutcomesHandler> _logger;

        public GetAllAssessorPageReviewOutcomesHandler(IAssessorRepository repository, ILogger<GetAllAssessorPageReviewOutcomesHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<AssessorPageReviewOutcome>> Handle(GetAllAssessorPageReviewOutcomesRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GetAllAssessorPageReviewOutcomes for ApplicationId '{request.ApplicationId}'");

            var assessorPageReviewOutcomes = await _repository.GetAllAssessorPageReviewOutcomes(request.ApplicationId, request.UserId);

            return assessorPageReviewOutcomes;
        }
    }
}
