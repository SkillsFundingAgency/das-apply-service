using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ConfirmTrusteesDateOfBirthViewModel : WhosInControlViewModel, IPageViewModel
    {
        public Guid ApplicationId { get; set; }
        public List<TrusteeDateOfBirth> TrusteeDatesOfBirth { get; set; }

        public string Title { get { return "Enter the date of birth for trustees"; } set { } }
        public string SequenceId { get { return RoatpWorkflowSequenceIds.YourOrganisation.ToString(); } set { } }
        public int SectionId { get { return RoatpWorkflowSectionIds.YourOrganisation.WhosInControl; } set { } }
        public string PageId { get; set; }
        public string GetHelpQuestion { get; set; }
        public bool GetHelpQuerySubmitted { get; set; }
        public string GetHelpErrorMessage { get; set; }
        public string GetHelpAction { get { return "ConfirmTrusteesDob"; } set { } }
    }

    public class TrusteeDateOfBirth
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DobMonth { get; set; }
        public string DobYear { get; set; }
    }
}
