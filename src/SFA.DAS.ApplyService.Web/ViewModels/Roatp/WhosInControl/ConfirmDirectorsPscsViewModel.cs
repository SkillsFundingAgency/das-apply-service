using SFA.DAS.ApplyService.Application.Apply.Roatp;
using System;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ConfirmDirectorsPscsViewModel : WhosInControlViewModel, IPageViewModel
    {
        public Guid ApplicationId { get; set; }
        public PeopleInControl CompaniesHouseDirectors { get; set; }
        public PeopleInControl CompaniesHousePscs { get; set; }

        public string Title { get { return "Confirm who's in control"; } set { } }
        public string SequenceId { get { return RoatpWorkflowSequenceIds.YourOrganisation.ToString(); } set { } }
        public int SectionId { get { return RoatpWorkflowSectionIds.YourOrganisation.WhosInControl; } set { } }
        public string PageId { get; set; }
        public string GetHelpQuestion { get; set; }
        public bool GetHelpQuerySubmitted { get; set; }
        public string GetHelpErrorMessage { get; set; }
    }
}
