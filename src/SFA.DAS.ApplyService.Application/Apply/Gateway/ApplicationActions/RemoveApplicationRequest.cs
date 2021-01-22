using MediatR;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway.ApplicationActions
{
    public class RemoveApplicationRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; }
        public string Comments { get; }
        public string ExternalComments { get; }
        public string UserId { get; }
        public string UserName { get; }

        public RemoveApplicationRequest(Guid applicationId, string comments, string externalComments, string userId, string userName)
        {
            ApplicationId = applicationId;
            Comments = comments;
            ExternalComments = externalComments;
            UserId = userId;
            UserName = userName;
        }
    }
}
