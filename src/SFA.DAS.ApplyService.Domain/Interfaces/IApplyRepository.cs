using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    public interface IApplyRepository
    {
        Task<Guid> StartApplication(Guid applicationId, ApplyData applyData, Guid organisationId, Guid createdBy);

        Task<Domain.Entities.Apply> GetApplication(Guid applicationId);
        Task<Domain.Entities.Apply> GetApplicationByUserId(Guid applicationId, Guid signinId);
        Task<List<Domain.Entities.Apply>> GetUserApplications(Guid signinId);
        Task<List<Domain.Entities.Apply>> GetOrganisationApplications(Guid signinId);
        Task<List<Domain.Entities.Apply>> GetApplicationsByUkprn(string ukprn);

        Task<FinancialReviewDetails> GetFinancialReviewDetails(Guid applicationId);
        Task<List<ClarificationFile>> GetFinancialReviewClarificationFiles(Guid applicationId);

        Task UpdateApplication(Domain.Entities.Apply application);
        
        Task<bool> CanSubmitApplication(Guid applicationId);
        Task<bool> SubmitApplication(Guid applicationId, ApplyData applyData, FinancialData financialData, Guid submittedBy);

        Task<Guid> SnapshotApplication(Guid applicationId, Guid snapshotApplicationId, List<ApplySequence> newSequences);

        Task<List<RoatpFinancialSummaryItem>> GetOpenFinancialApplications(string searchTerm, string sortColumn, string sortOrder);
        Task<List<RoatpFinancialSummaryDownloadItem>> GetOpenFinancialApplicationsForDownload();
        Task<List<RoatpFinancialSummaryItem>> GetClarificationFinancialApplications(string searchTerm, string sortColumn, string sortOrder);
        Task<List<RoatpFinancialSummaryItem>> GetClosedFinancialApplications(string searchTerm, string sortColumn, string sortOrder);
        Task<RoatpFinancialApplicationsStatusCounts> GetFinancialApplicationsStatusCounts(string searchTerm);
        Task<bool> StartFinancialReview(Guid applicationId, string reviewer);
        Task<bool> RecordFinancialGrade(Guid applicationId, FinancialReviewDetails financialReviewDetails, string financialReviewStatus);
        Task<bool> AddFinancialReviewClarificationFile(Guid applicationId, string filename);
        Task<bool> RemoveFinancialReviewClarificationFile(Guid applicationId, string filename);
        Task<IEnumerable<RoatpApplicationStatus>> GetExistingApplicationStatusByUkprn(string ukprn);

        Task<string> GetNextRoatpApplicationReference();

        Task<bool> StartAssessorReview(Guid applicationId, string reviewer);  
        
        Task<ApplyData> GetApplyData(Guid applicationId);

        Task<bool> UpdateApplyData(Guid applicationId, ApplyData applyData, string updatedBy);

        Task<bool> ChangeProviderRoute(Guid applicationId, int providerRoute, string providerRouteName);

        Task UpdateApplicationStatus(Guid applicationId, string status, string userId);

        Task<Contact> GetContactForApplication(Guid applicationId);
        Task<Organisation> GetOrganisationForApplication(Guid applicationId);
        Task<List<ApplicationOversightDownloadDetails>> GetOversightsForDownload(DateTime dateFrom, DateTime dateTo);
        Task<bool> SubmitReapplicationRequest(Guid requestApplicationId, string requestedBy);
    }
}