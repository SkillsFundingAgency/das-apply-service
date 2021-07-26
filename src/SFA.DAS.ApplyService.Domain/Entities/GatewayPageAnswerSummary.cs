using System;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class GatewayPageAnswerSummary
    {
        public Guid ApplicationId { get; set; }
        public string PageId { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
    }
}