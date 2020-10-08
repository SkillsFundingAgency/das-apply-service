using System;

namespace SFA.DAS.ApplyService.Domain.Apply.Assessor
{
    public class ClosedApplicationSummary : AssessorApplicationSummary
    {
        public string OutcomeStatus { get; set; }
        public DateTime OutcomeDate { get; set; }
    }
}
