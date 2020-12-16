using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class UpdateGatewayReviewStatusAsClarificationRequest:IRequest<bool>
    {
        public Guid ApplicationId { get; }
        public string UserId { get; }
        public string UserName { get; }

        public UpdateGatewayReviewStatusAsClarificationRequest(Guid applicationId, string userId, string userName)
        {
            ApplicationId = applicationId;
            UserId = userId;
            UserName = userName;
        }
    }
}