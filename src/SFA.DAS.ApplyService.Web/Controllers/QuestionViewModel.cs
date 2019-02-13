using System;
using SFA.DAS.ApplyService.Application.Apply.Validation;
using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class QuestionViewModel
    {
        public string QuestionId { get; set; }
        public string Type { get; set; }
        public string Label { get; set; }
        public string ShortLabel { get; set; }
        public string QuestionBodyText { get; set; }
        public string Hint { get; set; }
        public string InputClasses { get; set; }
        public string Value { get; set; }
        public List<Option> Options { get; set; }
        public List<ValidationErrorDetail> ErrorMessages { get; set; }

        public Guid ApplicationId { get; set; }
        public int SequenceId { get; set; }
        public int SectionId { get; set; }
        public string PageId { get; set; }
        public string RedirectAction { get; set; }

        public string DisplayAnswerValue(Answer answer)
        {
            if (Type == "Date")
            {
                var dateparts = answer.Value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                var day = dateparts[0];
                var month = dateparts[1];
                var year = dateparts[2];

                return $"{day}/{month}/{year}";
            }
            return answer.Value;
        }
    }
}