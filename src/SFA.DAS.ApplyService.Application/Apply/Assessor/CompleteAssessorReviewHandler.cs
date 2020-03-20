using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class CompleteAssessorReviewHandler : IRequestHandler<CompleteAssessorReviewRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly ILogger<CompleteAssessorReviewHandler> _logger;

        public CompleteAssessorReviewHandler(IApplyRepository applyRepository, ILogger<CompleteAssessorReviewHandler> logger)
        {
            _applyRepository = applyRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(CompleteAssessorReviewRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Completing assessor review for application {request.ApplicationId}");
            return await _applyRepository.CompleteAssessorReview(request.ApplicationId, request.Reviewer);
        }
    }
}
