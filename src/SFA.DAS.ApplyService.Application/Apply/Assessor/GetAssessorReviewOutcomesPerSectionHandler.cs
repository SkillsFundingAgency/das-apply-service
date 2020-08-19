using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class GetAssessorReviewOutcomesPerSectionHandler : IRequestHandler<GetAssessorReviewOutcomesPerSectionRequest, List<AssessorPageReviewOutcome>>
    {
        private readonly IAssessorRepository _repository;
        private readonly ILogger<GetAssessorReviewOutcomesPerSectionHandler> _logger;

        public GetAssessorReviewOutcomesPerSectionHandler(IAssessorRepository repository, ILogger<GetAssessorReviewOutcomesPerSectionHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<AssessorPageReviewOutcome>> Handle(GetAssessorReviewOutcomesPerSectionRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GetAssessorReviewOutcomesPerSection for ApplicationId '{request.ApplicationId}' - " +
                                                    $"SequenceNumber '{request.SequenceNumber}' - SectionNumber '{request.SectionNumber}' - " +
                                                    $"AssessorType '{request.AssessorType}' - UserId '{request.UserId}'");

            var assessorPageReviewOutcomes = await _repository.GetAssessorPageReviewOutcomesForSection(request.ApplicationId,
                                                        request.SequenceNumber,
                                                        request.SectionNumber,
                                                        request.AssessorType,
                                                        request.UserId);

            return assessorPageReviewOutcomes;
        }
    }
}
