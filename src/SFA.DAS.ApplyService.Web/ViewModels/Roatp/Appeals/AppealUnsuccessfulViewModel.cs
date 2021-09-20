using System;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp.Appeals
{
    public class AppealInProgressViewModel
    {
        public Guid ApplicationId { get; set; }
        public DateTime AppealSubmittedDate { get; set; }
        public string ExternalComments { get; set; }
    }
}
