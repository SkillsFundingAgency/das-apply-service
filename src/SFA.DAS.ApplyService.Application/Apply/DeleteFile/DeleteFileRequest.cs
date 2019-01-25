using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.DeleteFile
{
    public class DeleteFileRequest : IRequest
    {
        public Guid ApplicationId { get; set; }
        public Guid UserId { get; set;}
        public string PageId { get; set;}
        public int SequenceId { get; set;}
        public int SectionId { get; set;}
        public string QuestionId { get; set;}

        public DeleteFileRequest(Guid applicationId, Guid userId, string pageId, int sequenceId, int sectionId, string questionId)
        {
            ApplicationId = applicationId;
            UserId = userId;
            PageId = pageId;
            SequenceId = sequenceId;
            SectionId = sectionId;
            QuestionId = questionId;
        }

        public DeleteFileRequest()
        {
            
        }
    }
}