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
    
    public class ApplicationData
    {
        public string ReferenceNumber { get; set; }
        public List<InitSubmission> InitSubmissions { get; set; }
        public int InitSubmissionsCount { get; set; }
        public DateTime? LatestInitSubmissionDate { get; set; }
        public DateTime? InitSubmissionFeedbackAddedDate { get; set; }
        public DateTime? InitSubmissionClosedDate { get; set; }
    }

    public abstract class Submission
    {
        public DateTime SubmittedAt { get; set; }
        public Guid SubmittedBy { get; set; }
        public string SubmittedByEmail { get; set; }
    }

    public class InitSubmission : Submission
    {
    }
}