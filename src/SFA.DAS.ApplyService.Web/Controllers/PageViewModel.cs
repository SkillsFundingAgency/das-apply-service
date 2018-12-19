using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using SFA.DAS.ApplyService.Application.Apply.Validation;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class PageViewModel
    {
        public Guid ApplicationId { get; }

        public PageViewModel(Page page, Guid applicationId)
        {
            ApplicationId = applicationId;
            SetupPage(page, null);
        }

        public PageViewModel(Page page, Guid applicationId, List< ValidationErrorDetail> errorMessages)
        {
            ApplicationId = applicationId;
            SetupPage(page, errorMessages);
            ErrorMessages = errorMessages;
        }

        public bool HasFeedback { get; set; }

        public List<Feedback> Feedback { get; set; }      
        
        public string LinkTitle { get; set; }

        public string PageId { get; set; }
        public string Title { get; set; }
        
        public List<QuestionViewModel> Questions { get; set; }
        public string SequenceId { get; set; }
        public int SectionId { get; set; }

        public List<PageOfAnswers> PageOfAnswers { get; set; }
        public string BodyText { get; set; }
        public string RedirectAction { get; set; }

        public List<ValidationErrorDetail> ErrorMessages { get; set; }

        private void SetupPage(Page page, List<ValidationErrorDetail> errorMessages)
        {
            Title = page.Title;
            LinkTitle = page.LinkTitle;
            PageId = page.PageId;
            SequenceId = page.SequenceId;
            PageOfAnswers = page.PageOfAnswers;
            SectionId = int.Parse((string) page.SectionId);

            var questions = page.Questions;
            var answers = page.PageOfAnswers.FirstOrDefault()?.Answers;

            Questions = new List<QuestionViewModel>();

            Questions.AddRange(questions.Select(q => new QuestionViewModel()
            {
                Label = q.Label,
                ShortLabel = q.ShortLabel,
                QuestionBodyText = q.QuestionBodyText,
                QuestionId = q.QuestionId,
                Type = q.Input.Type,
                Hint = q.Hint,
                Options = q.Input.Options,
                Value = page.AllowMultipleAnswers ? null : answers?.SingleOrDefault(a => a?.QuestionId == q.QuestionId)?.Value,
                ErrorMessages = errorMessages?.Where(f=>f.Field == q.QuestionId).ToList()
            }));

            Feedback = page.Feedback;
            HasFeedback = page.HasFeedback;
            BodyText = page.BodyText;

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
    }
}