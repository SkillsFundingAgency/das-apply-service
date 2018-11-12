using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class Page
    {
        public string PageId { get; set; }
        public string SequenceId { get; set; }
        public string Title { get; set; }
        public string LinkTitle { get; set; }
        public string InfoText { get; set; }
        public List<Question> Questions { get; set; }
        public List<PageOfAnswers> PageOfAnswers { get; set; }
        public List<Next> Next { get; set; }
        public bool Complete { get; set; }
        public bool AllowMultipleAnswers { get; set; }
        public int? Order { get; set; }
        public bool Active { get; set; }        
        public bool Visible { get; set; }
    }

}