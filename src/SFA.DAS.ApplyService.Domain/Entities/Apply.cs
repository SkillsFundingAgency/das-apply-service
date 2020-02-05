using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class Apply : EntityBase
    {
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public string ApplicationStatus { get; set; }
        public string ReviewStatus { get; set; }
        public string GatewayReviewStatus { get; set; }
        public ApplyData ApplyData { get; set; }
    }

    public class ApplyData
    {
        public List<ApplySequence> Sequences { get; set; }
        public ApplyDetails ApplyDetails { get; set; }

        public FinancialReviewDetails FinancialReviewDetails { get; set; }
    }

    public class ApplyDetails
    {
        // NOTE THIS IS A SIMILAR COPY OF RoatpApplicationData
        public string ReferenceNumber { get; set; }
        public string UKPRN { get; set; }
        public string OrganisationName { get; set; }
        public string TradingName { get; set; }
        public int ProviderRoute { get; set; } // was string - ApplicationRouteId
        public DateTime? ApplicationSubmittedOn { get; set; }
        public Guid? ApplicationSubmittedBy { get; set; }
    }

    public class ApplySequence
    {
        public Guid SequenceId { get; set; }
        public int SequenceNo { get; set; }
        public List<ApplySection> Sections { get; set; }
        //public string Status { get; set; }
        public bool IsActive { get; set; }
        public bool NotRequired { get; set; }
        //public bool Sequential { get; set; }
        //public string Description { get; set; }
        //public DateTime? ApprovedDate { get; set; }
        //public string ApprovedBy { get; set; }
    }

    public class ApplySection
    {
        public Guid SectionId { get; set; }
        public int SectionNo { get; set; }
        //public string Status { get; set; }
        //public DateTime? ReviewStartDate { get; set; }
        //public string ReviewedBy { get; set; }
        //public DateTime? EvaluatedDate { get; set; }
        //public string EvaluatedBy { get; set; }
        //public bool NotRequired { get; set; }
        //public bool? RequestedFeedbackAnswered { get; set; }
    }

    public class FinancialReviewDetails
    {
        public string SelectedGrade { get; set; }
        public DateTime? FinancialDueDate { get; set; }
        public string GradedBy { get; set; }
        public DateTime? GradedDateTime { get; set; }
        public string Comments { get; set; }
        public List<FinancialEvidence> FinancialEvidences { get; set; }
    }

    public class FinancialEvidence
    {
        public string Filename { get; set; }
    }
    
    public static class ApplicationStatus
    {
        public const string New = "New";
        public const string InProgress = "In Progress";
        public const string Submitted = "Submitted";
        public const string FeedbackAdded = "FeedbackAdded";
        public const string Resubmitted = "Resubmitted";
        public const string Declined = "Declined";
        public const string Approved = "Approved";
        public const string GatewayAssessed = "GatewayAssessed";
    }

    public static class ApplicationReviewStatus
    {
        public const string Draft = "Draft";
        public const string New = "New";
        public const string InProgress = "In Progress";
        public const string HasFeedback = "Has Feedback";
        public const string Approved = "Approved";
        public const string Declined = "Declined";
    }

    public static class GatewayReviewStatus
    {
        public const string Draft = "Draft";
        public const string New = "New";
        public const string InProgress = "In Progress";
        public const string Approved = "Approved";
        public const string Declined = "Declined";
    }

    public static class AssessorReviewStatus
    {
        public const string Draft = "Draft";
        public const string New = "New";
        public const string InProgress = "In Progress";
        public const string Approved = "Approved";
        public const string Declined = "Declined";
    }
}