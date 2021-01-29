namespace SFA.DAS.ApplyService.Domain.Apply.Assessor
{
    public class ModerationApplicationSummary : AssessorApplicationSummary
    {
        public string ModerationStatus { get; set; }
        public string ModeratorName { get; set; }
    }
}
