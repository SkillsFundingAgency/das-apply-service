using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class GetAssessorPageReviewOutcomeHandler : IRequestHandler<GetAssessorPageReviewOutcomeRequest, AssessorPageReviewOutcome>
    {
        private readonly IAssessorRepository _repository;
        private readonly ILogger<GetAssessorPageReviewOutcomeHandler> _logger;

        public GetAssessorPageReviewOutcomeHandler(IAssessorRepository repository, ILogger<GetAssessorPageReviewOutcomeHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<AssessorPageReviewOutcome> Handle(GetAssessorPageReviewOutcomeRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GetAssessorPageReviewOutcome for ApplicationId '{request.ApplicationId}' - " +
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
