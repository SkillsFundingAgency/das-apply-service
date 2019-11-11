using System;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class SearchByUkprnViewModel : IPageViewModel
    {
        public string UKPRN { get; set; }
        public string Title { get { return "What is your organisation's UK provider reference number (UKPRN)?"; } set { } }

        public Guid ApplicationId { get; set; }

        public string SequenceId { get; set; }
        public int SectionId { get; set; }
        public string PageId { get; set; }
    }
}