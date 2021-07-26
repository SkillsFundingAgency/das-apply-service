using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Financial
{
    public class AddClarificationFileUploadRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; set; }
        public string FileName { get; set; }
        public AddClarificationFileUploadRequest(Guid applicationId, string fileName)
        {
            ApplicationId = applicationId;
            FileName = fileName;
        }
    }
}