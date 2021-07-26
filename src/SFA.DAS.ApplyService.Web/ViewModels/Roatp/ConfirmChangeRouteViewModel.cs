using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ConfirmChangeRouteViewModel : IPageViewModel
    {
        [Required(ErrorMessage = "Tell us if your organisation wants to change provider route")]
        public string ConfirmChangeRoute { get; set; }

        public List<ValidationErrorDetail> ErrorMessages { get; set; }
        public string Title { get { return "Changing your provider route"; } set { } }

        public Guid ApplicationId { get; set; }

        public int SequenceId { get; set; }
        public int SectionId { get; set; }
        public string PageId { get; set; }
        public string GetHelpQuestion { get; set; }
        public bool GetHelpQuerySubmitted { get; set; }
        public string GetHelpErrorMessage { get; set; }
        
        public string GetHelpAction { get { return "ConfirmChangeRoute"; } set { } }
    }
}
