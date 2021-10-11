﻿using SFA.DAS.ApplyService.Application.Apply.Roatp;
using System;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ConfirmDirectorsPscsViewModel : WhosInControlViewModel, IPageViewModel
    {
        public Guid ApplicationId { get; set; }
        public PeopleInControl CompaniesHouseDirectors { get; set; }
        public PeopleInControl CompaniesHousePscs { get; set; }

        public string Title { get { return "Confirm who's in control"; } set { } }
        public int SequenceId { get { return RoatpWorkflowSequenceIds.YourOrganisation; } set { } }
        public int SectionId { get { return RoatpWorkflowSectionIds.YourOrganisation.WhosInControl; } set { } }
        public string PageId { get { return RoatpWorkflowPageIds.WhosInControl.CompaniesHouseStartPage; } set { } }
        public string GetHelpQuestion { get; set; }
        public bool GetHelpQuerySubmitted { get; set; }
        public string GetHelpErrorMessage { get; set; }

        public string GetHelpContoller { get { return "RoatpWhosInControlApplication"; } set { } }
        public string GetHelpAction { get { return "ConfirmDirectorsPscs"; } set { } }
    }
}
