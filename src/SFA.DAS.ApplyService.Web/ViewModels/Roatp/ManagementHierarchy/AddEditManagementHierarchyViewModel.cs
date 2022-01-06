using System;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp.ManagementHierarchy;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class AddEditManagementHierarchyViewModel : ManagementHierarchyViewModel, IPageViewModel
    {
        public Guid ApplicationId { get; set; }

        public string Identifier { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string JobRole { get; set; }

        public string TimeInRoleMonths { get; set; }
        public string TimeInRoleYears { get; set; }

        public string IsPartOfOtherOrgThatGetsFunding { get; set; }

        public string OtherOrgName { get; set; }


        public string DobMonth { get; set; }
        public string DobYear { get; set; }

        public string Email { get; set; }

        public string ContactNumber { get; set; }
        public string Title { get; set; }
        public int SequenceId { get { return RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining; } set { } }
        public int SectionId { get { return RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy; } set { } }
        public string PageId { get; set; }
        public string GetHelpQuestion { get; set; }
        public bool GetHelpQuerySubmitted { get; set; }
        public string GetHelpErrorMessage { get; set; }
        public string GetHelpAction { get; set; }


        public int Index { get; set; }
    }
}
