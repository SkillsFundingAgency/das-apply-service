using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class UpdateGatewayPageAnswerClarificationRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; }
        public string PageId { get; }
        public string Status { get; }
        public string Comments { get; }
        public string UserId { get; }
        public string UserName { get; }
        public string ClarificationAnswer { get; }

        public UpdateGatewayPageAnswerClarificationRequest(Guid applicationId, string pageId, string status, string comments, string userId, string username, string clarificationAnswer)
        {
            ApplicationId = applicationId;
            PageId = pageId;
            Status = status;
            Comments = comments;
            UserId = userId;
            UserName = username;
            ClarificationAnswer = clarificationAnswer;
        }
    }
}