using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class RemoveSubcontractorDeclarationFileRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; set; }
        public string FileName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public RemoveSubcontractorDeclarationFileRequest(Guid applicationId, string fileName, string userId, string userName)
        {
            ApplicationId = applicationId;
            FileName = fileName;
            UserId = userId;
            UserName = userName;
        }
    }
}