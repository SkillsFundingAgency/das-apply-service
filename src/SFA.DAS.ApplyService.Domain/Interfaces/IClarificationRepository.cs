using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    public interface IClarificationRepository
    {
        Task SubmitClarificationPageOutcome(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId, string userId, string userName, string status, string comment, string clarificationResponse, string clarificationFile);
        Task<ClarificationPageReviewOutcome> GetClarificationPageReviewOutcome(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId);
        Task<List<ClarificationPageReviewOutcome>> GetClarificationPageReviewOutcomesForSection(Guid applicationId, int sequenceNumber, int sectionNumber);
        Task<List<ClarificationPageReviewOutcome>> GetAllClarificationPageReviewOutcomes(Guid applicationId);
        Task DeleteClarificationFile(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId, string clarificationFile);
    }
}
