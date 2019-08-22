using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class DateNotInFutureValidator : Validator
    {
        protected string[] Formats;

        public DateNotInFutureValidator(string[] formats = null)
        {
            Formats = formats ?? new string[] { "d,M,yyyy" };
        }

        public override List<KeyValuePair<string, string>> Validate(string questionId, Answer answer)
        {
            var errorMessages = base.Validate(questionId, answer);

            if (!string.IsNullOrWhiteSpace(GetValue(answer)))
            {
                if (DateTime.TryParseExact(GetValue(answer), Formats, null, DateTimeStyles.None, out DateTime dateEntered))
                {
                    if (dateEntered > DateTime.Today)
                    {
                        errorMessages.Add(new KeyValuePair<string, string>(GetFieldId(questionId), ValidationDefinition.ErrorMessage));
                    }
                }
            }

            return errorMessages;
        }
    }
}
