using System;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class SoleTraderOrPartnershipViewModel
    {
        public Guid ApplicationId { get; set; }
        [Required(ErrorMessage = "Tell us what your organisation is")]
        public string OrganisationType { get; set; }
    }
}
