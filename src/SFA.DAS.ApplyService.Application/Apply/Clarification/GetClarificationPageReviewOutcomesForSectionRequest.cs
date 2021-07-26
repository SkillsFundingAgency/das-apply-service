using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.Clarification
{
    public class GetClarificationPageReviewOutcomesForSectionRequest : IRequest<List<ClarificationPageReviewOutcome>>
    {
        public GetClarificationPageReviewOutcomesForSectionRequest(Guid applicationId,
                                                                    int sequenceNumber,
                                                                    int sectionNumber,
                                                                    string userId)
        {
            ApplicationId = applicationId;
            SequenceNumber = sequenceNumber;
            SectionNumber = sectionNumber;
            UserId = userId;
        }

        public Guid ApplicationId { get; }
        public int SequenceNumber { get; }
        public int SectionNumber { get; }
        public string UserId { get; }
    }
}
