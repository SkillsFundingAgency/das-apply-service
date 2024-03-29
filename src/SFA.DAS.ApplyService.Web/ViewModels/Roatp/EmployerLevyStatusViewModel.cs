﻿using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class EmployerLevyStatusViewModel : IPageViewModel
    {
        public string UKPRN { get; set; }
        public int ApplicationRouteId { get; set; }

        [Required(ErrorMessage = "Tell us if your organisation is a levy-paying employer")]
        public string LevyPayingEmployer { get; set; }

        public List<ValidationErrorDetail> ErrorMessages { get; set; }
        public string Title { get { return "Is your organisation a levy-paying employer?"; } set { } }

        public Guid ApplicationId { get; set; }

        public int SequenceId { get; set; }
        public int SectionId { get; set; }
        public string PageId { get { return "ConfirmLevyStatus"; } set { } }
        public string GetHelpQuestion { get; set; }
        public bool GetHelpQuerySubmitted { get; set; }
        public string GetHelpErrorMessage { get; set; }
        
        public string GetHelpAction { get { return "ConfirmLevyStatus"; } set { } }
    }
}
