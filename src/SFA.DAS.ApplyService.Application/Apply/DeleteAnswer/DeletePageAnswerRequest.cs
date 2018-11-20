using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.DeleteAnswer
{
    public class DeletePageAnswerRequest : IRequest
    {
        public Guid ApplicationId { get; }
        public Guid UserId { get; }
        public int SequenceId { get; }
        public int SectionId { get; }
        public string PageId { get; }
        public Guid AnswerId { get; }

        public DeletePageAnswerRequest(Guid applicationId, Guid userId, int sequenceId, int sectionId, string pageId, Guid answerId)
        {
            ApplicationId = applicationId;
            UserId = userId;
            SequenceId = sequenceId;
            SectionId = sectionId;
            PageId = pageId;
            AnswerId = answerId;
        }
    }
}