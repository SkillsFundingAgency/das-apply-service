using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class MaxLengthValidator : IValidator
    {
        public ValidationDefinition ValidationDefinition { get; set; }
        public List<KeyValuePair<string, string>> Validate(Question question, Answer answer)
        {
            var errors = new List<KeyValuePair<string, string>>();
            if (answer.Value.Length > (long)ValidationDefinition.Value)
            {
                errors.Add(new KeyValuePair<string, string>(answer.QuestionId, ValidationDefinition.ErrorMessage));
            }

            return errors;
        }
    }
}