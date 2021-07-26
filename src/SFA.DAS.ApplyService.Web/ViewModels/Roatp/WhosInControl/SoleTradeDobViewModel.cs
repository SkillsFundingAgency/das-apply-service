using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class SoleTradeDobViewModel : WhosInControlViewModel, IPageViewModel
    {
        public const string DobFieldPrefix = "SoleTraderDob";

        public Guid ApplicationId { get; set; }
        public string SoleTraderName { get; set; }
        public string SoleTraderDobMonth { get; set; }
        public string SoleTraderDobYear { get; set; }

        public string Title { get { return $"What is the sole trader's date of birth?"; } set { } }
        public int SequenceId { get { return RoatpWorkflowSequenceIds.YourOrganisation; } set { } }
        public int SectionId { get { return RoatpWorkflowSectionIds.YourOrganisation.WhosInControl; } set { } }
        public string PageId { get; set; }
        public string GetHelpQuestion { get; set; }
        public bool GetHelpQuerySubmitted { get; set; }
        public string GetHelpErrorMessage { get; set; }
        public string GetHelpAction { get { return "AddSoleTradeDob"; } set { } }
    }
}
