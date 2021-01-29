using System;

namespace SFA.DAS.ApplyService.Domain.Apply.Assessor
{
    public class ClosedApplicationSummary : AssessorApplicationSummary
    {
        public string ModerationStatus { get; set; }

        public DateTime OutcomeMadeDate { get; set; }
        public string OutcomeMadeBy { get; set; }
    }
}
