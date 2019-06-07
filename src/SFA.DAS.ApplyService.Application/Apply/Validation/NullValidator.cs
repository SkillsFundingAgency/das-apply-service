using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class NullValidator : Validator
    {
        public override List<KeyValuePair<string, string>> Validate(Answer answer)
        {
            return base.Validate(answer);
        }
    }
}