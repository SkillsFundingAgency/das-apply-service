using SFA.DAS.ApplyService.Application.Apply.Roatp;
using System;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ConfirmTrusteesViewModel : WhosInControlViewModel, IPageViewModel
    {
        public bool VerifiedCompaniesHouse { get; set; }
        public Guid ApplicationId { get; set; }
        public PeopleInControl Trustees { get; set; }

        public string Title { get { return "Confirm your organisation's trustees"; } set { } }
        public string SequenceId { get { return RoatpWorkflowSequenceIds.YourOrganisation.ToString(); } set { } }
        public int SectionId { get { return RoatpWorkflowSectionIds.YourOrganisation.WhosInControl; } set { } }
        public string PageId { get; set; }
        public string GetHelpQuestion { get; set; }
        public bool GetHelpQuerySubmitted { get; set; }
        public string GetHelpErrorMessage { get; set; }
        public string GetHelpAction { get { return "ConfirmTrusteesNoDob"; } set { } }
    }
}
