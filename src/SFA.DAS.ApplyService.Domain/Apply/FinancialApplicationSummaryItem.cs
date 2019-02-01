using System;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class FinancialApplicationSummaryItem
    {
        public Guid ApplicationId { get; set; }
        public int SequenceId { get; set; }
        public int SectionId { get; set; }
        public string OrganisationName { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public int SubmissionCount { get; set; }
        public string CurrentStatus { get; set; }
        public FinancialApplicationGrade Grade { get; set; }
    }
}
