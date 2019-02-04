using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Download
{
    public class FileInfoRequest : IRequest<FileInfoResponse>
    {
        public Guid ApplicationId { get; }
        public Guid UserId { get; }
        public string PageId { get; }
        public int SequenceId { get; }
        public int SectionId { get; }
        public string QuestionId { get; }
        public string Filename { get; }

        public FileInfoRequest(Guid applicationId, Guid userId, string pageId, int sequenceId, int sectionId, string questionId, string filename)
        {
            ApplicationId = applicationId;
            UserId = userId;
            PageId = pageId;
            SequenceId = sequenceId;
            SectionId = sectionId;
            QuestionId = questionId;
            Filename = filename;
        }
    }
}