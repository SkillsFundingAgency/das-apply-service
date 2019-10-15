using SFA.DAS.ApplyService.Domain.Roatp;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.ApplyService.Web.Services;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ChangeProviderRouteViewModel
    {
        public string UKPRN { get; set; }
        public ApplicationRoute CurrentProviderType { get; set; }

        public string CurrentProviderTypeText
        {
            get
            {
                if (CurrentProviderType == null)
                {
                    return string.Empty;
                }

                string prefix = "a";

                if (CurrentProviderType.RouteName.StartsWithVowel())
                {
                    prefix = "an";
                }

                return $"{prefix} {CurrentProviderType.RouteName.ToLower()}";
            }
        }
        
        [Required(ErrorMessage = "Tell us if your organisation wants to change provider route")]
        public string ChangeApplicationRoute { get; set; }
    }
}
