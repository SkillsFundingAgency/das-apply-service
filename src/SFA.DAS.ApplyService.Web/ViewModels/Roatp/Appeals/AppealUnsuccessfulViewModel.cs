using System;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp.Appeals
{
    public class AppealUnsuccessfulViewModel
    {
        public Guid ApplicationId { get; set; }
        public DateTime AppealSubmittedDate { get; set; }
        public DateTime AppealDeterminedDate { get; set; }
        public bool AppealedOnEvidenceSubmitted { get; set; }
        public bool AppealedOnPolicyOrProcesses { get; set; }
        public string ExternalComments { get; set; }
    }
}
