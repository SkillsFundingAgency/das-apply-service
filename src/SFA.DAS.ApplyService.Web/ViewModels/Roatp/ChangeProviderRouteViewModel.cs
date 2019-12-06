using SFA.DAS.ApplyService.Domain.Roatp;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.ApplyService.Web.Services;
using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Apply;
using System;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ChangeProviderRouteViewModel : IPageViewModel
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

        public List<ValidationErrorDetail> ErrorMessages { get; set; }
        public string Title { get { return "Your organisation is already on the RoATP"; } set { } }

        public Guid ApplicationId { get; }
        public string SequenceId { get; set; }
        public int SectionId { get; set; }
        public string PageId { get; set; }
        public string GetHelpQuestion { get; set; }
        public bool GetHelpQuerySubmitted { get; set; }
        public string GetHelpErrorMessage { get; set; }
    }
}
