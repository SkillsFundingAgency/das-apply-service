using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply
{
    public interface IModeratorRepository
    {
        Task<bool> SubmitModeratorPageOutcome(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId, string userId, string userName, string status, string comment);
        Task<BlindAssessmentOutcome> GetBlindAssessmentOutcome(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId);
        Task<ModeratorPageReviewOutcome> GetModeratorPageReviewOutcome(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId);
        Task<List<ModeratorPageReviewOutcome>> GetModeratorPageReviewOutcomesForSection(Guid applicationId, int sequenceNumber, int sectionNumber);
        Task<List<ModeratorPageReviewOutcome>> GetAllModeratorPageReviewOutcomes(Guid applicationId);
        Task CreateEmptyModeratorReview(Guid applicationId, string userId, string userName, List<ModeratorPageReviewOutcome> pageReviewOutcomes);
        Task<bool> UpdateModerationStatus(Guid applicationId, ApplyData applyData, string status, string userId);

    }
}
