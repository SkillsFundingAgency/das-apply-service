using System;
using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ConditionsOfAcceptanceViewModel
    {
        [Required(ErrorMessage = "Tell us if you accept the conditions of acceptance to join the Register")]
        public string ConditionsAccepted { get; set; }
        public int ApplicationRouteId { get; set; }
        public Guid ApplicationId { get; set; }
        public List<ValidationErrorDetail> ErrorMessages { get; set; }
    }
}
