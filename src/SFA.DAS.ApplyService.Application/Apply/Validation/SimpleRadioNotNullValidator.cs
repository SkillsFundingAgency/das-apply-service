using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class SimpleRadioNotNullValidator : Validator
    {
        public override List<KeyValuePair<string, string>> Validate(string questionId, Answer answer)
        {
            var errorMessages = base.Validate(questionId, answer);

            if (answer == null)
            {
                errorMessages.Add(new KeyValuePair<string, string>(GetFieldId(questionId), ValidationDefinition.ErrorMessage));
            }
            
            return errorMessages;
        }
    }
}