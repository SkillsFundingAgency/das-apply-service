using System;
using SFA.DAS.ApplyService.Types;

namespace SFA.DAS.ApplyService.InternalApi.Types.QueryResults
{
    public class ApplicationOversightDetails
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string OrganisationName { get; set; }

        public string Ukprn { get; set; }
        public string ProviderRoute { get; set; }
        public string ApplicationReferenceNumber { get; set; }
        public DateTime ApplicationSubmittedDate { get; set; }
        public string ApplicationStatus { get; set; }
        public OversightReviewStatus OversightStatus { get; set; }
       
        public DateTime? ApplicationDeterminedDate { get; set; }
        public string OversightUserName { get; set; }

        public string ApplicationEmailAddress { get; set; }
        public string AssessorReviewStatus { get; set; } 

        public string GatewayReviewStatus { get; set; }
        public DateTime? GatewayOutcomeMadeDate { get; set; }

        public string GatewayOutcomeMadeBy { get; set; }
        public string GatewayComments { get; set; }
        public string GatewayExternalComments { get; set; }
        public string FinancialReviewStatus { get; set; }
        public string FinancialGradeAwarded { get; set; }

        public DateTime? FinancialHealthAssessedOn { get; set; }
        public string FinancialHealthAssessedBy { get; set; }

        public string FinancialHealthComments { get; set; }
        public string FinancialHealthExternalComments { get; set; }
        public string ModerationReviewStatus { get; set; }

        public DateTime? ModerationOutcomeMadeOn { get; set; }
        public string ModeratedBy { get; set; }
        public string ModerationComments { get; set; }
    }
}
