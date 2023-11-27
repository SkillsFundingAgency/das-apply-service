﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class EmployerProviderContinueApplicationViewModel : IPageViewModel
    {
        [Required(ErrorMessage = "Tell us if you want to continue with this application")]
        public string ContinueWithApplication { get; set; }

        public string LevyPayingEmployer { get; set; }

        public List<ValidationErrorDetail> ErrorMessages { get; set; }
        public string Title { get { return "Your organisation cannot apply to join the APAR as an employer provider"; } set { } }

        public Guid ApplicationId { get; set; }

        public int SequenceId { get; set; }
        public int SectionId { get; set; }
        public string PageId { get { return "IneligibleNonLevy"; } set { } }
        public string GetHelpQuestion { get; set; }
        public bool GetHelpQuerySubmitted { get; set; }
        public string GetHelpErrorMessage { get; set; }

        public string GetHelpAction { get { return "IneligibleNonLevy"; } set { } }
    }
}
