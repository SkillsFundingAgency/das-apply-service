using MediatR;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Moderator
{
    public class SubmitModeratorPageOutcomeRequest : IRequest
    {
        public SubmitModeratorPageOutcomeRequest(Guid applicationId,
                                                        int sequenceNumber,
                                                        int sectionNumber,
                                                        string pageId,
                                                        string userId,
                                                        string status,
                                                        string comment,
                                                        string externalComment)
        {
            ApplicationId = applicationId;
            SequenceNumber = sequenceNumber;
            SectionNumber = sectionNumber;
            PageId = pageId;
            UserId = userId;
            Status = status;
            Comment = comment;
            ExternalComment = externalComment;
        }

        public Guid ApplicationId { get; }
        public int SequenceNumber { get; }
        public int SectionNumber { get; }
        public string PageId { get; }
        public string UserId { get; }
        public string Status { get; }
        public string Comment { get; }
        public string ExternalComment { get; }
    }
}
