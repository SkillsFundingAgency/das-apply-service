using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Entities;
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
                _logger.LogError("Application {ApplicationId} has no Apply Data JSON", request.ApplicationId);
                return await Task.FromResult(false);
            }

            var sequenceUnderReview = applyData.Sequences.FirstOrDefault(x => x.SequenceNo == request.SequenceId);
            if (sequenceUnderReview == null)
            {
                _logger.LogError("Application {ApplicationId} unable to lookup sequence {SequenceId}", request.ApplicationId, request.SequenceId);
                return await Task.FromResult(false);
            }

            var sectionUnderReview = sequenceUnderReview.Sections.FirstOrDefault(x => x.SectionNo == request.SectionId);
            if (sectionUnderReview == null)
            {
                _logger.LogError("Application {ApplicationId} unable to lookup sequence {SequenceId}, section {SectionId}", request.ApplicationId, request.SequenceId, request.SectionId);
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
                _logger.LogInformation("Request to change status of application {ApplicationId} ignored, current status is {Status}", request.ApplicationId, sectionUnderReview.Status);
                return await Task.FromResult(false);
            }

        }
    }
}
