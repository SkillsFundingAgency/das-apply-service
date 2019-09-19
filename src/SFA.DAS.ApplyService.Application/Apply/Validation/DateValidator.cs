using System;
using System.Collections.Generic;
using System.Globalization;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class DateValidator : Validator
    {
        protected string[] Formats;

        public DateValidator(string[] formats = null)
        {
            Formats = formats ?? new string[] { "d,M,yyyy" };
        }

        public override List<KeyValuePair<string, string>> Validate(string questionId, Answer answer)
        {
            var errorMessages = base.Validate(questionId, answer);

            if (!string.IsNullOrWhiteSpace(GetValue(answer)))
            {
                if (!DateTime.TryParseExact(GetValue(answer), Formats, null, DateTimeStyles.None, out _))
                {
                    errorMessages.Add(new KeyValuePair<string, string>(GetFieldId(questionId), ValidationDefinition.ErrorMessage));
                    return errorMessages;
                }
            }
            else
            {
                errorMessages.Add(new KeyValuePair<string, string>(GetFieldId(questionId), ValidationDefinition.ErrorMessage));
                return errorMessages;
            }

            return errorMessages;
        }
    }
}