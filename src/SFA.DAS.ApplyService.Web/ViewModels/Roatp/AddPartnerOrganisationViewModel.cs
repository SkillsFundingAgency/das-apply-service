using System;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class AddPartnerOrganisationViewModel
    {
        public Guid ApplicationId { get; set; }
        [Required(ErrorMessage = "Enter the partner organisation's name")]
        [MaxLength(255, ErrorMessage = "Enter a full name using 255 characters or less")]
        public string OrganisationName { get; set; }
    }
}
