using System;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public static class ManagementHierarchyValidator
    {
        public const string FirstNameMinLengthError = "Enter first name";
        public const string FirstNameMaxLengthError = "Enter a first name using 255 characters or less";
        public const string LastNameMinLengthError = "Enter last name";
        public const string LastNameMaxLengthError = "Enter a last name using 255 characters or less";
        public const string JobRoleMinLengthError = "Enter a job role";
        public const string JobRoleMaxLengthError = "Enter a job role using 255 characters or less";
        public const string TimeInRoleError = "Enter a year and month";
        public const string TimeInRoleMonthsTooBigError = "Enter 11 or less for month";
        public const string TimeInRoleYearsTooBigError = "Enter 99 or less for year";
        public const string IsPartOfOtherOrgThatGetsFundingError = "Tell us if this person is part of any other organisations";
        public const string OtherOrgNameError = "Enter the names of all these organisations";
        public const string OtherOrgNameLengthError = "Enter the names of all these organisations using 750 characters or less";

        public const string DobFieldPrefix = "Dob";
        public const string EmailError = "Enter your email";
        public const string EmailErrorTooLong = "Enter your email using 320 characters or less";
        public const string EmailErrorWrongFormat = "Enter your email address using an acceptable format (eg name@location.com)";
        public const string ContactNumberError = "Enter your contact number";
        public const string ContactNumberTooLongError = "Enter your contact number using 50 characters or less";


        public static List<ValidationErrorDetail> Validate(AddEditManagementHierarchyViewModel model)
        {
            var errorMessages = new List<ValidationErrorDetail>();

            EvaluateFirstName(model, errorMessages);
            EvaluateLastName(model, errorMessages);
            EvaluateJobRole(model, errorMessages);
            EvaluateTimeInRole(model, errorMessages);
            EvaluateOrganisationDetails(model, errorMessages);
            EvaluationDateOfBirth(model, errorMessages);
            EvaluationEmail(model, errorMessages);
            EvaluationContactNumber(model, errorMessages);
            return errorMessages;
        }

        private static void EvaluationContactNumber(AddEditManagementHierarchyViewModel model, List<ValidationErrorDetail> errorMessages)
        {
            if (string.IsNullOrEmpty(model.ContactNumber))
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = ContactNumberError,
                        Field = "ContactNumber"
                    });
            }
            else if (model.ContactNumber.Length >50)
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = ContactNumberTooLongError,
                        Field = "ContactNumber"
                    });
            }
        }

        private static void EvaluationEmail(AddEditManagementHierarchyViewModel model, List<ValidationErrorDetail> errorMessages)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = EmailError,
                        Field = "Email"
                    });
            }
            else if (model.Email.Length > 320)
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = EmailErrorTooLong,
                        Field = "Email"
                    });
            }
            else
            {
                string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                           + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                           + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

                var emailRegex = new Regex(validEmailPattern, RegexOptions.IgnoreCase);
                if (!emailRegex.IsMatch(model.Email))
                {
                    errorMessages.Add(
                        new ValidationErrorDetail
                        {
                            ErrorMessage = EmailErrorWrongFormat,
                            Field = "Email"
                        });
                }
            }
        }

        private static void EvaluationDateOfBirth(AddEditManagementHierarchyViewModel model, List<ValidationErrorDetail> errorMessages)
        {
            var dobErrorMessages = DateOfBirthAnswerValidator.ValidateDateOfBirth(model.DobMonth, model.DobYear, DobFieldPrefix);
            if (dobErrorMessages.Any())
            {
                errorMessages.AddRange(dobErrorMessages);
            }
        }

        private static void EvaluateOrganisationDetails(AddEditManagementHierarchyViewModel model, List<ValidationErrorDetail> errorMessages)
        {
            if (string.IsNullOrEmpty(model.IsPartOfOtherOrgThatGetsFunding))
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = IsPartOfOtherOrgThatGetsFundingError,
                        Field = "IsPartOfOtherOrgThatGetsFunding"
                    });
            }
            else if (model.IsPartOfOtherOrgThatGetsFunding == "Yes")
            {
                if (string.IsNullOrEmpty(model.OtherOrgName))
                {
                    errorMessages.Add(
                        new ValidationErrorDetail
                        {
                            ErrorMessage = OtherOrgNameError,
                            Field = "IsPartOfOtherOrgThatGetsFunding"
                        });
                }
                else if (model.OtherOrgName.Length > 750)
                {
                    errorMessages.Add(
                        new ValidationErrorDetail
                        {
                            ErrorMessage = OtherOrgNameLengthError,
                            Field = "IsPartOfOtherOrgThatGetsFunding"
                        });
                }
            }
        }


        private static void EvaluateFirstName(AddEditManagementHierarchyViewModel model, ICollection<ValidationErrorDetail> errorMessages)
        {
            if (string.IsNullOrEmpty(model.FirstName))
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = FirstNameMinLengthError,
                        Field = "FirstName"
                    });
            }
            else if (model.FirstName.Length > 255)
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = FirstNameMaxLengthError,
                        Field = "FirstName"
                    });
            }
        }
        private static void EvaluateLastName(AddEditManagementHierarchyViewModel model, ICollection<ValidationErrorDetail> errorMessages)
        {
            if (string.IsNullOrEmpty(model.LastName))
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = LastNameMinLengthError,
                        Field = "LastName"
                    });
            }
            else if (model.LastName.Length > 255)
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage =LastNameMaxLengthError,
                        Field = "LastName"
                    });
            }
        }

        private static void EvaluateJobRole(AddEditManagementHierarchyViewModel model, List<ValidationErrorDetail> errorMessages)
        {
            if (string.IsNullOrEmpty(model.JobRole))
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = JobRoleMinLengthError,
                        Field = "JobRole"
                    });
            }
            else if (model.JobRole.Length > 255)
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = JobRoleMaxLengthError,
                        Field = "JobRole"
                    });
            }
        }

        private static void EvaluateTimeInRole(AddEditManagementHierarchyViewModel model, List<ValidationErrorDetail> errorMessages)
        {
            if (string.IsNullOrEmpty(model.TimeInRoleMonths) || string.IsNullOrEmpty(model.TimeInRoleYears))
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = TimeInRoleError,
                        Field = "TimeInRole"
                    });
            }
            else
            {
                var monthsOk = int.TryParse(model.TimeInRoleMonths, out var months);
                var yearsOk = int.TryParse(model.TimeInRoleYears, out var years);

                if (!monthsOk || !yearsOk || months + years <= 0)
                {
                    errorMessages.Add(
                        new ValidationErrorDetail
                        {
                            ErrorMessage = TimeInRoleError,
                            Field = "TimeInRole"
                        });
                }
                else
                {
                    if (months > 11)
                    {
                        errorMessages.Add(
                            new ValidationErrorDetail
                            {
                                ErrorMessage = TimeInRoleMonthsTooBigError,
                                Field = "TimeInRole"
                            });
                    }

                    if (years > 99)
                    {
                        errorMessages.Add(
                            new ValidationErrorDetail
                            {
                                ErrorMessage = TimeInRoleYearsTooBigError,
                                Field = "TimeInRole"
                            });
                    }
                }
            }
        }
    }
}
