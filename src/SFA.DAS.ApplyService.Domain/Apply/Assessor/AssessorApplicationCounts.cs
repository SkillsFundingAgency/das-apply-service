namespace SFA.DAS.ApplyService.Domain.Apply.Assessor
{
    public class AssessorApplicationCounts
    {
        public AssessorApplicationCounts(int newApplications, int inProgressApplications, int moderationApplications, int clarificationApplications)
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
