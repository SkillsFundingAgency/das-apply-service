using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

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
            var applyData = await _applyRepository.GetApplyData(request.ApplicationId);
            if (applyData == null)
            {
                _logger.LogError($"Application {request.ApplicationId} has no Apply Data JSON");
                return await Task.FromResult(false);
            }

            var sequenceUnderReview = applyData.Sequences.FirstOrDefault(x => x.SequenceNo == request.SequenceId);
            if (sequenceUnderReview == null)
            {
                _logger.LogError($"Application {request.ApplicationId} unable to lookup sequence {request.SequenceId}");
                return await Task.FromResult(false);
            }

            var sectionUnderReview = sequenceUnderReview.Sections.FirstOrDefault(x => x.SectionNo == request.SectionId);
            if (sectionUnderReview == null)
            {
                _logger.LogError($"Application {request.ApplicationId} unable to lookup sequence {request.SequenceId}, section {request.SectionId}");
                return await Task.FromResult(false);
            }

            if (String.IsNullOrWhiteSpace(sectionUnderReview.Status)
                || (sectionUnderReview.Status != AssessorReviewStatus.Approved && sectionUnderReview.Status != AssessorReviewStatus.Declined))
            {
                sectionUnderReview.Status = AssessorReviewStatus.InProgress;
                return await _applyRepository.UpdateApplyData(request.ApplicationId, applyData, request.Reviewer);
            }
            else
            {
                _logger.LogInformation($"Request to change status of application {request.ApplicationId} ignored, current status is {sectionUnderReview.Status}");
                return await Task.FromResult(false);
            }
                      
        }
    }
}
