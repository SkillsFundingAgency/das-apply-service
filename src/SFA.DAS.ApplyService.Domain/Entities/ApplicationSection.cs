using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class QnAData
    {
        public List<Page> Pages { get; set; }
        public FinancialApplicationGrade FinancialApplicationGrade { get; set; }

        public List<Feedback> Feedback { get; set; } // Section level feedback

        [JsonIgnore]
        public bool HasFeedback => Feedback?.Any() ?? false;

        [JsonIgnore]
        public bool HasNewFeedback => HasFeedback && Feedback.Any(f => f.IsNew);

        [JsonIgnore]
        public bool AllFeedbackIsCompleted => HasFeedback ? Feedback.All(f => f.IsCompleted) : true;
    }

    public class ApplicationSection : EntityBase
    {
        public Guid ApplicationId { get; set; }
        public int SectionId { get; set; }
        public int SequenceId { get; set; }

        public QnAData QnAData { get; set; }

        public int PagesComplete
        {
            get { return QnAData.Pages.Count(p => p.Active && p.Complete); }
        }
        
        public int PagesActive
        {
            get { return QnAData.Pages.Count(p => p.Active); }
        }

        public string Title { get; set; }
        public string LinkTitle { get; set; }
        public string DisplayType { get; set; }

        public Page GetPage(string pageId)
        {
            return QnAData.Pages.SingleOrDefault(p => p.PageId == pageId);
        }

        public void UpdatePage(Page page)
        {
            var currentPages = QnAData.Pages;

            var currentPageIndex = currentPages.IndexOf(currentPages.Single(p => p.PageId == page.PageId));
            
            currentPages.RemoveAt(currentPageIndex);
            currentPages.Insert(currentPageIndex, page);

            QnAData.Pages = currentPages;
        }
        
        [JsonIgnore]
        public bool HasNewPageFeedback => QnAData.Pages.Any(p => p.HasNewFeedback);

        [JsonIgnore]
        public bool HasNewSectionFeedback => QnAData.HasNewFeedback;
    }

    public class ApplicationSectionStatus
    {
        public const string Submitted = "Submitted";
        public const string InProgress = "In Progress";
        public const string Graded = "Graded";
        public const string Evaluated = "Evaluated";
    }
}