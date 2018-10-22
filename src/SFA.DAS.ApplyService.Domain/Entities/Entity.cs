using System;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class Entity : EntityBase
    {
        public Guid ApplyingOrganisationId { get; set; }
        public string QnAData { get; set; }

        public Workflow QnAWorkflow
        {
            get => JsonConvert.DeserializeObject<Workflow>(QnAData);
            set => QnAData = JsonConvert.SerializeObject(value);
        }

        public DateTime WithdrawnAt { get; set; }
        public string WithdrawnBy { get; set; }
        public string ApplicationStatus { get; set; }
    }
}