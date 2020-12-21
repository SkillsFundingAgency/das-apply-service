using MediatR;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway.ApplicationActions
{
    public class WithdrawApplicationRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; }
        public string Comments { get; }
        public string UserId { get; }
        public string UserName { get; }

        public WithdrawApplicationRequest(Guid applicationId, string comments, string userId, string userName)
        {
            ApplicationId = applicationId;
            Comments = comments;
            UserId = userId;
            UserName = userName;
        }
    }
}
