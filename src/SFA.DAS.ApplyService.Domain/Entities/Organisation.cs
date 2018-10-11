using System;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class Organisation : EntityBase
    {
        public string Name { get; set; }
        public string OrganisationType { get; set; }
        public string ApplicationType { get; set; }
        public string OrganisationDetails { get; set; }
        public Guid ApplyOrganisationId { get; set; }
    }
}