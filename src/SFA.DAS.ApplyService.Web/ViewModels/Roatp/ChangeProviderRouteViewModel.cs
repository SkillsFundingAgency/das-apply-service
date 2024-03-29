﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Web.Services;

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
        public string Title { get { return "Your organisation is already on the APAR"; } set { } }

        public Guid ApplicationId { get; }
        public int SequenceId { get; set; }
        public int SectionId { get; set; }
        public string PageId { get { return "AlreadyOnRegister"; } set { } }
        public string GetHelpQuestion { get; set; }
        public bool GetHelpQuerySubmitted { get; set; }
        public string GetHelpErrorMessage { get; set; }

        public string GetHelpAction { get { return "ProviderAlreadyOnRegister"; } set { } }
    }
}
