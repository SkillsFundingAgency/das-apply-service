using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class ApplicationSequence : EntityBase
    {
        public Guid ApplicationId { get; set; }
        [JsonProperty("sequenceNo")]
        public int SequenceId { get; set; }
        public bool IsActive { get; set; }
        public List<ApplicationSection> Sections { get; set; }
        public bool NotRequired { get; set; }
        public string Description { get; set; }
        public bool Sequential { get; set; }
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
    }

    public static class SequenceId
    {
        public static int Preamble = 0;
        public static int Stage1 = 1;
        public static int Stage2 = 2;
    }
}