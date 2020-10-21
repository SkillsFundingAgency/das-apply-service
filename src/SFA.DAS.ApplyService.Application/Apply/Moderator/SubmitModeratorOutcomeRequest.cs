using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Moderator
{
    public class SubmitModeratorOutcomeRequest : IRequest<bool>
    {
        public SubmitModeratorOutcomeRequest(Guid applicationId, string userId, string userName, string status, string comment)
        {
            ApplicationId = applicationId;
            UserId = userId;
            UserName = userName;
            Status = status;
            Comment = comment;

        }
        public Guid ApplicationId { get; }
        public string UserId { get; }
        public string UserName { get; }
        public string Status { get; }
        public string Comment { get; }
    }
}