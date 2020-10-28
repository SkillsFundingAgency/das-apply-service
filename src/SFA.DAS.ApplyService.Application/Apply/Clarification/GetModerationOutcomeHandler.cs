using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Clarification
{
    public class GetModerationOutcomeHandler : IRequestHandler<GetModerationOutcomeRequest, ModerationOutcome>
    {
        private readonly IClarificationRepository _repository;
        private readonly ILogger<GetModerationOutcomeHandler> _logger;

        public GetModerationOutcomeHandler(IClarificationRepository repository, ILogger<GetModerationOutcomeHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ModerationOutcome> Handle(GetModerationOutcomeRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GetModerationOutcome for ApplicationId '{request.ApplicationId}' - PageId '{request.PageId}'");

            var moderationOutcomeRequest = await _repository.GetModerationOutcome(request.ApplicationId,
                                                        request.SequenceNumber,
                                                        request.SectionNumber,
                                                        request.PageId);

            return moderationOutcomeRequest;
        }
    }
}
