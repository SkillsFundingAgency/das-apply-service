using System;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class EnterNewUkprnViewModel : IPageViewModel
    {
        public string Title { get; set; }
        public Guid ApplicationId { get; set; }
        public string SequenceId { get; set; }
        public int SectionId { get; set; }
        public string PageId { get; set; }
        public string GetHelpQuestion { get; set; }
        public bool GetHelpQuerySubmitted { get; set; }
        public string GetHelpErrorMessage { get; set; }
        public string GetHelpAction { get; set; }
        public string Ukprn { get; set; }
        public long CurrentUkprn { get; set; }
    }
}
