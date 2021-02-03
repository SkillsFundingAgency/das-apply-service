using System;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class ApplicationOversightDownloadDetails
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string OrganisationName { get; set; }

        public string Ukprn { get; set; }
        public string ProviderRoute { get; set; }
        public string ApplicationReferenceNumber { get; set; }
        public DateTime ApplicationSubmittedDate { get; set; }
        public string ApplicationStatus { get; set; }
        public DateTime? ApplicationDeterminedDate { get; set; }
        public string ProviderRouteNameOnRegister { get; set; }
        public string OrganisationType { get; set; }
        public string CompanyNumber { get; set; }
        public string Address { get; set; }
        public string GatewayOutcome { get; set; }
        public string AssessorOutcome { get; set; }
        public string FHCOutcome { get; set; }
        public string OverallOutcome { get; set; }
    }
}
