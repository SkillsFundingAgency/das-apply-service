using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Moderator
{
    public class GetBlindAssessmentOutcomeHandler : IRequestHandler<GetBlindAssessmentOutcomeRequest, BlindAssessmentOutcome>
    {
        private readonly IModeratorRepository _repository;
        private readonly ILogger<GetBlindAssessmentOutcomeHandler> _logger;

        public GetBlindAssessmentOutcomeHandler(IModeratorRepository repository, ILogger<GetBlindAssessmentOutcomeHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<BlindAssessmentOutcome> Handle(GetBlindAssessmentOutcomeRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GetBlindAssessmentOutcome for ApplicationId '{request.ApplicationId}' - PageId '{request.PageId}'");

            var blindAssessmentOutcome = await _repository.GetBlindAssessmentOutcome(request.ApplicationId,
                                                        request.SequenceNumber,
                                                        request.SectionNumber,
                                                        request.PageId);

            return blindAssessmentOutcome;
        }
    }
}
