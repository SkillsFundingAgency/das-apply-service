using System;
using System.Collections.Generic;
using Remotion.Linq;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Web.ViewModels
{
    public class QuestionViewModel
    {
        public Guid Id => ApplicationId;
        public int SequenceNo => SequenceId;


        public string QuestionId { get; set; }
        public string Type { get; set; }
        public string Label { get; set; }
        public string ShortLabel { get; set; }
        public string QuestionBodyText { get; set; }
        public string Hint { get; set; }
        public string InputClasses { get; set; }
        public string InputPrefix { get; set; }
        public string InputSuffix { get; set; }
        public string Value { get; set; }
        public dynamic JsonValue { get; set; }
        public List<Option> Options { get; set; }
        public List<ValidationDefinition> Validations { get; set; }
        public List<ValidationErrorDetail> ErrorMessages { get; set; }

        public Guid ApplicationId { get; set; }
        public int SequenceId { get; set; }
        public int SectionId { get; set; }
        public string PageId { get; set; }
        public string RedirectAction { get; set; }

        public string DisplayAnswerValue(Answer answer)
        {
            var answerValue = answer?.Value;

            if (!string.IsNullOrWhiteSpace(answer?.Value))
            {
                var dateparts = answer.Value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                if ("Date".Equals(Type, StringComparison.InvariantCultureIgnoreCase) && dateparts.Length == 3)
                {
                    if (DateTime.TryParse($"{dateparts[0]}/{dateparts[1]}/{dateparts[2]}", out var datetime))
                    {
                        answerValue = datetime.ToString("dd/MM/yyyy");
                    }
                }
                else if ("MonthAndYear".Equals(Type, StringComparison.InvariantCultureIgnoreCase) && dateparts.Length == 2)
                {
                    if (DateTime.TryParse($"{dateparts[0]}/{dateparts[1]}", out var datetime))
                    {
                        answerValue = datetime.ToString("MM/yyyy");
                    }
                }
            }

            return answerValue ?? string.Empty;
        }
    }
}