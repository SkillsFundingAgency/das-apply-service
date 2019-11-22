using System;

namespace SFA.DAS.ApplyService.Web.ViewModels
{
    public interface IPageViewModel
    {
        string Title { get; set; }
        Guid ApplicationId { get; }
        string SequenceId { get; set; }
        int SectionId { get; set; }
        string PageId { get; set; }
        bool GetHelpQuerySubmitted { get; set; }
    }
}