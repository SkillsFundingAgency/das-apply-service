using System;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public static class PeopleInControlValidator
    {
        public const string PersonInControlNameMinLengthError = "Enter a full name";
        public const string PersonInControlNameMaxLengthError = "Enter a full name using 255 characters or less";
        public const string DobFieldPrefix = "PersonInControlDob";

        public static List<ValidationErrorDetail> Validate(AddEditPeopleInControlViewModel model)
        {
            var errorMessages = new List<ValidationErrorDetail>();

            if (String.IsNullOrWhiteSpace(model.PersonInControlName)
                && String.IsNullOrWhiteSpace(model.PersonInControlDobMonth)
                && String.IsNullOrWhiteSpace(model.PersonInControlDobYear))
            {
                var message = $"Enter the individual's details";

                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = message,
                        Field = "PersonInControlName"
                    });

                return errorMessages; 
            }
            
            if (String.IsNullOrEmpty(model.PersonInControlName))
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = PersonInControlNameMinLengthError,
                        Field = "PartnerName"
                    });
            }
            else if (model.PersonInControlName.Length > 255)
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = PersonInControlNameMaxLengthError,
                        Field = "PartnerName"
                    });
            }

            var dobErrorMessages = DateOfBirthAnswerValidator.ValidateDateOfBirth(model.PersonInControlDobMonth, model.PersonInControlDobYear, DobFieldPrefix);
            if (dobErrorMessages.Any())
            {
                errorMessages.AddRange(dobErrorMessages);
            }

            return errorMessages;
        }
    }
}
