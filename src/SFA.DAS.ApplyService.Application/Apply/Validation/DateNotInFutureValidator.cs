using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class DateNotInFutureValidator : Validator
    {
        public override List<KeyValuePair<string, string>> Validate(Answer answer)
        {
            var errorMessages = base.Validate(answer);

            var dateParts = GetValue(answer).Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (dateParts.Length != 3)
            {
                return errorMessages;
            }

            var day = dateParts[0];
            var month = dateParts[1];
            var year = dateParts[2];

            if (string.IsNullOrWhiteSpace(day) || string.IsNullOrWhiteSpace(month) || string.IsNullOrWhiteSpace(year))
            {
                return errorMessages;
            }

            var formatStrings = new string[] { "d/M/yyyy" };
            if (DateTime.TryParseExact($"{day}/{month}/{year}", formatStrings, null, DateTimeStyles.None, out DateTime dateEntered))
            {
                if (dateEntered > DateTime.Today)
                {
                    errorMessages.Add(new KeyValuePair<string, string>(GetFieldId(answer),
                        ValidationDefinition.ErrorMessage));
                }
            }

            return errorMessages;
        }
    }
}
