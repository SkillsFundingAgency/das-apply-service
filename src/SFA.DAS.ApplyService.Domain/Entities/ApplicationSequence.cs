using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class ApplicationSequence : EntityBase
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public int SequenceId { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public List<ApplicationSection> Sections { get; set; }
    }

    public class ApplicationSequenceStatus
    {
        public const string Draft = "Draft";
        public const string Submitted = "Submitted";
    }
}