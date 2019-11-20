using System;

namespace SFA.DAS.ApplyService.Domain.Entities.Review
{
    public class Gateway
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string Status { get; set; }
    }
}
