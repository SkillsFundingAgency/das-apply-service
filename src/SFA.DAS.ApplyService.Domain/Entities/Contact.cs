using System;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    class Contact : EntityBase
    {
        public string Email { get; set; }
        public string GivenNames { get; set; }
        public string FamilyName { get; set; }
        public Guid SigninId { get; set; }
        public string SigninType { get; set; }
        public Guid ApplyOrganisationId { get; set; }
    }
}