using System;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp.Appeals
{
    public class AppealSubmittedViewModel
    {
        public Guid ApplicationId { get; set; }
        public DateTime AppealSubmittedDate => DateTime.Today;
    }
}
