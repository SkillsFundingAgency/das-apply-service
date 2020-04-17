using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class UpdateGatewayReviewStatusAndCommentRequest
    {
        public Guid ApplicationId { get; }
        public string GatewayReviewStatus { get; }
        public string GatewayReviewComment { get; }
        public string UserName { get; }

        public UpdateGatewayReviewStatusAndCommentRequest(Guid applicationId, string gatewayReviewStatus, string gatewayReviewComment, string userName)
        {
            ApplicationId = applicationId;
            GatewayReviewStatus = gatewayReviewStatus;
            GatewayReviewComment = gatewayReviewComment;
            UserName = userName;
        }
    }
}
