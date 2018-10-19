using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class Page
    {
        public string PageId { get; set; }
        public string SequenceId { get; set; }
        public string Title { get; set; }
        public List<Question> Questions { get; set; }
        public List<Answer> Answers { get; set; }
        public List<Next> Next { get; set; }
        public bool Complete { get; set; }
        public bool AllowMultipleAnswers { get; set; }
        public int? Order { get; set; }
        public bool Active { get; set; }
    }
}