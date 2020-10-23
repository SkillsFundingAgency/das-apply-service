using MediatR;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Clarification
{
    public class SubmitClarificationPageOutcomeRequest : IRequest
    {
        public SubmitClarificationPageOutcomeRequest(Guid applicationId,
                                                        int sequenceNumber,
                                                        int sectionNumber,
                                                        string pageId,
                                                        string userId,
                                                        string status,
                                                        string comment,
                                                        string clarificationResponse)
        {
            ApplicationId = applicationId;
            SequenceNumber = sequenceNumber;
            SectionNumber = sectionNumber;
            PageId = pageId;
            UserId = userId;
            Status = status;
            Comment = comment;
            ClarificationResponse = clarificationResponse;
        }

        public Guid ApplicationId { get; }
        public int SequenceNumber { get; }
        public int SectionNumber { get; }
        public string PageId { get; }
        public string UserId { get; }
        public string Status { get; }
        public string Comment { get; }
        public string ClarificationResponse { get; }
    }
}
