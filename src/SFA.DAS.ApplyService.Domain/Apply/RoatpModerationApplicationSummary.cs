using System;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class RoatpModerationApplicationSummary : RoatpAssessorApplicationSummary
    {
        public ModerationStatus Status { get; set; }
    }
}
