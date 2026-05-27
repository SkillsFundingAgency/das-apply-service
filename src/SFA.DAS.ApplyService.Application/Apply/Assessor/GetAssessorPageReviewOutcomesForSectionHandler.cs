using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class GetAssessorPageReviewOutcomesForSectionHandler : IRequestHandler<GetAssessorPageReviewOutcomesForSectionRequest, List<AssessorPageReviewOutcome>>
    {
        private readonly IAssessorRepository _repository;
        private readonly ILogger<GetAssessorPageReviewOutcomesForSectionHandler> _logger;

        public GetAssessorPageReviewOutcomesForSectionHandler(IAssessorRepository repository, ILogger<GetAssessorPageReviewOutcomesForSectionHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<AssessorPageReviewOutcome>> Handle(GetAssessorPageReviewOutcomesForSectionRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetAssessorPageReviewOutcomesForSection for ApplicationId '{ApplicationId}' - SequenceNumber '{SequenceNumber}' - SectionNumber '{SectionNumber}'", request.ApplicationId, request.SequenceNumber, request.SectionNumber);

            var assessorPageReviewOutcomes = await _repository.GetAssessorPageReviewOutcomesForSection(request.ApplicationId,
                                                        request.SequenceNumber,
                                                        request.SectionNumber,
                                                        request.UserId);

            return assessorPageReviewOutcomes;
        }
    }
}
