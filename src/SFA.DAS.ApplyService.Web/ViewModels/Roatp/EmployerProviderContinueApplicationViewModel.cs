using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class EmployerProviderContinueApplicationViewModel : IPageViewModel
    {
        [Required(ErrorMessage = "Tell us if you want to continue with this application")]
        public string ContinueWithApplication { get; set; }

        public string LevyPayingEmployer { get; set; }

        public List<ValidationErrorDetail> ErrorMessages { get; set; }
        public string Title { get { return "Your organisation cannot apply to join the RoATP as an employer provider"; }  set { } }

        public Guid ApplicationId { get; set; }

        public string SequenceId { get; set; }
        public int SectionId { get; set; }
        public string PageId { get; set; }
        public string GetHelpQuestion { get; set; }
        public bool GetHelpQuerySubmitted { get; set; }
        public string GetHelpErrorMessage { get; set; }
        
        public string GetHelpAction { get { return "IneligibleNonLevy"; } set { } }
    }
}
