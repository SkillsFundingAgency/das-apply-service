using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ExistingAccountViewModel
    {
        [Required(ErrorMessage ="Tell us if this is your first time using the apprenticeship service (AS) sign in")]
        public string FirstTimeSignin { get; set; }

        public List<ValidationErrorDetail> ErrorMessages { get; set; }
    }
}
