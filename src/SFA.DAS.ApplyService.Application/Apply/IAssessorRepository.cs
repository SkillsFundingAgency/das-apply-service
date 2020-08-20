
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;

namespace SFA.DAS.ApplyService.Application.Apply
{
    public interface IAssessorRepository
    {
        Task<List<AssessorApplicationSummary>> GetNewAssessorApplications(string userId);
        Task<int> GetNewAssessorApplicationsCount(string userId);
        Task<List<AssessorApplicationSummary>> GetInProgressAssessorApplications(string userId);
        Task<int> GetInProgressAssessorApplicationsCount(string userId);
        Task<List<ModerationApplicationSummary>> GetApplicationsInModeration();
        Task<int> GetApplicationsInModerationCount();
        Task UpdateAssessor1(Guid applicationId, string userId, string userName);
        Task UpdateAssessor2(Guid applicationId, string userId, string userName);
        Task SubmitAssessorPageOutcome(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId, string userId, string status, string comment);
        Task<AssessorPageReviewOutcome> GetAssessorPageReviewOutcome(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId, string userId);
        Task<List<AssessorPageReviewOutcome>> GetAssessorPageReviewOutcomesForSection(Guid applicationId, int sequenceNumber, int sectionNumber, string userId);
        Task<List<AssessorPageReviewOutcome>> GetAllAssessorPageReviewOutcomes(Guid applicationId, string userId);
        Task UpdateAssessorReviewStatus(Guid applicationId, string userId, string status);
    }
}