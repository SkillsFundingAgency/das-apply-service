using System;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class GetHelpWithQuestionViewModel : IPageViewModel
    {
        public string Title { get; set; }
        public string SequenceId { get; set; }
        public int SectionId { get; set; }

        public string PageId { get; set; }
        public Guid ApplicationId { get; set; }
        public string OrganisationName { get; set; }
        public string UKPRN { get; set; }
        public string EmailAddress { get; set; }
        public string ApplicantFullName { get; set; }
        public string ApplicantsQuery { get; set; }
    }
}
