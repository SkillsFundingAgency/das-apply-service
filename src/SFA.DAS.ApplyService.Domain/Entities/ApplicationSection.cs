using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class ApplicationSection : EntityBase
    {
        public Guid ApplicationId { get; set; }
        public int SectionId { get; set; }
        public int SequenceId { get; set; }
        public string QnAData { get; set; }
        
        public List<Page> Pages
        {
            get => JsonConvert.DeserializeObject<List<Page>>(QnAData);
            set => QnAData = JsonConvert.SerializeObject(value);
        }

        public int PagesComplete
        {
            get { return Pages.Count(p => p.Active && p.Complete); }
        }
        
        public int PagesActive
        {
            get { return Pages.Count(p => p.Active); }
        }

        public string Title { get; set; }
        public string LinkTitle { get; set; }
        public string DisplayType { get; set; }

        public Page GetPage(string pageId)
        {
            return Pages.SingleOrDefault(p => p.PageId == pageId);
        }

        public void UpdatePage(Page page)
        {
            var currentPages = JsonConvert.DeserializeObject<List<Page>>(QnAData);

            var currentPageIndex = currentPages.IndexOf(currentPages.Single(p => p.PageId == page.PageId));
            
            currentPages.RemoveAt(currentPageIndex);
            currentPages.Insert(currentPageIndex, page);

            Pages = currentPages;
        }
    }
}