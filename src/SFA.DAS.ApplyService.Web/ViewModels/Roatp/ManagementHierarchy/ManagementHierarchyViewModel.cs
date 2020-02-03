using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp.ManagementHierarchy
{
    public class ManagementHierarchyViewModel
    {
        public string SectionTitle { get => "Management hierarchy for apprenticeships"; }
        public List<ValidationErrorDetail> ErrorMessages { get; set; }
    }
}
