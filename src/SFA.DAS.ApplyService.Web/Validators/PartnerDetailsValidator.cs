using System;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public static class PartnerDetailsValidator
    {
        public const string NoDetailsValidationError = "Enter the individual's details";
        public const string PartnerNameMinLengthError = "Enter a full name";
        public const string PartnerNameMaxLengthError = "Enter a full name using 255 characters or less";
        public const string DobFieldPrefix = "PartnerDob";

        public static List<ValidationErrorDetail> Validate(AddPartnerIndividualViewModel model)
        {
            var errorMessages = new List<ValidationErrorDetail>();

            if (String.IsNullOrWhiteSpace(model.PartnerName)
                && String.IsNullOrWhiteSpace(model.PartnerDobMonth)
                && String.IsNullOrWhiteSpace(model.PartnerDobYear))
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = NoDetailsValidationError,
                        Field = "PartnerName"
                    });

                return errorMessages; 
            }
            
            if (String.IsNullOrEmpty(model.PartnerName))
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = PartnerNameMinLengthError,
                        Field = "PartnerName"
                    });
            }
            else if (model.PartnerName.Length > 255)
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = PartnerNameMaxLengthError,
                        Field = "PartnerName"
                    });
            }

            var dobMonth = new Answer { Value = model.PartnerDobMonth };
            var dobYear = new Answer { Value = model.PartnerDobYear };
            var dobErrorMessages = DateOfBirthAnswerValidator.ValidateDateOfBirth(dobMonth, dobYear, DobFieldPrefix);
            if (dobErrorMessages.Any())
            {
                errorMessages.AddRange(dobErrorMessages);
            }

            return errorMessages;
        }
    }
}
