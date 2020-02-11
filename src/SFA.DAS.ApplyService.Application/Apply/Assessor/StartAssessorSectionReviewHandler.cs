using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class StartAssessorSectionReviewHandler : IRequestHandler<StartAssessorSectionReviewRequest, bool>
    {
        private IApplyRepository _applyRepository;
        private ILogger<StartAssessorSectionReviewHandler> _logger;

        public StartAssessorSectionReviewHandler(IApplyRepository applyRepository, ILogger<StartAssessorSectionReviewHandler> logger)
        {
            _applyRepository = applyRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(StartAssessorSectionReviewRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.StartAssessorSectionReview(request.ApplicationId, request.SequenceId,
                                                                     request.SectionId, request.Reviewer);
        }
    }
}
