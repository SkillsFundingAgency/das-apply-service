using MediatR;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class EvaluateGatewayRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; }
        public bool IsGatewayApproved { get; }
        public string UserId { get; }
        public string UserName { get; }

        public EvaluateGatewayRequest(Guid applicationId, bool isGatewayApproved, string userId, string userName)
        {
            ApplicationId = applicationId;
            IsGatewayApproved = isGatewayApproved;
            UserId = userId;
            UserName = userName;
        }
    }
}
