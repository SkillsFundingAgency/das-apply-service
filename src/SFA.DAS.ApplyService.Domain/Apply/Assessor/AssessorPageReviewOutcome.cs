using System;

namespace SFA.DAS.ApplyService.Domain.Apply.Assessor
{
    public class AssessorPageReviewOutcome
    {
        public Guid ApplicationId { get; set; }
        public int SequenceNumber { get; set; }
        public int SectionNumber { get; set; }
        public string PageId { get; set; }
        public int AssessorType { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
    }
}
