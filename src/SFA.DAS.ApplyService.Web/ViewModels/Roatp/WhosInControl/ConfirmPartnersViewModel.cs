﻿using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using System;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ConfirmPartnersViewModel : WhosInControlViewModel, IPageViewModel
    {
        public const int MaximumNumberOfPartners = 50;

        public Guid ApplicationId { get; set; }
        public string BackAction { get; set; }
        public TabularData PartnerData { get; set; }

        public string Title { get { return "Confirm your organisation's partners"; } set { } }
        public int SequenceId { get { return RoatpWorkflowSequenceIds.YourOrganisation; } set { } }
        public int SectionId { get { return RoatpWorkflowSectionIds.YourOrganisation.WhosInControl; } set { } }
        public string PageId { get { return "ConfirmPartners"; } set { } }
        public string GetHelpQuestion { get; set; }
        public bool GetHelpQuerySubmitted { get; set; }
        public string GetHelpErrorMessage { get; set; }
        public string GetHelpAction { get { return "ConfirmPartners"; } set { } }
    }
}
