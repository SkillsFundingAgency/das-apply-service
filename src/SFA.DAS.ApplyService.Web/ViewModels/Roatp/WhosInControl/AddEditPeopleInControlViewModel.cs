using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class AddEditPeopleInControlViewModel : WhosInControlViewModel, IPageViewModel
    {
        public bool DateOfBirthOptional { get; set; }
        public string Identifier { get; set; }

        public Guid ApplicationId { get; set; }
        public string OrganisationType { get; set; }

        [Required(ErrorMessage = "Enter a full name")]
        [MaxLength(255, ErrorMessage = "Enter a full name using 255 characters or less")]
        public string PersonInControlName { get; set; }
        public string PersonInControlDobMonth { get; set; }
        public string PersonInControlDobYear { get; set; }
        public int Index { get; set; }

        public string Title { get { return $"Enter the {Identifier}'s details"; } set { } }
        public string SequenceId { get { return RoatpWorkflowSequenceIds.YourOrganisation.ToString(); } set { } }
        public int SectionId { get { return RoatpWorkflowSectionIds.YourOrganisation.WhosInControl; } set { } }
        public string PageId { get; set; }
        public string GetHelpQuestion { get; set; }
        public bool GetHelpQuerySubmitted { get; set; }
        public string GetHelpErrorMessage { get; set; }

        public string GetHelpAction { get; set; }
    }
}
