using SFA.DAS.ApplyService.Application.Apply.Roatp;
using System;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp.ManagementHierarchy;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ConfirmRemoveManagementHierarchyViewModel : ManagementHierarchyViewModel, IPageViewModel
    {
        public Guid ApplicationId { get; set; }
        public string Name { get; set; }
        public string Confirmation { get; set; }
        public int Index { get; set; }
        public string ActionName { get; set; }
        public string BackAction { get; set; }

        public string Title { get { return "Are you sure you want to remove this person from management hierarchy?"; } set { } }
        public int SequenceId { get { return RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining; } set { } }
        public int SectionId { get { return RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy; } set { } }
        public string PageId { get; set; }
        public string GetHelpQuestion { get; set; }
        public bool GetHelpQuerySubmitted { get; set; }
        public string GetHelpErrorMessage { get; set; }

        public string GetHelpAction { get; set; }
    }
}
