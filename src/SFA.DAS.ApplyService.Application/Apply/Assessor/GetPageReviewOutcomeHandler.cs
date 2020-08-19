using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class GetPageReviewOutcomeHandler : IRequestHandler<GetPageReviewOutcomeRequest, AssessorPageReviewOutcome>
    {
        private readonly IAssessorRepository _repository;
        private readonly ILogger<GetPageReviewOutcomeHandler> _logger;

        public GetPageReviewOutcomeHandler(IAssessorRepository repository, ILogger<GetPageReviewOutcomeHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<AssessorPageReviewOutcome> Handle(GetPageReviewOutcomeRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GetPageReviewOutcome for ApplicationId '{request.ApplicationId}' - " +
                                                    $"SequenceNumber '{request.SequenceNumber}' - SectionNumber '{request.SectionNumber}' - PageId '{request.PageId}' - " +
                                                    $"AssessorType '{request.AssessorType}' - UserId '{request.UserId}'");

            var pagePageReviewOutcome = await _repository.GetAssessorPageReviewOutcome(request.ApplicationId,
                                                        request.SequenceNumber,
                                                        request.SectionNumber,
                                                        request.PageId,
                                                        request.AssessorType,
                                                        request.UserId);

            return pagePageReviewOutcome;
        }
    }
}
