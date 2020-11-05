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
                                                        string userName,
                                                        string status,
                                                        string comment,
                                                        string clarificationResponse,
                                                        string clarificationFile)
        {
            ApplicationId = applicationId;
            SequenceNumber = sequenceNumber;
            SectionNumber = sectionNumber;
            PageId = pageId;
            UserId = userId;
            UserName = userName;
            Status = status;
            Comment = comment;
            ClarificationResponse = clarificationResponse;
            ClarificationFile = clarificationFile;
        }

        public Guid ApplicationId { get; }
        public int SequenceNumber { get; }
        public int SectionNumber { get; }
        public string PageId { get; }
        public string UserId { get; }
        public string UserName { get; }
        public string Status { get; }
        public string Comment { get; }
        public string ClarificationResponse { get; }
        public string ClarificationFile { get; }
    }
}
