using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class AssessorEvaluateSectionHandler : IRequestHandler<AssessorEvaluateSectionRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly ILogger<AssessorEvaluateSectionHandler> _logger;

        public AssessorEvaluateSectionHandler(IApplyRepository applyRepository, ILogger<AssessorEvaluateSectionHandler> logger)
        {
            _applyRepository = applyRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(AssessorEvaluateSectionRequest request, CancellationToken cancellationToken)
        {            
            var applyData = await _applyRepository.GetApplyData(request.ApplicationId);

            if (applyData == null)
            {
                return await Task.FromResult(false);
            }

            var sequenceUnderReview = applyData.Sequences.FirstOrDefault(x => x.SequenceNo == request.SequenceId);
            if (sequenceUnderReview == null)
            {
                return await Task.FromResult(false);
            }

            var sectionUnderReview = sequenceUnderReview.Sections.FirstOrDefault(x => x.SectionNo == request.SectionId);
            if (sectionUnderReview == null)
            {
                return await Task.FromResult(false);
            }

            var status = AssessorReviewStatus.Approved;
            if (!request.SectionCompleted)
            {
                status = AssessorReviewStatus.Declined;
            }
            sectionUnderReview.Status = status;

            _logger.LogInformation($"Assessor evaluation of {sectionUnderReview.Status} for application {request.ApplicationId} sequence {request.SequenceId} section {request.SectionId}");

            return await _applyRepository.UpdateApplyData(request.ApplicationId, applyData, request.Reviewer);
        }
    }
}
