using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class AddEditPartnerViewModel
    {
        public Guid ApplicationId { get; set; }
        public List<ValidationErrorDetail> ErrorMessages { get; set; }

        [MaxLength(255, ErrorMessage = "Enter a full name using 255 characters or less")]
        public string PartnerName { get; set; }
        public string PartnerDobMonth { get; set; }
        public string PartnerDobYear { get; set; }
    }
}
