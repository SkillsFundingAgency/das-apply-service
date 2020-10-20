using System;

namespace SFA.DAS.ApplyService.Domain.Apply.Clarification
{
    public class ClarificationPageReviewOutcome
    {
        public Guid ApplicationId { get; set; }
        public int SequenceNumber { get; set; }
        public int SectionNumber { get; set; }
        public string PageId { get; set; }
        public string ModeratorUserId { get; set; }
        public string ModeratorReviewStatus { get; set; }
        public string ModeratorReviewComment { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public string ClarificationResponse { get; set; }
    }
}
