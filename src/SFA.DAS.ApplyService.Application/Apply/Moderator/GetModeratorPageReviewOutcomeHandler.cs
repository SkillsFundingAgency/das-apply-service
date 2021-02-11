using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Moderator
{
    public class GetModeratorPageReviewOutcomeHandler : IRequestHandler<GetModeratorPageReviewOutcomeRequest, ModeratorPageReviewOutcome>
    {
        private readonly IModeratorRepository _repository;
        private readonly ILogger<GetModeratorPageReviewOutcomeHandler> _logger;

        public GetModeratorPageReviewOutcomeHandler(IModeratorRepository repository, ILogger<GetModeratorPageReviewOutcomeHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ModeratorPageReviewOutcome> Handle(GetModeratorPageReviewOutcomeRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GetModeratorPageReviewOutcome for ApplicationId '{request.ApplicationId}' - PageId '{request.PageId}'");

            var pagePageReviewOutcome = await _repository.GetModeratorPageReviewOutcome(request.ApplicationId,
                                                        request.SequenceNumber,
                                                        request.SectionNumber,
                                                        request.PageId);

            return pagePageReviewOutcome;
        }
    }
}
