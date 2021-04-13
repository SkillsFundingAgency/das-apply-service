using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Apply.Gateway;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    public interface IGatewayRepository
    {
        Task<bool> WithdrawApplication(Guid applicationId, string comments, string userId, string userName);
        Task<bool> RemoveApplication(Guid applicationId, string comments, string externalComments, string userId, string userName);

        Task<List<RoatpGatewaySummaryItem>> GetNewGatewayApplications();
        Task<List<RoatpGatewaySummaryItem>> GetInProgressGatewayApplications();
        Task<List<RoatpGatewaySummaryItem>> GetClosedGatewayApplications();
        Task<IEnumerable<GatewayApplicationStatusCount>> GetGatewayApplicationStatusCounts();

        Task<bool> StartGatewayReview(Guid applicationId, string userId, string userName);

        Task<bool> EvaluateGateway(Guid applicationId, bool isGatewayApproved, string evaluatedBy);

        Task<bool> UpdateGatewayApplyData(Guid applicationId, ApplyData applyData, string userId, string userName);

        Task<bool> UpdateGatewayReviewStatusAndComment(Guid applicationId, ApplyData applyData, string gatewayReviewStatus, string userId, string userName);

        Task<List<GatewayPageAnswerSummary>> GetGatewayPageAnswers(Guid applicationId);
        Task<GatewayPageAnswer> GetGatewayPageAnswer(Guid applicationId, string pageId);

        Task<bool> InsertGatewayPageAnswer(GatewayPageAnswer pageAnswer, string userId, string userName);
        Task<bool> UpdateGatewayPageAnswer(GatewayPageAnswer pageAnswer, string userId, string userName);
        Task<bool> InsertGatewayPageAnswerClarification(GatewayPageAnswer pageAnswer, string userId, string userName);

        Task<bool> UpdateGatewayPageAnswerClarification(GatewayPageAnswer pageAnswer, string userId, string userName);
        Task<bool> UpdateGatewayPageAnswerPostClarification(GatewayPageAnswer pageAnswer, string userId, string userName);
    }
}
