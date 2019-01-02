using System;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class Application : EntityBase
    {
        public Organisation ApplyingOrganisation { get; set; }
        public Guid ApplyingOrganisationId { get; set; }
        public DateTime WithdrawnAt { get; set; }
        public string WithdrawnBy { get; set; }
        public string ApplicationStatus { get; set; }
        public string ApplicationData { get; set; }
    }

    public class StandardApplicationData
    {
        public string StandardName { get; set; }
    }
    
    public class ApplicationStatus
    {
        public const string InProgress = "In Progress";
        public const string Submitted = "Submitted";
        public const string FeedbackAdded = "FeedbackAdded";
        public const string Rejected = "Rejected";
        public const string Approved = "Approved";
    }
}