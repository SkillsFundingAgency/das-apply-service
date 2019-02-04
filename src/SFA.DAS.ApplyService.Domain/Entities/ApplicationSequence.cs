using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public const string InProgress = "In Progress";
        public const string FeedbackAdded = "FeedbackAdded";
        public const string Resubmitted = "Resubmitted";
        public const string Rejected = "Rejected";
        public const string Approved = "Approved";
        public const string NotRequired = "NotRequired";
    }

    public enum SequenceId
    {
        Stage1 = 1,
        Stage2 = 2
    }
}