using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class NullValidator : Validator
    {
        public override List<KeyValuePair<string, string>> Validate(string questionId, Answer answer)
        {
            return base.Validate(questionId, answer);
        }
    }
}