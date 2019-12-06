using System;
using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class SearchByUkprnViewModel : IPageViewModel
    {
        public string UKPRN { get; set; }

        public string Title { get { return "What is your organisation's UK provider reference number (UKPRN)?"; } set { } }

        public Guid ApplicationId { get; set; }

        public string SequenceId { get; set; }
        public int SectionId { get; set; }
        public string PageId { get; set; }

        public string GetHelpQuestion { get; set; }

        public bool GetHelpQuerySubmitted { get; set; }

        public string GetHelpErrorMessage { get; set; }

        public List<ValidationErrorDetail> ErrorMessages { get; set; }
    }
}