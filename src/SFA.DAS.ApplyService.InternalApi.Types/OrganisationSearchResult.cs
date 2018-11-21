﻿namespace SFA.DAS.ApplyService.InternalApi.Types
{
    public class OrganisationSearchResult
    {
        public string Id { get; set; }
        public int? Ukprn { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public OrganisationAddress Address { get; set; }
        public string OrganisationType { get; set; }

        public string OrganisationReferenceType { get; set; }
        public string OrganisationReferenceId { get; set; }
    }

    public class OrganisationAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
    }
}
