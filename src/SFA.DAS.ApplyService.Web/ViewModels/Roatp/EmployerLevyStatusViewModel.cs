using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class EmployerLevyStatusViewModel
    {
        public string UKPRN { get; set; }
        public int ApplicationRouteId { get; set; }

        [Required(ErrorMessage = "Tell us if your organisation is a levy-paying employer")]
        public string LevyPayingEmployer { get; set; }

        public List<ValidationErrorDetail> ErrorMessages { get; set; }
    }
}
