using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class SubmitApplicationViewModel
    {
        public Guid ApplicationId { get; set; }
        public string OrganisationName { get; set; }
        public string EmailAddress { get; set; }

        public string Title => $"Submit application on behalf of {OrganisationName}";

        [Range(typeof(bool), "true", "true", ErrorMessage = "Tell us if you confirm that all your answers and file uploads are true and accurate to the best of your knowledge")]
        public bool ConfirmSubmitApplication { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Tell us if you confirm that all information and evidence is specific to your organisation and you have not used any information or answers from any other organisation's application to the Register")]
        public bool ConfirmOrganisationSpecificSubmitApplication { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Tell us if you agree to give further information on any application answers within 5 working days when requested by DfE")]
        public bool ConfirmFurtherInfoSubmitApplication { get; set; }



        [Range(typeof(bool), "true", "true", ErrorMessage = "Tell us if you understand that you must notify DfE 12 weeks before there is any change of ownership or control within your organisation")]
        public bool ConfirmChangeOfOwnershipSubmitApplication { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Tell us if you understand where communication about this application will be sent to")]
        public bool ConfirmFurtherCommunicationSubmitApplication { get; set; }

        public List<ValidationErrorDetail> ErrorMessages { get; set; }
    }
}