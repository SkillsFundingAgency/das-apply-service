using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class PageViewModel
    {
        public PageViewModel(Page page)
        {
            Title = page.Title;
            PageId = page.PageId;
            SequenceId = page.SequenceId;

            var questions = page.Questions;
            var answers = page.Answers;

            Questions = new List<QuestionViewModel>();

            Questions.AddRange(questions.Select(q => new QuestionViewModel()
            {
                Label = q.Label,
                QuestionId = q.QuestionId,
                Type = q.Input.Type,
                Hint = q.Hint,
                Options = q.Input.Options,
                Value = answers?.SingleOrDefault(a => a?.QuestionId == q.QuestionId)?.Value
            }));

            foreach (var question in Questions)
            {
                if (question.Options == null) continue;
                foreach (var option in question.Options)
                {
                    if (option.FurtherQuestions == null) continue;
                    foreach (var furtherQuestion in option.FurtherQuestions)
                    {
                        furtherQuestion.Value = answers
                            ?.SingleOrDefault(a => a?.QuestionId == furtherQuestion.QuestionId.ToString())?.Value;
                    }
                }
            }
        } 
        public string PageId { get; set; }
        public string Title { get; set; }
        
        public List<QuestionViewModel> Questions { get; set; }
        public string SequenceId { get; set; }
    }
}