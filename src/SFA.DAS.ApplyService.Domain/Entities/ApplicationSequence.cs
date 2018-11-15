using System;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class ApplicationSequence : EntityBase
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public int SequenceId { get; set; }
        public string Status { get; set; }
    }
}