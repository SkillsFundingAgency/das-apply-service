using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class Question
    {
        public string QuestionId { get; set; }
        public string AnswerTag { get; set; }
        public string Label { get; set; }
        public string ShortLabel { get; set; }
        public string QuestionBodyText { get; set; }
        public string Hint { get; set; }
        public Input Input { get; set; }
        public int? Order { get; set; }
        public string Value { get; set; }
        public IEnumerable<dynamic> ErrorMessages { get; set; }
    }
}