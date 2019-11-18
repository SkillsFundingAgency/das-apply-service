using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class SearchByUkprnViewModel
    {
        public string UKPRN { get; set; }
        public List<ValidationErrorDetail> ErrorMessages { get; set; }
    }
}
