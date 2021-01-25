using System;
using SFA.DAS.ApplyService.Application.Interfaces;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class OversightReview: IAuditable
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }   
        public bool? GatewayApproved { get; set; }
        public bool? ModerationApproved { get; set; }
        public string Status { get; set; }
        public DateTime ApplicationDeterminedDate { get; set; }
        public string InternalComments { get; set; }
        public string ExternalComments { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedOn { get; set; }

        public OversightReview()
        {
            Id = Guid.NewGuid();
            CreatedOn = DateTime.UtcNow;
            ApplicationDeterminedDate = DateTime.UtcNow.Date;
        }
    };
}
