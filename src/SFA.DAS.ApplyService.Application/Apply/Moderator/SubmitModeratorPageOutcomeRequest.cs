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
                                                        string userName,
                                                        string status,
                                                        string comment)
        {
            ApplicationId = applicationId;
            SequenceNumber = sequenceNumber;
            SectionNumber = sectionNumber;
            PageId = pageId;
            UserId = userId;
            UserName = userName;
            Status = status;
            Comment = comment;
        }

        public Guid ApplicationId { get; }
        public int SequenceNumber { get; }
        public int SectionNumber { get; }
        public string PageId { get; }
        public string UserId { get; }
        public string UserName { get; }
        public string Status { get; }
        public string Comment { get; }
    }
}
