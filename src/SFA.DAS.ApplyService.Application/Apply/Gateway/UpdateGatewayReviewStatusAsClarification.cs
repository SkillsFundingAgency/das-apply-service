using System;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class UpdateGatewayReviewStatusAsClarification
    {
        public Guid ApplicationId { get; }
        public string UserId { get; }
        public string UserName { get; }

        public UpdateGatewayReviewStatusAsClarification(Guid applicationId, string userId, string userName)
        {
            ApplicationId = applicationId;
            UserId = userId;
            UserName = userName;
        }
    }
}