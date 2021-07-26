using System;

namespace SFA.DAS.ApplyService.Domain.Apply.Clarification
{
    public class ModerationOutcome
    {
        public Guid ApplicationId { get; set; }
        public int SequenceNumber { get; set; }
        public int SectionNumber { get; set; }
        public string PageId { get; set; }

        public string ModeratorName { get; set; }
        public string ModeratorUserId { get; set; }
        public string ModeratorReviewStatus { get; set; }
        public string ModeratorReviewComment { get; set; }
    }
}
