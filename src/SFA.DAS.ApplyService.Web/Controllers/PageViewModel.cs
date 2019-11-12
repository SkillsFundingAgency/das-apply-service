using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class PageViewModel : IPageViewModel
    {
        public Guid ApplicationId { get; set; }

        public const string DefaultCTAButtonText = "Save and continue";

        private List<QnaPageOverrideConfiguration> _pageOverrideConfiguration;
        public List<QnaLinksConfiguration> LinksConfiguration;

        public PageViewModel() { }

        public PageViewModel(Guid applicationId, int sequenceId, int sectionId, string pageId, Page page, string pageContext, string redirectAction, string returnUrl, List<ValidationErrorDetail> errorMessages, List<QnaPageOverrideConfiguration> pageOverrideConfiguration, List<QnaLinksConfiguration> linksConfiguration)
        {
            ApplicationId = applicationId;
            SequenceId = sequenceId.ToString();
            SectionId = sectionId;
            PageId = pageId;
            PageContext = pageContext;
            RedirectAction = redirectAction;
            ReturnUrl = returnUrl;
            ErrorMessages = errorMessages;
            _pageOverrideConfiguration = pageOverrideConfiguration;
            LinksConfiguration = linksConfiguration.Where(x=>x.PageId == pageId).ToList();
            if (page != null)
            {
                SetupPage(page, errorMessages);
            }
        }

        public bool HasFeedback { get; set; }
        public List<Feedback> Feedback { get; set; }

        public string LinkTitle { get; set; }

        public string PageId { get; set; }
        public string PageContext { get; set; }
        public string Title { get; set; }

        public string DisplayType { get; set; }

        public List<QuestionViewModel> Questions { get; set; }
        public string SequenceId { get; set; }
        public int SectionId { get; set; }

        public bool AllowMultipleAnswers { get; set; }
        public List<PageOfAnswers> PageOfAnswers { get; set; }
        public string BodyText { get; set; }

        public PageDetails Details { get; set; }

        public string RedirectAction { get; set; }
        public string ReturnUrl { get; set; }

        public List<ValidationErrorDetail> ErrorMessages { get; set; }

        public string CTAButtonText { get; set; }
        public bool HideCTA { get; set; }

        public bool GetHelpQuerySubmitted { get; set; }

        private void SetupPage(Page page, List<ValidationErrorDetail> errorMessages)
        {
            Title = page.Title;
            LinkTitle = page.LinkTitle;
            DisplayType = page.DisplayType;
            PageId = page.PageId;
            AllowMultipleAnswers = page.AllowMultipleAnswers;
            if (errorMessages != null && errorMessages.Any())
            {
                PageOfAnswers = page.PageOfAnswers.Take(page.PageOfAnswers.Count - 1).ToList();
            }
            else
            {
                PageOfAnswers = page.PageOfAnswers;
            }


            var questionsWithRetainedAnswers = page.Questions;
            var answers = page.PageOfAnswers.FirstOrDefault()?.Answers;

            Questions = new List<QuestionViewModel>();

            Questions.AddRange(questionsWithRetainedAnswers.Select(q => new QuestionViewModel()
            {
                Label = q.Label,
                ShortLabel = q.ShortLabel,
                QuestionBodyText = q.QuestionBodyText,
                QuestionId = q.QuestionId,
                Type = q.Input.Type,
                InputClasses = q.Input.InputClasses,
                Hint = q.Hint,
                Options = q.Input.Options,
                Validations = q.Input.Validations,
                Value = page.AllowMultipleAnswers ? GetMultipleValue(page.PageOfAnswers.LastOrDefault()?.Answers, q, errorMessages) : answers?.SingleOrDefault(a => a?.QuestionId == q.QuestionId)?.Value,
                JsonValue = page.AllowMultipleAnswers ? GetMultipleJsonValue(page.PageOfAnswers.LastOrDefault()?.Answers, q, errorMessages) : answers?.SingleOrDefault(a => a?.QuestionId == q.QuestionId)?.JsonValue,
                ErrorMessages = errorMessages?.Where(f => f.Field.Split("_Key_")[0] == q.QuestionId).ToList(),
                SequenceId = int.Parse(SequenceId),
                SectionId = SectionId,
                ApplicationId = ApplicationId,
                PageId = PageId,
                RedirectAction = RedirectAction
            }));

            foreach (var question in questionsWithRetainedAnswers.Where(x=>x.Value!=null))
            {
                foreach (var q in Questions.Where(q => question.QuestionId == q.QuestionId))
                {
                    q.Value = question.Value;
                }
            }

            Feedback = page.Feedback;
            HasFeedback = page.HasFeedback;
            BodyText = page.BodyText;

            Details = page.Details;

            SetupCallToActionButton();

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

        private void SetupCallToActionButton()
        {
            string ctaButtonText = DefaultCTAButtonText;

            var pageOverride = _pageOverrideConfiguration.FirstOrDefault(p => p.PageId == PageId);

            if (pageOverride != null)
            {
                if (!String.IsNullOrWhiteSpace(pageOverride.CTAButtonText))
                {
                    ctaButtonText = pageOverride.CTAButtonText;
                }
                if (pageOverride.HideCTA)
                {
                    HideCTA = true;
                }
            }

            CTAButtonText = ctaButtonText;
        }

        private string GetMultipleValue(List<Answer> answers, Question question, List<ValidationErrorDetail> errorMessages)
        {
            if (errorMessages != null && errorMessages.Any())
            {
                return answers?.LastOrDefault(a => a?.QuestionId == question.QuestionId)?.Value;
            }

            return null;
        }

        private dynamic GetMultipleJsonValue(List<Answer> answers, Question question, List<ValidationErrorDetail> errorMessages)
        {
            if (errorMessages != null && errorMessages.Any())
            {
                return answers?.LastOrDefault(a => a?.QuestionId == question.QuestionId)?.JsonValue;
            }

            return null;
        }
    }
}