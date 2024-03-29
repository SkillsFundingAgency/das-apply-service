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

        public int SequenceId { get; set; }
        public int SectionId { get; set; }
        public string PageId { get { return "UKPRN"; } set { } }

        public string GetHelpQuestion { get; set; }

        public bool GetHelpQuerySubmitted { get; set; }

        public string GetHelpErrorMessage { get; set; }

        public List<ValidationErrorDetail> ErrorMessages { get; set; }

        public string GetHelpAction { get { return "EnterApplicationUkprn"; } set { } }
    }
}