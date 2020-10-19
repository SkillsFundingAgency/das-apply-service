using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        Task SubmitGatewayPageAnswer(Guid applicationId, string pageId, string userName, string status, string comments);
        Task<bool> UpdateGatewayReviewStatusAndComment(Guid applicationId, string gatewayReviewStatus, string gatewayReviewComment, string userName);

        Task<bool> CanSubmitApplication(Guid applicationId);
        Task SubmitApplication(Guid applicationId, ApplyData applyData, Guid submittedBy);

        Task<Guid> SnapshotApplication(Guid applicationId, Guid snapshotApplicationId, List<ApplySequence> newSequences);

        Task<List<RoatpApplicationSummaryItem>> GetNewGatewayApplications();
        Task<List<RoatpApplicationSummaryItem>> GetInProgressGatewayApplications();
        Task<List<RoatpApplicationSummaryItem>> GetClosedGatewayApplications();
        Task StartGatewayReview(Guid applicationId, string reviewer);
        Task EvaluateGateway(Guid applicationId, bool isGatewayApproved, string evaluatedBy);

        Task<List<RoatpApplicationSummaryItem>> GetOpenApplications();
        Task<List<RoatpApplicationSummaryItem>> GetFeedbackAddedApplications();
        Task<List<RoatpApplicationSummaryItem>> GetClosedApplications();

        Task<List<RoatpFinancialSummaryItem>> GetOpenFinancialApplications();
        Task<List<RoatpFinancialSummaryItem>> GetClarificationFinancialApplications();
        Task<List<RoatpFinancialSummaryItem>> GetClosedFinancialApplications();
        Task<RoatpFinancialApplicationsStatusCounts> GetFinancialApplicationsStatusCounts();
        Task<bool> StartFinancialReview(Guid applicationId, string reviewer);
        Task<bool> RecordFinancialGrade(Guid applicationId, FinancialReviewDetails financialReviewDetails, string financialReviewStatus);


        Task<IEnumerable<RoatpApplicationStatus>> GetExistingApplicationStatusByUkprn(string ukprn);

        Task<string> GetNextRoatpApplicationReference();

        Task<bool> StartAssessorReview(Guid applicationId, string reviewer);  
        
        Task<ApplyData> GetApplyData(Guid applicationId);

        Task<bool> UpdateApplyData(Guid applicationId, ApplyData applyData, string updatedBy);

        Task<bool> ChangeProviderRoute(Guid applicationId, int providerRoute, string providerRouteName);

        Task<bool> IsUkprnWhitelisted(int ukprn);

        Task UpdateApplicationStatus(Guid applicationId, string status);

        Task<Contact> GetContactForApplication(Guid applicationId);
        Task<Organisation> GetOrganisationForApplication(Guid applicationId);

        Task<List<NotRequiredOverride>> GetNotRequiredOverrides(Guid applicationId);
        Task<bool> UpdateNotRequiredOverrides(Guid applicationId, IEnumerable<NotRequiredOverride> notRequiredOverrides);
       
        Task<List<ApplicationOversightDetails>> GetOversightsPending();
        Task<List<ApplicationOversightDetails>> GetOversightsCompleted();

        Task<bool> UpdateOversightReviewStatus(Guid applicationId, string oversightStatus, DateTime applicationDeterminedDate, string updatedBy);
        Task<ApplicationOversightDetails> GetOversightDetails(Guid applicationId);
    }
}