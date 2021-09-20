using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp.Appeals
{
    public class AppealSubmittedViewModel
    {
        public Guid ApplicationId { get; set; }
        public DateTime AppealSubmittedDate { get; set; }
        public string HowFailedOnEvidenceSubmitted { get; set; }
        public string HowFailedOnPolicyOrProcesses { get; set; }
        public List<InternalApi.Types.Responses.Appeals.AppealFile> AppealFiles { get; set; }
    }
}
