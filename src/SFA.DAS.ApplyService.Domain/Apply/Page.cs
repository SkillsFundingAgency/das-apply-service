using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class Page
    {
        public string PageId { get; set; }
        public string SequenceId { get; set; }
        public string SectionId { get; set; }
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
        
        public string BodyText { get; set; }

        public bool IsQuestionAnswered(string questionId)
        {
            var allAnswers = PageOfAnswers.SelectMany(poa => poa.Answers).ToList();
            return allAnswers.Any(a => a.QuestionId == questionId);
        }

        public List<Feedback> Feedback { get; set; }

        [JsonIgnore]
        public bool HasFeedback => Feedback?.Any() ?? false;

        [JsonIgnore]
        public bool HasNewFeedback => HasFeedback && Feedback.Any(f => f.IsNew || !f.IsCompleted);

        [JsonIgnore]
        public bool HasCompletedFeedback => HasFeedback && Feedback.Any(f => f.IsCompleted);
    }

}