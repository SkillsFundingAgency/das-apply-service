using System;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class UpdateGatewayReviewStatusAndCommentRequest
    {
        public Guid ApplicationId { get; }
        public string GatewayReviewStatus { get; }
        public string GatewayReviewComment { get; }
        public string UserId { get; }
        public string UserName { get; }

        public UpdateGatewayReviewStatusAndCommentRequest(Guid applicationId, string gatewayReviewStatus, string gatewayReviewComment, string userId, string userName)
        {
            ApplicationId = applicationId;
            GatewayReviewStatus = gatewayReviewStatus;
            GatewayReviewComment = gatewayReviewComment;
            UserId = userId;
            UserName = userName;
        }
    }
}
