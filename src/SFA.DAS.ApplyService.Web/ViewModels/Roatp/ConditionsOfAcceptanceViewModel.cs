using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ConditionsOfAcceptanceViewModel
    {
        [Required(ErrorMessage = "Tell us if you agree to the terms and conditions of making an application")]
        public string ConditionsAccepted { get; set; }
        public int ApplicationRouteId { get; set; }

        public List<ValidationErrorDetail> ErrorMessages { get; set; }
    }
}
