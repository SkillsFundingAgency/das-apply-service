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
        public int SectionId { get; set; }
        [JsonProperty("SequenceNo")]
        public int SequenceId { get; set; }

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
        // convert to GETTER that is : return section.PagesComplete == section.PagesActive && section.PagesActive > 0
        // rather than a field updated and returned from database table
        // then refactor RoatpTaskListWorkflowService to use this
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