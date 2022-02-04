using System.Collections.Generic;
using System.Text.RegularExpressions;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.ViewModels;


namespace SFA.DAS.ApplyService.Web.Validators
{
    public static class CreateAccountViewModelValidator
    {
        public const string FirstNameMinLengthError = "Enter first name";
        public const string FirstNameMaxLengthError = "Enter a first name using 250 characters or less";

        public const string LastNameMinLengthError = "Enter last name";
        public const string LastNameMaxLengthError = "Enter a last name using 250 characters or less";

        public const string ValidEmailPattern = @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$";
        public const string EmailError = "Enter your email";
        public const string EmailErrorTooLong = "Enter your email using 256 characters or less";
        public const string EmailErrorWrongFormat = "Enter your email address using an acceptable format (eg name@location.com)";

        public static List<ValidationErrorDetail> Validate(CreateAccountViewModel model)
        {
            var errorMessages = new List<ValidationErrorDetail>();

            if (string.IsNullOrEmpty(model.GivenName))
            {
                errorMessages.Add(
                    new ValidationErrorDetail { ErrorMessage = FirstNameMinLengthError, Field = "GivenName" });
            }
            else if (model.GivenName.Length > 250)
            {
                errorMessages.Add(
                    new ValidationErrorDetail { ErrorMessage = FirstNameMaxLengthError, Field = "GivenName" });
            }

            if (string.IsNullOrEmpty(model.FamilyName))
            {
                errorMessages.Add(
                    new ValidationErrorDetail { ErrorMessage = LastNameMinLengthError, Field = "FamilyName" });
            }
            else if (model.FamilyName.Length > 250)
            {
                errorMessages.Add(
                    new ValidationErrorDetail { ErrorMessage = LastNameMaxLengthError, Field = "FamilyName" });
            }

            if (string.IsNullOrEmpty(model.Email))
            {
                errorMessages.Add(
                    new ValidationErrorDetail { ErrorMessage = EmailError, Field = "Email" });
            }
            else if (model.Email.Length > 256)
            {
                errorMessages.Add(
                    new ValidationErrorDetail { ErrorMessage = EmailErrorTooLong, Field = "Email" });
            }
            else if(!Regex.Match(model.Email,ValidEmailPattern).Success)
            {
                errorMessages.Add(
                    new ValidationErrorDetail { ErrorMessage = EmailErrorWrongFormat, Field = "Email" });
            }


            return errorMessages;
        }
    }
}
