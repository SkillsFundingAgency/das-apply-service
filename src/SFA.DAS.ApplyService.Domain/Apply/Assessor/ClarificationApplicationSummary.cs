using System;

namespace SFA.DAS.ApplyService.Domain.Apply.Assessor
{
    public class ClarificationApplicationSummary : AssessorApplicationSummary
    {
        public string ModeratorName { get; set; }
        public DateTime ClarificationRequestedDate { get; set; }
    }
}
