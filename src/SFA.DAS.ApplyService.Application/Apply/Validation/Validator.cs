using System;
using System.Collections.Generic;
using System.Text;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public abstract class Validator : IValidator
    {
        public ValidationDefinition ValidationDefinition { get; set; }
        
        public virtual List<KeyValuePair<string, string>> Validate(string questionId, Answer answer)
        {
            return new List<KeyValuePair<string, string>>();
        }

        protected string GetValue(Answer answer)
        {
            return answer != null 
                ? string.IsNullOrEmpty(ValidationDefinition.Key)
                    ? answer.Value
                    : answer.JsonValue[ValidationDefinition.Key]
                : null;
        }

        protected string GetFieldId(string questionId)
        {
            return string.IsNullOrEmpty(ValidationDefinition.Key)
                    ? questionId
                    : $"{questionId}_Key_{ValidationDefinition.Key}";
        }
    }
}
