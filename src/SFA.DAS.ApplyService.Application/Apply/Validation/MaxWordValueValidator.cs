using System;
using System.Collections.Generic;
using System.Text;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class MaxWordValueValidator: IValidator
    {
        public ValidationDefinition ValidationDefinition { get; set; }
        public List<KeyValuePair<string, string>> Validate(Question question, Answer answer)
        {
            var errors = new List<KeyValuePair<string, string>>();
            var text = answer?.Value?.Trim();
            if (string.IsNullOrEmpty(text))
                return errors;

            int wordCount = 0, index = 0;

            while (index < text.Length)
            {
                while (index < text.Length && !char.IsWhiteSpace(text[index]))
                    index++;
                    wordCount++;
                while (index < text.Length && char.IsWhiteSpace(text[index]))
                    index++;
            }

            if (wordCount > (long)ValidationDefinition.Value)
            {
                errors.Add(new KeyValuePair<string, string>(answer.QuestionId, ValidationDefinition.ErrorMessage));
            }

            return errors;
        }
    }
}
