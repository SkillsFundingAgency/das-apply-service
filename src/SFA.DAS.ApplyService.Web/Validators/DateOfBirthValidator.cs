using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public static class DateOfBirthAnswerValidator
    {
        public const string MissingDateOfBirthErrorMessage = "Enter a date of birth";
        public const string InvalidIncompleteDateOfBirthErrorMessage = "Enter a date of birth using a month and year";
        public const string DateOfBirthInFutureErrorMessage = "Enter a date of birth using a month and year that's in the past";
        public const string DateOfBirthYearLengthErrorMessage = "Enter a date of birth using a month and year using 4 numbers";

        public static List<ValidationErrorDetail> ValidateDateOfBirth(Answer dobMonth, Answer dobYear, string fieldPrefix)
        {
            var errorMessages = new List<ValidationErrorDetail>();

            if (dobMonth == null && dobYear == null)
            {
                var errorMessage = new ValidationErrorDetail
                {
                    Field = fieldPrefix + "Month",
                    ErrorMessage = MissingDateOfBirthErrorMessage
                };
                errorMessages.Add(errorMessage);
            }
            else
            {
                if (dobMonth == null)
                {
                    var errorMessage = new ValidationErrorDetail
                    {
                        Field = fieldPrefix + "Month",
                        ErrorMessage = InvalidIncompleteDateOfBirthErrorMessage
                    };
                    errorMessages.Add(errorMessage);
                }

                if (dobYear == null)
                {
                    var errorMessage = new ValidationErrorDetail
                    {
                        Field = fieldPrefix + "Year",
                        ErrorMessage = InvalidIncompleteDateOfBirthErrorMessage
                    };
                    errorMessages.Add(errorMessage);
                }

                if (dobMonth != null)
                {
                    int monthValue = 0;
                    int.TryParse(dobMonth.Value, out monthValue);
                    if (monthValue < 1 || monthValue > 12)
                    {
                        var errorMessage = new ValidationErrorDetail
                        {
                            Field = fieldPrefix + "Month",
                            ErrorMessage = InvalidIncompleteDateOfBirthErrorMessage
                        };
                        errorMessages.Add(errorMessage);
                    }
                }
                if (dobYear != null)
                {
                    int yearValue = 0;
                    int.TryParse(dobYear.Value, out yearValue);
                    if (yearValue == 0)
                    {
                        var errorMessage = new ValidationErrorDetail
                        {
                            Field = fieldPrefix + "Year",
                            ErrorMessage = InvalidIncompleteDateOfBirthErrorMessage
                        };
                        errorMessages.Add(errorMessage);
                    }
                    if (yearValue > 0 && yearValue < 1000)
                    {
                        var errorMessage = new ValidationErrorDetail
                        {
                            Field = fieldPrefix + "Year",
                            ErrorMessage = DateOfBirthYearLengthErrorMessage
                        };
                        errorMessages.Add(errorMessage);
                    }
                    if (yearValue > DateTime.Now.Year)
                    {
                        var errorMessage = new ValidationErrorDetail
                        {
                            Field = fieldPrefix + "Year",
                            ErrorMessage = DateOfBirthInFutureErrorMessage
                        };
                        errorMessages.Add(errorMessage);
                    }
                }
                if (dobMonth != null && dobYear != null)
                {
                    int monthValue = 0;
                    int.TryParse(dobMonth.Value, out monthValue);
                    int yearValue = 0;
                    int.TryParse(dobYear.Value, out yearValue);
                    if (monthValue == DateTime.Now.Month && yearValue == DateTime.Now.Year)
                    {
                        var errorMessage = new ValidationErrorDetail
                        {
                            Field = fieldPrefix + "Month",
                            ErrorMessage = DateOfBirthInFutureErrorMessage
                        };
                        errorMessages.Add(errorMessage);
                    }
                    if (monthValue > DateTime.Now.Month && yearValue == DateTime.Now.Year)
                    {
                        var errorMessage = new ValidationErrorDetail
                        {
                            Field = fieldPrefix + "Month",
                            ErrorMessage = DateOfBirthInFutureErrorMessage
                        };
                        errorMessages.Add(errorMessage);
                    }
                }

            }

            return errorMessages;

        }

    }
}
