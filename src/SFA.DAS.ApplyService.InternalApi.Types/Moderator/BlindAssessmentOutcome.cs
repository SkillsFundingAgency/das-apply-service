using System;

namespace SFA.DAS.ApplyService.InternalApi.Types.Moderator
{
    public class BlindAssessmentOutcome
    {
        public Guid ApplicationId { get; set; }
        public int SequenceNumber { get; set; }
        public int SectionNumber { get; set; }
        public string PageId { get; set; }

        public string Assessor1Name { get; set; }
        public string Assessor1UserId { get; set; }
        public string Assessor1ReviewStatus { get; set; }
        public string Assessor1ReviewComment { get; set; }
        public string Assessor2Name { get; set; }
        public string Assessor2UserId { get; set; }
        public string Assessor2ReviewStatus { get; set; }
        public string Assessor2ReviewComment { get; set; }
    }
}
