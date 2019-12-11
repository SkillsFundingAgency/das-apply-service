
using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class WhosInControlViewModel
    {
        public string SectionTitle { get => "Tell us who's in control"; }
        public List<ValidationErrorDetail> ErrorMessages { get; set; }
    }
}
