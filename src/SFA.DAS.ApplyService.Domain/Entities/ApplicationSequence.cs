using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class ApplicationSequence : EntityBase
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public SequenceId SequenceId { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public List<ApplicationSection> Sections { get; set; }
    }

    public class ApplicationSequenceStatus
    {
        public const string Draft = "Draft";
        public const string Submitted = "Submitted";
        public const string InProgress = "InProgress";
        public const string FeedbackAdded = "FeedbackAdded";
        public const string Rejected = "Rejected";
        public const string Approved = "Approved";
    }

    public enum SequenceId
    {
        Stage1 = 1,
        Stage2 = 2
    }
}