using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.Moderator
{
    public class GetModeratorPageReviewOutcomesForSectionRequest : IRequest<List<ModeratorPageReviewOutcome>>
    {
        public GetModeratorPageReviewOutcomesForSectionRequest(Guid applicationId,
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
