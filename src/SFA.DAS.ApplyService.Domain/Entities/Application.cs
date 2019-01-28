using System;
using System.Collections.Generic;
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
        public ApplicationData ApplicationData { get; set; }
    }

    public class StandardApplicationData
    {
        public string StandardName { get; set; }
        public int StandardCode { get; set; }
        public string UserEmail { get; set; }
    }
    
    public class ApplicationStatus
    {
        public const string InProgress = "In Progress";
        public const string Submitted = "Submitted";
        public const string FeedbackAdded = "FeedbackAdded";
        public const string Rejected = "Rejected";
        public const string Approved = "Approved";
    }

    public class ApplicationData
    {
        public string ReferenceNumber { get; set; }
        public int StandardCode { get; set; }
        public string StandardName { get; set; }
        public List<InitSubmission> InitSubmissions { get; set; }
        public int InitSubmissionsCount { get; set; }
        public DateTime? LatestInitSubmissionDate { get; set; }
        public DateTime? InitSubmissionClosedDate { get; set; }
        public List<StandardSubmission> StandardSubmissions { get; set; }
        public int StandardSubmissionsCount { get; set; }
        public DateTime? LatestStandardSubmissionDate { get; set; }
        public DateTime? StandardSubmissionClosedDate { get; set; }
    }

    public abstract class Submission
    {
        public DateTime SubmittedAt { get; set; }
        public string SubmittedBy { get; set; }
    }

    public class InitSubmission : Submission
    {
    }

    public class StandardSubmission : Submission
    {
    }
}