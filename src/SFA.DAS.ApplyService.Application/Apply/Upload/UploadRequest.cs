using System;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace SFA.DAS.ApplyService.Application.Apply.Upload
{
    public class UploadRequest : IRequest<UploadResult>
    {
        public Guid ApplicationId { get; }
        public int SequenceId { get; }
        public int SectionId { get; }
        public Guid UserId { get; }
        public string PageId { get; }
        public IFormFileCollection Files { get; }

        public UploadRequest(Guid applicationId, int sequenceId, int sectionId, Guid userId, string pageId, IFormFileCollection files)
        {
            ApplicationId = applicationId;
            SequenceId = sequenceId;
            SectionId = sectionId;
            UserId = userId;
            PageId = pageId;
            Files = files;
        }
    }
}