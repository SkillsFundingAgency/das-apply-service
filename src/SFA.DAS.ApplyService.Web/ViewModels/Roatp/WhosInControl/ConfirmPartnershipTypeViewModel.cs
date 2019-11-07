using System;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ConfirmPartnershipTypeViewModel : WhosInControlViewModel
    {
        public const string PartnershipTypeIndividual = "Individual";
        public const string PartnershipTypeOrganisation = "Organisation";

        public Guid ApplicationId { get; set; }
        [Required(ErrorMessage = "Tell us what your organisation's partner is")]
        public string PartnershipType { get; set; }
    }
}
