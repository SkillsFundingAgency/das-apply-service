using SFA.DAS.ApplyService.Domain.Roatp;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ChangeProviderRouteViewModel
    {
        public string UKPRN { get; set; }
        public ApplicationRoute CurrentProviderType { get; set; }

        public string IndefiniteArticle
        {
            get
            {
                if (CurrentProviderType == null)
                {
                    return string.Empty;
                }

                if (CurrentProviderType.Id == ApplicationRoute.EmployerProviderApplicationRoute)
                {
                    return "an";
                }
                     
                return "a";
            }
        }

        [Required(ErrorMessage = "Tell us if your organisation wants to change provider route")]
        public string ChangeApplicationRoute { get; set; }
    }
}
