using System.Collections.Generic;
using System.Text.RegularExpressions;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class RegexValidator : IValidator
    {
        public ValidationDefinition ValidationDefinition { get; set; }
        public List<KeyValuePair<string, string>> Validate(Question question, Answer answer)
        {
            var regex = new Regex(ValidationDefinition.Value.ToString());
            return !regex.IsMatch(answer.Value)
                ? new List<KeyValuePair<string, string>>
                    {new KeyValuePair<string, string>(answer.QuestionId, ValidationDefinition.ErrorMessage)}
                : new List<KeyValuePair<string, string>>();
        }
    }
}