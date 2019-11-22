using System;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ConfirmTrusteesViewModel : WhosInControlViewModel
    {
        public bool VerifiedCompaniesHouse { get; set; }
        public Guid ApplicationId { get; set; }
        public PeopleInControl Trustees { get; set; }
    }
}
