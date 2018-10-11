using System;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    class Entity : EntityBase
    {
        public Guid ApplyingOrganisationId { get; set; }
        public string QnAData { get; set; }
        public DateTime WithdrawnAt { get; set; }
        public string WithdrawnBy { get; set; }
    }
}