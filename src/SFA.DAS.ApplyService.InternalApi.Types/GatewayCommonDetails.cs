using System;

namespace SFA.DAS.ApplyService.InternalApi.Types
{
    public class GatewayCommonDetails
    {
        public Guid ApplicationId { get; set; }
        public string Ukprn { get; set; }
        public DateTime? ApplicationSubmittedOn { get; set; }
        public DateTime? GatewayOutcomeMadeOn { get; set; }
        public string GatewayOutcomeMadeBy { get; set; }
        public DateTime? SourcesCheckedOn { get; set; }
        public string LegalName { get; set; }
        public string ProviderRouteName { get; set; }
        public string ApplicationStatus { get; set; }
        public string GatewayReviewStatus { get; set; }

        // The below properties are related to the requested page
        public string PageId { get; set; }
        public string OptionClarificationText { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public DateTime? OutcomeMadeOn { get; set; }
        public string OutcomeMadeBy { get; set; }

        public string GatewaySubcontractorDeclarationClarificationUpload { get; set; }
    }
}
