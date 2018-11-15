namespace SFA.DAS.ApplyService.InternalApi.Types
{
    public class CreateOrganisationRequest
    {
        public string Name { get; set; }
        public string OrganisationType { get; set; }
        public int? OrganisationUkprn { get; set; }

        public OrganisationDetails OrganisationDetails { get; set; }

        public string CreatedBy { get; set; }
        public string PrimaryContactEmail { get; set; }
    }

    public class OrganisationDetails
    {
        public string OrganisationReferenceType { get; set; } // "RoEPAO", "RoATP" or "EASAPI"
        public string OrganisationReferenceId { get; set; } // CSV list of known id's

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
    }
}
