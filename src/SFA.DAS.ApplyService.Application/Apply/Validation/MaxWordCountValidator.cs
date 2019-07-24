using System;
using System.Collections.Generic;
using System.Text;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class MaxWordCountValidator : Validator
    {
        public override List<KeyValuePair<string, string>> Validate(string questionId, Answer answer)
        {
            var errorMessages = base.Validate(questionId, answer);

            var text = GetValue(answer)?.Trim();
            if (string.IsNullOrEmpty(text))
                return errorMessages;

            var wordCount = text.Split(new[]{" "}, StringSplitOptions.RemoveEmptyEntries).Length;

            if (wordCount > (long)ValidationDefinition.Value)
            {
                errorMessages.Add(new KeyValuePair<string, string>(GetFieldId(questionId), ValidationDefinition.ErrorMessage));
            }

            return errorMessages;
        }
    }
}
