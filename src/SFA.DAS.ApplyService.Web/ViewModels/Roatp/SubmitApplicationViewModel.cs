using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class SubmitApplicationViewModel
    {
        public Guid ApplicationId { get; set; }
        public string OrganisationName { get; set; }
        [Required(ErrorMessage = "You must confirm that all the answers and uploads in this application are accurate and to the best of your knowledge")]
        public string ConfirmSubmitApplication { get; set; }

        public List<ValidationErrorDetail> ErrorMessages { get; set; }
    }
}
