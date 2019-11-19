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
                        
            if (String.IsNullOrEmpty(model.PersonInControlName))
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = PersonInControlNameMinLengthError,
                        Field = "PersonInControlName"
                    });
            }
            else if (model.PersonInControlName.Length > 255)
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = PersonInControlNameMaxLengthError,
                        Field = "PersonInControlName"
                    });
            }

            if (!model.DateOfBirthOptional)
            {
                var dobErrorMessages = DateOfBirthAnswerValidator.ValidateDateOfBirth(model.PersonInControlDobMonth, model.PersonInControlDobYear, DobFieldPrefix);
                if (dobErrorMessages.Any())
                {
                    errorMessages.AddRange(dobErrorMessages);
                }
            }

            return errorMessages;
        }
    }
}
