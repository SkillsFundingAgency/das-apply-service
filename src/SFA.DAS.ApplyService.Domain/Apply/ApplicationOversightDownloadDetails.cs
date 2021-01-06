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
        public string OversightStatus { get; set; }
        public string ApplicationStatus { get; set; }
        public DateTime? ApplicationDeterminedDate { get; set; }
        public string ProviderRouteNameOnRegister { get; set; }
        public string OrganisationType { get; set; }
    }
}
