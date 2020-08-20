using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class GetAssessorReviewOutcomesPerSectionHandler : IRequestHandler<GetAssessorReviewOutcomesPerSectionRequest, List<PageReviewOutcome>>
    {
        private readonly IAssessorRepository _repository;
        private readonly ILogger<GetAssessorReviewOutcomesPerSectionHandler> _logger;

        public GetAssessorReviewOutcomesPerSectionHandler(IAssessorRepository repository, ILogger<GetAssessorReviewOutcomesPerSectionHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<PageReviewOutcome>> Handle(GetAssessorReviewOutcomesPerSectionRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GetAssessorReviewOutcomesPerSection for ApplicationId '{request.ApplicationId}' - " +
                                                    $"SequenceNumber '{request.SequenceNumber}' - SectionNumber '{request.SectionNumber}' - " +
                                                    $"AssessorType '{request.AssessorType}' - UserId '{request.UserId}'");

            var assessorReviewOutcomes = await _repository.GetAssessorReviewOutcomesPerSection(request.ApplicationId,
                                                        request.SequenceNumber,
                                                        request.SectionNumber,
                                                        request.AssessorType,
                                                        request.UserId);

            return assessorReviewOutcomes;
        }
    }
}
