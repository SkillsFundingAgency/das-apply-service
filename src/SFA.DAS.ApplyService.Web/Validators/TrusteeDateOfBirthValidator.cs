using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public class TrusteeDateOfBirthValidator
    {
        public static List<ValidationErrorDetail> ValidateTrusteeDatesOfBirth(TabularData trusteesData, List<Answer> answers)
        {
            var errorMessages = new List<ValidationErrorDetail>();

            foreach (var trustee in trusteesData.DataRows)
            {
                var dobMonthKey = $"{trustee.Id}_Month";
                var dobYearKey = $"{trustee.Id}_Year";
                var dobMonth = answers.FirstOrDefault(x => x.QuestionId == dobMonthKey);
                var dobYear = answers.FirstOrDefault(x => x.QuestionId == dobYearKey);

                var prefix = trustee.Id + "_";
                var validatorMessages = DateOfBirthAnswerValidator.ValidateDateOfBirth(dobMonth, dobYear, prefix);
                if (validatorMessages.Any())
                {
                    errorMessages.AddRange(validatorMessages);
                }
            }

            return errorMessages;
        }

    }
}
