using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

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
            _logger.LogInformation($"GetAssessorPageReviewOutcome for ApplicationId '{request.ApplicationId}' - PageId '{request.PageId}'");

            var pagePageReviewOutcome = await _repository.GetAssessorPageReviewOutcome(request.ApplicationId,
                                                        request.SequenceNumber,
                                                        request.SectionNumber,
                                                        request.PageId,
                                                        request.UserId);

            return pagePageReviewOutcome;
        }
    }
}
