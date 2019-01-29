using System;

namespace SFA.DAS.ApplyService.InternalApi.Types
{
    public class CreateOrganisationRequest
    {
        public string Name { get; set; }
        public string OrganisationType { get; set; }
        public int? OrganisationUkprn { get; set; }

        public bool RoEPAOApproved { get; set; }
        public bool RoATPApproved { get; set; }

        public OrganisationDetails OrganisationDetails { get; set; }

        public Guid CreatedBy { get; set; }
        public string PrimaryContactEmail { get; set; }
    }

    public class OrganisationDetails
    {
        public string OrganisationReferenceType { get; set; } // "RoEPAO", "RoATP" or "EASAPI"
        public string OrganisationReferenceId { get; set; } // CSV list of known id's

        public string LegalName { get; set; }
        public string TradingName { get; set; }
        public string ProviderName { get; set; }

        public string CompanyNumber { get; set; }
        public string CharityNumber { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
    }
}
