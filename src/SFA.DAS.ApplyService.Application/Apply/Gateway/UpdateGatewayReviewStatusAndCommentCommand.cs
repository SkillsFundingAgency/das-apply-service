using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class UpdateGatewayReviewStatusAndCommentCommand : IRequest<bool>
    {
        public Guid ApplicationId { get; }
        public string GatewayReviewStatus { get; }
        public string GatewayReviewComment { get; }
        public string GatewayReviewExternalComment { get; }
        public string UserId { get; }
        public string UserName { get; }

        public UpdateGatewayReviewStatusAndCommentCommand(Guid applicationId, string gatewayReviewStatus, string gatewayReviewComment, string gatewayReviewExternalComment, string userId, string userName)
        {
            ApplicationId = applicationId;
            GatewayReviewStatus = gatewayReviewStatus;
            GatewayReviewComment = gatewayReviewComment;
            GatewayReviewExternalComment = gatewayReviewExternalComment;
            UserId = userId;
            UserName = userName;
        }
    }
}
