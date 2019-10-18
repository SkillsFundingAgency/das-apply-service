using System;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ConfirmTrusteesViewModel
    {
        public Guid ApplicationId { get; set; }
        public PeopleInControl Trustees { get; set; }
    }
}
