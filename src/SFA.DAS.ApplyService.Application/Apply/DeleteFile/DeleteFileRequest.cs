using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.DeleteFile
{
    public class DeleteFileRequest : IRequest
    {
        public Guid ApplicationId { get; }
        public Guid UserId { get; }
        public string PageId { get; }
        public int SequenceId { get; }
        public int SectionId { get; }
        public string QuestionId { get; }

        public DeleteFileRequest(Guid applicationId, Guid userId, string pageId, int sequenceId, int sectionId, string questionId)
        {
            ApplicationId = applicationId;
            UserId = userId;
            PageId = pageId;
            SequenceId = sequenceId;
            SectionId = sectionId;
            QuestionId = questionId;
        }
    }
}