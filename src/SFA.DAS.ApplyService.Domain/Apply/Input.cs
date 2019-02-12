using System.Collections;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Domain.Apply
{
  public class Input
  {
    public string Type { get; set; }
    public string InputClasses { get; set; }
    public List<Option> Options { get; set; }
    public List<ValidationDefinition> Validations { get; set; }
    public string DataEndpoint { get; set; }
  }

  public class Option
  {
    public List<Question> FurtherQuestions { get; set; }
    public string Value { get; set; }
    public string Label { get; set; }
  }
}