using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class GetAssessorPageReviewOutcomeRequest : IRequest<AssessorPageReviewOutcome>
    {
        public GetAssessorPageReviewOutcomeRequest(Guid applicationId,
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
