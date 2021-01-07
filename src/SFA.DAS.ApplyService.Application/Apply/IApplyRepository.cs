using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Apply.Gateway;
using SFA.DAS.ApplyService.Domain.Audit;

namespace SFA.DAS.ApplyService.Application.Apply
{
    public interface IApplyRepository
    {
        Task<Guid> StartApplication(Guid applicationId, ApplyData applyData, Guid organisationId, Guid createdBy);

        Task<Domain.Entities.Apply> GetApplication(Guid applicationId);
        Task<Domain.Entities.Apply> GetApplicationByUserId(Guid applicationId, Guid signinId);
        Task<List<Domain.Entities.Apply>> GetUserApplications(Guid signinId);
        Task<List<Domain.Entities.Apply>> GetOrganisationApplications(Guid signinId);

        Task<List<GatewayPageAnswerSummary>> GetGatewayPageAnswers(Guid applicationId);
        Task<GatewayPageAnswer> GetGatewayPageAnswer(Guid applicationId, string pageId);

        Task<string> GetGatewayPageStatus(Guid applicationId, string pageId);
        Task<string> GetGatewayPageComments(Guid applicationId, string pageId);
       
        Task InsertGatewayPageAnswer(GatewayPageAnswer pageAnswer, string userId, string userName);
        Task UpdateGatewayPageAnswer(GatewayPageAnswer pageAnswer, string userId, string userName);
        Task UpdateApplication(Domain.Entities.Apply application);

        Task<bool> UpdateGatewayReviewStatusAndComment(Guid applicationId, ApplyData applyData, string gatewayReviewStatus, string userId, string userName);
        Task<bool> UpdateGatewayApplyData(Guid applicationId, ApplyData applyData, string userId, string userName);
        Task<bool> CanSubmitApplication(Guid applicationId);
        Task SubmitApplication(Guid applicationId, ApplyData applyData, FinancialData financialData, Guid submittedBy);

        Task<Guid> SnapshotApplication(Guid applicationId, Guid snapshotApplicationId, List<ApplySequence> newSequences);

        Task<List<RoatpGatewaySummaryItem>> GetNewGatewayApplications();
        Task<List<RoatpGatewaySummaryItem>> GetInProgressGatewayApplications();
        Task<List<RoatpGatewaySummaryItem>> GetClosedGatewayApplications();
        Task StartGatewayReview(Guid applicationId, string reviewer);
        Task EvaluateGateway(Guid applicationId, bool isGatewayApproved, string evaluatedBy);

        Task<List<RoatpFinancialSummaryItem>> GetOpenFinancialApplications();
        Task<List<RoatpFinancialSummaryDownloadItem>> GetOpenFinancialApplicationsForDownload();
        Task<List<RoatpFinancialSummaryItem>> GetClarificationFinancialApplications();
        Task<List<RoatpFinancialSummaryItem>> GetClosedFinancialApplications();
        Task<RoatpFinancialApplicationsStatusCounts> GetFinancialApplicationsStatusCounts();
        Task<bool> StartFinancialReview(Guid applicationId, string reviewer);
        Task<bool> RecordFinancialGrade(Guid applicationId, FinancialReviewDetails financialReviewDetails, string financialReviewStatus);

        Task<bool> UpdateFinancialReviewDetails(Guid applicationId, FinancialReviewDetails financialReviewDetails);

        Task<IEnumerable<RoatpApplicationStatus>> GetExistingApplicationStatusByUkprn(string ukprn);

        Task<string> GetNextRoatpApplicationReference();

        Task<bool> StartAssessorReview(Guid applicationId, string reviewer);  
        
        Task<ApplyData> GetApplyData(Guid applicationId);

        Task<bool> UpdateApplyData(Guid applicationId, ApplyData applyData, string updatedBy);

        Task<bool> ChangeProviderRoute(Guid applicationId, int providerRoute, string providerRouteName);

        Task<bool> IsUkprnWhitelisted(int ukprn);

        Task UpdateApplicationStatus(Guid applicationId, string status);

        Task<bool> WithdrawApplication(Guid applicationId, string comments, string userId, string userName);
        Task<bool> RemoveApplication(Guid applicationId, string comments, string externalComments, string userId, string userName);

        Task<Contact> GetContactForApplication(Guid applicationId);
        Task<Organisation> GetOrganisationForApplication(Guid applicationId);

        Task<List<ApplicationOversightDetails>> GetOversightsPending();
        Task<List<ApplicationOversightDetails>> GetOversightsCompleted();

        Task<bool> UpdateOversightReviewStatus(Guid applicationId, string oversightStatus, string userId, string userName);
        Task<ApplicationOversightDetails> GetOversightDetails(Guid applicationId);
        Task<IEnumerable<GatewayApplicationStatusCount>> GetGatewayApplicationStatusCounts();

        Task InsertAudit(Audit audit);
    }
}