using System;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class ApplicationSummaryItem
    {
        public Guid ApplicationId { get; set; }
        public int SequenceId { get; set; }
        public string OrganisationName { get; set; }
        public string ApplicationType { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public int SubmissionCount { get; set; }
        public string FinancialStatus { get; set; }
        public string CurrentStatus { get; set; }
    }
}
