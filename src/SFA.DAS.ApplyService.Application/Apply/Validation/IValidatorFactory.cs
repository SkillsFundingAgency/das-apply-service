using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public interface IValidatorFactory
    {
        List<IValidator> Build(Question question);
    }

    public interface IValidator
    {
        ValidationDefinition ValidationDefinition { get; set; }
        List<KeyValuePair<string, string>> Validate(Question question, Answer answer);
    }
}