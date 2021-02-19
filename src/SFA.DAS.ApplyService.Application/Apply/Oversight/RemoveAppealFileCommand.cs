using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class RemoveAppealFileCommand : IRequest
    {
        public Guid ApplicationId { get; set; }
        public Guid AppealUploadId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
