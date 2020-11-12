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
        public string EmailAddress { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Tell us if you confirm that all your answers and file uploads are true and accurate to the best of your knowledge")]
        public bool ConfirmSubmitApplication { get; set; }
        [Range(typeof(bool), "true", "true", ErrorMessage = "Tell us if you agree to give further information on any application answers within 5 working days when requested by ESFA")]
        public bool ConfirmFurtherInfoSubmitApplication { get; set; }
        [Range(typeof(bool), "true", "true", ErrorMessage = "Tell us if you understand where communication about this application will be sent to")]
        public bool ConfirmFurtherCommunicationSubmitApplication { get; set; }

        public List<ValidationErrorDetail> ErrorMessages { get; set; }
    }
}