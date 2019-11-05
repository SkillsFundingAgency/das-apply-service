using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class AddEditPeopleInControlViewModel
    {
        public const string DobFieldPrefix = "PersonInControlDob";

        public Guid ApplicationId { get; set; }
        public List<ValidationErrorDetail> ErrorMessages { get; set; }

        [Required(ErrorMessage = "Enter a full name")]
        [MaxLength(255, ErrorMessage = "Enter a full name using 255 characters or less")]
        public string PersonInControlName { get; set; }
        public string PersonInControlDobMonth { get; set; }
        public string PersonInControlDobYear { get; set; }
        public int Index { get; set; }
    }
}
