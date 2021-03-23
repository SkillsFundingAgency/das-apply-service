using System;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp.ManagementHierarchy
{
    public class ConfirmManagementHierarchyViewModel: ManagementHierarchyViewModel, IPageViewModel
    {
        public Guid ApplicationId { get; set; }
        public TabularData ManagementHierarchyData { get; set; }

        public string Title { get; set; }
        public int SequenceId { get; set; }
        public int SectionId { get; set; }
        public string PageId { get; set; }
        public string GetHelpQuestion { get; set; }
        public bool GetHelpQuerySubmitted { get; set; }
        public string GetHelpErrorMessage { get; set; }
        public string GetHelpAction { get; set; }
    }
}
