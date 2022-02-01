using System;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class OrganisationAddresses 
    {
        public int Id { get; set; }
        public Guid OrganisationId { get; set; }
        public int AddressType { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
    }
}
