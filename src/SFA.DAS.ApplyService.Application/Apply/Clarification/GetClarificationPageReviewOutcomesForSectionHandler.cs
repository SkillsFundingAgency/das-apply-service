using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Clarification
{
    public class GetClarificationPageReviewOutcomesForSectionHandler : IRequestHandler<GetClarificationPageReviewOutcomesForSectionRequest, List<ClarificationPageReviewOutcome>>
    {
        private readonly IClarificationRepository _repository;
        private readonly ILogger<GetClarificationPageReviewOutcomesForSectionHandler> _logger;

        public GetClarificationPageReviewOutcomesForSectionHandler(IClarificationRepository repository, ILogger<GetClarificationPageReviewOutcomesForSectionHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<ClarificationPageReviewOutcome>> Handle(GetClarificationPageReviewOutcomesForSectionRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetClarificationPageReviewOutcomesForSection for ApplicationId '{ApplicationId}' - SequenceNumber '{SequenceNumber}' - SectionNumber '{SectionNumber}'", request.ApplicationId, request.SequenceNumber, request.SectionNumber);

            var clarificationPageReviewOutcomes = await _repository.GetClarificationPageReviewOutcomesForSection(request.ApplicationId,
                                                        request.SequenceNumber,
                                                        request.SectionNumber);

            return clarificationPageReviewOutcomes;
        }
    }
}
