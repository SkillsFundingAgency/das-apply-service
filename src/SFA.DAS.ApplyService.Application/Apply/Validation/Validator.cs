using System;
using System.Collections.Generic;
using System.Text;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public abstract class Validator : IValidator
    {
        public ValidationDefinition ValidationDefinition { get; set; }
        
        public virtual List<KeyValuePair<string, string>> Validate(Answer answer)
        {
            return new List<KeyValuePair<string, string>>();
        }

        protected string GetValue(Answer answer)
        {
            return string.IsNullOrEmpty(ValidationDefinition.Key)
                ? answer?.Value
                : answer?.JsonValue[ValidationDefinition.Key];
        }

        protected string GetFieldId(Answer answer)
        {
            return string.IsNullOrEmpty(ValidationDefinition.Key)
                ? answer?.QuestionId
                : $"{answer?.QuestionId}_Key_{ValidationDefinition.Key}";
        }
    }
}
