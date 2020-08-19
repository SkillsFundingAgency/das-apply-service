using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class GetPageReviewOutcomeRequest : IRequest<AssessorPageReviewOutcome>
    {
        public GetPageReviewOutcomeRequest(Guid applicationId,
                                                    int sequenceNumber,
                                                    int sectionNumber,
                                                    string pageId,
                                                    int assessorType,
                                                    string userId)
        {
            ApplicationId = applicationId;
            SequenceNumber = sequenceNumber;
            SectionNumber = sectionNumber;
            PageId = pageId;
            AssessorType = assessorType;
            UserId = userId;
        }

        public Guid ApplicationId { get; }
        public int SequenceNumber { get; }
        public int SectionNumber { get; }
        public string PageId { get; }
        public int AssessorType { get; }
        public string UserId { get; }
    }
}
