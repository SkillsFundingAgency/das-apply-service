using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class RequiredValidator : Validator
    {
        public override List<KeyValuePair<string, string>> Validate(Answer answer)
        {
            var errorMessages = base.Validate(answer);

            if (string.IsNullOrWhiteSpace(GetValue(answer)))
            {
                errorMessages.Add(new KeyValuePair<string, string>(GetFieldId(answer), ValidationDefinition.ErrorMessage));
            }

            return errorMessages;
        }
    }
}