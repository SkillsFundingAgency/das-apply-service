using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ExistingAccountViewModel
    {
        [Required(ErrorMessage ="Tell us if you have an apprenticeship service (AS) sign in account")]
        public bool? FirstTimeSignin { get; set; }
      
    }
}
