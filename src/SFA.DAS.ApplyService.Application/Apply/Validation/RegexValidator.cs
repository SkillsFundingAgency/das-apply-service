using System.Collections.Generic;
using System.Text.RegularExpressions;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class RegexValidator : Validator
    {
        public override List<KeyValuePair<string, string>> Validate(Answer answer)
        {
            var errorMessages = base.Validate(answer);

            var regex = new Regex(ValidationDefinition.Value.ToString());
            var value = GetValue(answer);

            if (!string.IsNullOrEmpty(value) && !regex.IsMatch(value))
            {
                errorMessages.Add(new KeyValuePair<string, string>(GetFieldId(answer), ValidationDefinition.ErrorMessage));
            }

            return errorMessages;
        }
    }
}