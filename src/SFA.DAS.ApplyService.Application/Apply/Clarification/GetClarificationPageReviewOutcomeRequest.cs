using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Clarification
{
    public class GetClarificationPageReviewOutcomeRequest : IRequest<ClarificationPageReviewOutcome>
    {
        public GetClarificationPageReviewOutcomeRequest(Guid applicationId,
                                                    int sequenceNumber,
                                                    int sectionNumber,
                                                    string pageId,
                                                    string userId)
        {
            ApplicationId = applicationId;
            SequenceNumber = sequenceNumber;
            SectionNumber = sectionNumber;
            PageId = pageId;
            UserId = userId;
        }

        public Guid ApplicationId { get; }
        public int SequenceNumber { get; }
        public int SectionNumber { get; }
        public string PageId { get; }
        public string UserId { get; }
    }
}
