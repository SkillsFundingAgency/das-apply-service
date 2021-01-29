using SFA.DAS.ApplyService.Domain.CharityCommission;
using SFA.DAS.ApplyService.Domain.CompaniesHouse;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Domain.Ukrlp;
using System;
using System.Collections.Generic;
using SFA.DAS.ApplyService.Application.Interfaces;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class Apply : EntityBase, IAuditable
    {
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }

        public string ApplicationStatus { get; set; }
        public string AssessorReviewStatus { get; set; }
        public string GatewayReviewStatus { get; set; }
        public string FinancialReviewStatus { get; set; }
        public string ModerationStatus { get; set; }
        public string OversightStatus { get; set; }

        public ApplyData ApplyData { get; set; }
        public FinancialReviewDetails FinancialGrade { get; set; }

        public string Assessor1UserId { get; set; }
        public string Assessor1Name{ get; set; }
        public string Assessor1ReviewStatus { get; set; }
        public string Assessor2UserId { get; set; }
        public string Assessor2Name { get; set; }
        public string Assessor2ReviewStatus { get; set; }

        public string GatewayUserId { get; set; }
        public string GatewayUserName { get; set; }

        public string Comments { get; set; }
        public string ExternalComments { get; set; }
    }

    public class ApplyData
    {
        public List<ApplySequence> Sequences { get; set; }
        public ApplyDetails ApplyDetails { get; set; }
        public ApplyGatewayDetails GatewayReviewDetails { get; set; }
        public ModeratorReviewDetails ModeratorReviewDetails { get; set; }
    }

    public class ApplyDetails
    {
        public string ReferenceNumber { get; set; }
        public string UKPRN { get; set; }
        public string OrganisationName { get; set; }
        public string TradingName { get; set; }
        public int ProviderRoute { get; set; } 
        public string ProviderRouteName { get; set; }
        public DateTime? ApplicationSubmittedOn { get; set; }
        public Guid? ApplicationSubmittedBy { get; set; }
        public DateTime? ApplicationWithdrawnOn { get; set; }
        public string ApplicationWithdrawnBy { get; set; }
        public DateTime? ApplicationRemovedOn { get; set; }
        public string ApplicationRemovedBy { get; set; }
        public int? ProviderRouteOnRegister { get; set; }
        public string ProviderRouteNameOnRegister { get; set; }
        public string OrganisationType { get; set; }
        public string Address { get; set; }
    }


    // This is an application-level storage of external details for processing gateway reviews
    public class ApplyGatewayDetails
    {
        public ProviderDetails UkrlpDetails { get; set; }
        public CompaniesHouseSummary CompaniesHouseDetails { get; set; }
        public CharityCommissionSummary CharityCommissionDetails { get; set; }
        public OrganisationRegisterStatus RoatpRegisterDetails { get; set; }
        public DateTime? SourcesCheckedOn { get; set; }
        public string Comments { get; set; }
        public string ExternalComments { get; set; }
        public DateTime? OutcomeDateTime { get; set; }
        public DateTime? ClarificationRequestedOn { get; set; }
        public string ClarificationRequestedBy { get; set; }
        public string GatewaySubcontractorDeclarationClarificationUpload { get; set; }

    }

    public class ApplySequence
    {
        public Guid SequenceId { get; set; }
        public int SequenceNo { get; set; }
        public List<ApplySection> Sections { get; set; }
        public string Status { get; set; }
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
        public string Status { get; set; }
        //public DateTime? ReviewStartDate { get; set; }
        //public string ReviewedBy { get; set; }
        //public DateTime? EvaluatedDate { get; set; }
        //public string EvaluatedBy { get; set; }
        public bool NotRequired { get; set; }
        //public bool? RequestedFeedbackAnswered { get; set; }
    }

    public class FinancialReviewDetails
    {
        public string SelectedGrade { get; set; }
        public DateTime? FinancialDueDate { get; set; }
        public string GradedBy { get; set; }
        public DateTime? GradedDateTime { get; set; }
        public string Comments { get; set; }
        public string ExternalComments { get; set; }
        public List<FinancialEvidence> FinancialEvidences { get; set; }
        public List<ClarificationFile> ClarificationFiles { get; set; }
        public DateTime? ClarificationRequestedOn { get; set; }
        public string ClarificationRequestedBy { get; set; }
        public string ClarificationResponse { get; set; }
    }

    public class FinancialEvidence
    {
        public string Filename { get; set; }
    }


    public class ClarificationFile
    {
        public string Filename { get; set; }
    }

    public class ModeratorReviewDetails
    {
        public string ModeratorName { get; set; }
        public string ModeratorUserId { get; set; }
        public DateTime? OutcomeDateTime { get; set; }
        public DateTime? ClarificationRequestedOn { get; set; }
        public string ModeratorComments { get; set; }
    }

    public class FinancialData
    {
        public Guid ApplicationId { get; set; }
        public long? TurnOver { get; set; }
        public long? Depreciation { get; set; }
        public long? ProfitLoss { get; set; }
        public long? Dividends { get; set; }
        public long? IntangibleAssets { get; set; }
        public long? Assets { get; set; }
        public long? Liabilities { get; set; }
        public long? ShareholderFunds { get; set; }
        public long? Borrowings { get; set; }
    }

    public class ApplicationStatus	
    {
        public const string New = "New";
        public const string InProgress = "In Progress";	
        public const string Submitted = "Submitted";
        public const string Resubmitted = "Resubmitted";
        public const string GatewayAssessed = "GatewayAssessed";	
        public const string FeedbackAdded = "FeedbackAdded";	
        public const string Rejected = "Rejected";	
        public const string Approved = "Approved";	
        public const string Cancelled = "Cancelled";
        public const string Withdrawn = "Withdrawn";
        public const string Removed = "Removed";

        // Below are other statuses mentioned in the most recent status documentation
        // Please check the flow in RoatpApplicationController, under the line ' switch (application.ApplicationStatus)' if you add new statuses
        // GWResubmitted
        //PMOModerationInProgress
        //PMOModerationAssessed
        //OversightInProgress - probably not needed
        // Withdrawn
        // Reopened
        // GWFeedbackAdded
        // InAssessment
    }

    public class GatewayAnswerStatus
    {
        public const string Pass = "Pass";
        public const string Fail = "Fail";
        public const string InProgress = "In progress"; // TECH DEBT: Correct capitalization
        public const string Clarification = "Clarification";
    }

    public static class GatewayReviewStatus
    {
        public const string Draft = "Draft";
        public const string New = "New";
        public const string InProgress = "In Progress";
        public const string ClarificationSent = "Clarification sent"; // TECH DEBT: Correct capitalization
        public const string Resubmitted = "Re-submitted"; // TECH DEBT: Correct to "Resubmitted"
        public const string Fail = "Fail";
        public const string Pass = "Pass";
        public const string Reject = "Reject";
    }

    public static class AssessorReviewStatus
    {
        public const string Draft = "Draft";
        public const string New = "New";
        public const string InProgress = "In Progress";
        public const string HasFeedback = "Has Feedback";
        public const string Approved = "Approved";
        public const string Declined = "Declined";
        public const string NotRequired = "Not Required";
    }

    public static class FinancialReviewStatus
    {
        public const string Draft = "Draft";
        public const string New = "New";
        public const string InProgress = "In Progress";

        public const string ClarificationSent = "Clarification Sent";
        public const string Resubmitted = "Resubmitted";

        public const string Pass = "Pass";
        public const string Fail = "Fail";
        public const string Exempt = "Exempt";
    }

   


    public static class SequenceReviewStatus
    {
        public const string New = "";
        public const string InProgress = "In Progress";
        public const string Evaluated = "Evaluated";
    }

    public static class OversightReviewStatus
    {
        public const string New = "New";
        public const string InProgress = "In Progress";
        public const string Successful = "Successful";
        public const string Unsuccessful = "Unsuccessful";
        public const string SuccessfulAlreadyActive = "Successful - already active";
        public const string SuccessfulFitnessForFunding = "Successful - fitness for funding";
        public const string Rejected = "Rejected";
    }
   
}