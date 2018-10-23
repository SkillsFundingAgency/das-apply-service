using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class RequiredValidator : IValidator
    {
        public ValidationDefinition ValidationDefinition { get; set; }
        public List<KeyValuePair<string, string>> Validate(Question question, Answer answer)
        {
            if (string.IsNullOrWhiteSpace(answer.Value))
            {
                return new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(answer.QuestionId,
                        ValidationDefinition.ErrorMessage)
                };
            }
            return new List<KeyValuePair<string, string>>();
        }
    }
}