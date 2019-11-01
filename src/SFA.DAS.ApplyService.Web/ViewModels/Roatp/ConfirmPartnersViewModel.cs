using SFA.DAS.ApplyService.Domain.Apply;
using System;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ConfirmPartnersViewModel
    {
        public Guid ApplicationId { get; set; }
        public string BackAction { get; set; }
        public TabularData PartnerData { get; set; }
    }
}
