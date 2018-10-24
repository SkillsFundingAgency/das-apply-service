using System;

namespace SFA.DAS.ApplyService.InternalApi.Models.ReferenceData
{
    public class Organisation
    {
        public string Name { get; set; }
        public OrganisationType Type { get; set; }
        public string Code { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public Address Address { get; set; }
        public string Sector { get; set; }
    }
}
