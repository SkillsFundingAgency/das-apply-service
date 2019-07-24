using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class MinLengthValidator : Validator
    {
        public override List<KeyValuePair<string, string>> Validate(string questionId, Answer answer)
        {
            var errorMessages = base.Validate(questionId, answer);

            var value = GetValue(answer);
            if (value == null || value?.Length < (long)ValidationDefinition.Value)
            {
                errorMessages.Add(new KeyValuePair<string, string>(GetFieldId(questionId),
                        ValidationDefinition.ErrorMessage));
            }

            return errorMessages;
        }
    }
}