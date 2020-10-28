using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Clarification
{
    public class GetClarificationPageReviewOutcomeHandler : IRequestHandler<GetClarificationPageReviewOutcomeRequest, ClarificationPageReviewOutcome>
    {
        private readonly IClarificationRepository _repository;
        private readonly ILogger<GetClarificationPageReviewOutcomeHandler> _logger;

        public GetClarificationPageReviewOutcomeHandler(IClarificationRepository repository, ILogger<GetClarificationPageReviewOutcomeHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ClarificationPageReviewOutcome> Handle(GetClarificationPageReviewOutcomeRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GetClarificationPageReviewOutcome for ApplicationId '{request.ApplicationId}' - PageId '{request.PageId}'");

            var pagePageReviewOutcome = await _repository.GetClarificationPageReviewOutcome(request.ApplicationId,
                                                        request.SequenceNumber,
                                                        request.SectionNumber,
                                                        request.PageId);

            return pagePageReviewOutcome;
        }
    }
}
