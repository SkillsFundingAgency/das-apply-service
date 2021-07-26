using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class StartAssessorReviewHandler : IRequestHandler<StartAssessorReviewRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly ILogger<StartAssessorReviewHandler> _logger;

        public StartAssessorReviewHandler(IApplyRepository applyRepository, ILogger<StartAssessorReviewHandler> logger)
        {
            _applyRepository = applyRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(StartAssessorReviewRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting assessor review for application {request.ApplicationId}");
            return await _applyRepository.StartAssessorReview(request.ApplicationId, request.Reviewer);
        }
    }
}
