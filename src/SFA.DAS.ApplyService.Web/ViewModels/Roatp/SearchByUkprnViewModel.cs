namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    using System.ComponentModel.DataAnnotations;
    
    public class SearchByUkprnViewModel
    {
        [Range(1, 3, ErrorMessage = "Select an application route")]
        public int ApplicationRouteId { get; set; }

        public string UKPRN { get; set; }
    }
}
