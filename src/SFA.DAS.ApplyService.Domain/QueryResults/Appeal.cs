using SFA.DAS.ApplyService.Types;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Domain.QueryResults
{
    public class Appeal
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string Status { get; set; }
        public string HowFailedOnPolicyOrProcesses { get; set; }
        public string HowFailedOnEvidenceSubmitted { get; set; }
        public DateTime? AppealSubmittedDate { get; set; }
        public DateTime? AppealDeterminedDate { get; set; }
        public string InternalComments { get; set; }
        public string ExternalComments { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? InProgressDate { get; set; }
        public string InProgressUserId { get; set; }
        public string InProgressUserName { get; set; }
        public string InProgressInternalComments { get; set; }
        public string InProgressExternalComments { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public List<AppealFile> AppealFiles { get; set; }
    }
}
