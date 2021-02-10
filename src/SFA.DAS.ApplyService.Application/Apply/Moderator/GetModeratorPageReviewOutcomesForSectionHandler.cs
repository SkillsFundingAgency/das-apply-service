using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Moderator
{
    public class GetModeratorPageReviewOutcomesForSectionHandler : IRequestHandler<GetModeratorPageReviewOutcomesForSectionRequest, List<ModeratorPageReviewOutcome>>
    {
        private readonly IModeratorRepository _repository;
        private readonly ILogger<GetModeratorPageReviewOutcomesForSectionHandler> _logger;

        public GetModeratorPageReviewOutcomesForSectionHandler(IModeratorRepository repository, ILogger<GetModeratorPageReviewOutcomesForSectionHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<ModeratorPageReviewOutcome>> Handle(GetModeratorPageReviewOutcomesForSectionRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GetModeratorPageReviewOutcomesForSection for ApplicationId '{request.ApplicationId}' - " +
                                                    $"SequenceNumber '{request.SequenceNumber}' - SectionNumber '{request.SectionNumber}'");

            var moderatorPageReviewOutcomes = await _repository.GetModeratorPageReviewOutcomesForSection(request.ApplicationId,
                                                        request.SequenceNumber,
                                                        request.SectionNumber);

            return moderatorPageReviewOutcomes;
        }
    }
}
