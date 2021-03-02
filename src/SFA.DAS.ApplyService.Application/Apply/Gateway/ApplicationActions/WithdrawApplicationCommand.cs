using MediatR;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway.ApplicationActions
{
    public class WithdrawApplicationCommand : IRequest<bool>
    {
        public Guid ApplicationId { get; }
        public string Comments { get; }
        public string UserId { get; }
        public string UserName { get; }

        public WithdrawApplicationCommand(Guid applicationId, string comments, string userId, string userName)
        {
            ApplicationId = applicationId;
            Comments = comments;
            UserId = userId;
            UserName = userName;
        }
    }
}
