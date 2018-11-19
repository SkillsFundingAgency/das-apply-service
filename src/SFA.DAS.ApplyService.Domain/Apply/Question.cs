using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class Question
    {
        public string QuestionId { get; set; }
        public string Label { get; set; }
        public string Hint { get; set; }
        public Input Input { get; set; }
        public int? Order { get; set; }
        public List<Feedback> Feedback { get; set; }
        public bool HasFeedback => Feedback?.Any() ?? false;
    }
    
    public class Feedback
    {
        public string From { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}