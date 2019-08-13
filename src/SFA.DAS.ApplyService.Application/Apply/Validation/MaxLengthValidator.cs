using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class MaxLengthValidator : Validator
    {
        public override List<KeyValuePair<string, string>> Validate(string questionId, Answer answer)
        {
            var errorMessages = base.Validate(questionId, answer);

            if (GetValue(answer)?.Length > (long)ValidationDefinition.Value)
            {
                errorMessages.Add(new KeyValuePair<string, string>(GetFieldId(questionId), ValidationDefinition.ErrorMessage));
            }

            return errorMessages;
        }
    }
}