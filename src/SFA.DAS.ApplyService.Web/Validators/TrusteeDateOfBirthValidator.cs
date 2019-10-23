using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public class TrusteeDateOfBirthValidator
    {
        public const string MissingDateOfBirthErrorMessage = "Enter a date of birth";
        public const string InvalidIncompleteDateOfBirthErrorMessage = "Enter a date of birth using a month and year";

        public static List<ValidationErrorDetail> ValidateTrusteeDatesOfBirth(TabularData trusteesData, List<Answer> answers)
        {
            var errorMessages = new List<ValidationErrorDetail>();

            foreach (var trustee in trusteesData.DataRows)
            {
                var dobMonthKey = $"{trustee.Id}_Month";
                var dobYearKey = $"{trustee.Id}_Year";
                var dobMonth = answers.FirstOrDefault(x => x.QuestionId == dobMonthKey);
                var dobYear = answers.FirstOrDefault(x => x.QuestionId == dobYearKey);

                if (dobMonth == null && dobYear == null)
                {
                    var errorMessage = new ValidationErrorDetail
                    {
                        Field = trustee.Id + "_Month",
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
                            Field = trustee.Id + "_Month",
                            ErrorMessage = InvalidIncompleteDateOfBirthErrorMessage
                        };
                        errorMessages.Add(errorMessage);
                    }

                    if (dobYear == null)
                    {
                        var errorMessage = new ValidationErrorDetail
                        {
                            Field = trustee.Id + "_Year",
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
                                Field = trustee.Id + "_Month",
                                ErrorMessage = InvalidIncompleteDateOfBirthErrorMessage
                            };
                            errorMessages.Add(errorMessage);
                        }
                    }
                    if (dobYear != null)
                    {
                        int yearValue = 0;
                        int.TryParse(dobYear.Value, out yearValue);
                        if (yearValue == 0 || yearValue > DateTime.Now.Year)
                        {
                            var errorMessage = new ValidationErrorDetail
                            {
                                Field = trustee.Id + "_Year",
                                ErrorMessage = InvalidIncompleteDateOfBirthErrorMessage
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
                                Field = trustee.Id + "_Month",
                                ErrorMessage = InvalidIncompleteDateOfBirthErrorMessage
                            };
                            errorMessages.Add(errorMessage);
                        }
                    }
                }
            }

            return errorMessages;
        }

    }
}
