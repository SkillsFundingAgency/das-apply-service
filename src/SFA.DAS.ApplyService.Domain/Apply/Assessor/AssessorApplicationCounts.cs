namespace SFA.DAS.ApplyService.Domain.Apply.Assessor
{
    public class AssessorApplicationCounts
    {
        public AssessorApplicationCounts(int newApplications, int inProgressApplications, int moderationApplications, int clarificationApplications, int closedApplications)
        {
            NewApplications = newApplications;
            InProgressApplications = inProgressApplications;
            ModerationApplications = moderationApplications;
            ClarificationApplications = clarificationApplications;
            ClosedApplications = closedApplications;
        }

        public int NewApplications { get; }
        public int InProgressApplications { get; }
        public int ModerationApplications { get; }
        public int ClarificationApplications { get; }
        public int ClosedApplications { get; }
    }
}
