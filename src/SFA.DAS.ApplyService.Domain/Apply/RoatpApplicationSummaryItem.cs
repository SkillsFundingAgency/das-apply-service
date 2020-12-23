using SFA.DAS.ApplyService.Domain.Entities;
using System;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class RoatpApplicationSummaryItem
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string ApplicationRoute { get; set; }
        public string OrganisationName { get; set; }
        public string Ukprn { get; set; }
        public string ApplicationReferenceNumber { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime? FeedbackAddedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime? ClarificationRequestedDate { get; set; }
        public string ApplicationStatus { get; set; }
        public string GatewayReviewStatus { get; set; }
        public string AssessorReviewStatus { get; set; }
        public string FinancialReviewStatus { get; set; }
        public string GatewayUserName { get; set; }
    }

    public class RoatpFinancialSummaryItem : RoatpApplicationSummaryItem
    {
        public string DeclaredInApplication { get; set; }

        public FinancialReviewDetails FinancialReviewDetails { get; set; }
    }
 }