using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class QnAData
    {
        public bool? RequestedFeedbackAnswered { get; set; }
        public List<Page> Pages { get; set; }
        public FinancialApplicationGrade FinancialApplicationGrade { get; set; }
    }

    public class ApplicationSection : EntityBase
    {
        public Guid ApplicationId { get; set; }
        [JsonProperty("SectionNo")]
        public int SectionNo { get; set; }
        [JsonProperty("SequenceNo")]
        public int SequenceNo { get; set; }

        public QnAData QnAData { get; set; }

        public int PagesComplete
        {
            get { return QnAData.Pages.Count(p => !p.NotRequired && p.Active && p.Complete); }
        }

        public int PagesActive
        {
            get { return QnAData.Pages.Count(p => !p.NotRequired && p.Active); }
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

        public bool NotRequired { get; set; }

        [JsonIgnore]
        public bool SectionCompleted { get; set; }
    }

    public class ApplicationSectionStatus
    {
        public const string Draft = "Draft";
        public const string Submitted = "Submitted";
        public const string InProgress = "In Progress";
        public const string Graded = "Graded";
        public const string Evaluated = "Evaluated";
    }
}