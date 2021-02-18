using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Clarification
{
    public class GetAllClarificationPageReviewOutcomesHandler : IRequestHandler<GetAllClarificationPageReviewOutcomesRequest, List<ClarificationPageReviewOutcome>>
    {
        private readonly IClarificationRepository _repository;
        private readonly ILogger<GetAllClarificationPageReviewOutcomesHandler> _logger;

        public GetAllClarificationPageReviewOutcomesHandler(IClarificationRepository repository, ILogger<GetAllClarificationPageReviewOutcomesHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<ClarificationPageReviewOutcome>> Handle(GetAllClarificationPageReviewOutcomesRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GetClarificationPageReviewOutcomes for ApplicationId '{request.ApplicationId}'");

            var pageReviewOutcomes = await _repository.GetAllClarificationPageReviewOutcomes(request.ApplicationId);

            return pageReviewOutcomes;
        }
    }
}
