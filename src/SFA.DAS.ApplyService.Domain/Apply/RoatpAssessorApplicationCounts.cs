using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class RoatpAssessorApplicationCounts
    {
        public RoatpAssessorApplicationCounts(int newApplications, int inProgressApplications, int moderationApplications, int clarificationApplications)
        {
            NewApplications = newApplications;
            InProgressApplications = inProgressApplications;
            ModerationApplications = moderationApplications;
            ClarificationApplications = clarificationApplications;
        }

        public int NewApplications { get; }
        public int InProgressApplications { get; }
        public int ModerationApplications { get; }
        public int ClarificationApplications { get; }
    }
}
