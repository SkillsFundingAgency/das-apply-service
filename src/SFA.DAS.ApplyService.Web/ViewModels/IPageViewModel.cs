using System;

namespace SFA.DAS.ApplyService.Web.ViewModels
{
    public interface IPageViewModel
    {
        string Title { get; set; }
        Guid ApplicationId { get; }
        int SequenceId { get; set; }
        int SectionId { get; set; }
        string PageId { get; set; }
        string GetHelpQuestion { get; set; }
        bool GetHelpQuerySubmitted { get; set; }
        string GetHelpErrorMessage { get; set; }
        string GetHelpAction { get; set; }
    }
}