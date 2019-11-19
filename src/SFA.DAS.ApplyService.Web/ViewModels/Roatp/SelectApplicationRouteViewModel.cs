namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    using Domain.Roatp;
    using SFA.DAS.ApplyService.Domain.Apply;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class SelectApplicationRouteViewModel
    {
        public IEnumerable<ApplicationRoute> ApplicationRoutes { get; set; }

        [Required(ErrorMessage = "Tell us your organisation's provider route")]
        [Range(1, 3, ErrorMessage = "Tell us your organisation's provider route")]
        public int ApplicationRouteId { get; set; }

        public List<ValidationErrorDetail> ErrorMessages { get; set; }
    }
}
