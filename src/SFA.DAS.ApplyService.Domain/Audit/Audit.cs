using System;

namespace SFA.DAS.ApplyService.Domain.Audit
{
    public class Audit
    {
        public Guid Id { get; set; }
        public string EntityType { get; set; }
        public Guid EntityId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserAction { get; set; }
        public DateTime AuditDate { get; set; }
        public string InitialState { get; set; }
        public string UpdatedState { get; set; }
        public string Diff { get; set; }
        public Guid CorrelationId { get; set; }
    }
}
