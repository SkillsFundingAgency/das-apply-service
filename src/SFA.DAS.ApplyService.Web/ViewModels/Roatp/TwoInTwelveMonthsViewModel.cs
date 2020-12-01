using System;
using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class TwoInTwelveMonthsViewModel : IPageViewModel
    {
        public bool? HasTwoInTwelveMonths { get; set; }

        public string Title { get { return "Have you had 2 unsuccessful applications in the last 12 months?"; } set { } }

        public Guid ApplicationId { get; set; }

        public string SequenceId { get; set; }
        public int SectionId { get; set; }
        public string PageId { get; set; }

        public string GetHelpQuestion { get; set; }

        public bool GetHelpQuerySubmitted { get; set; }

        public string GetHelpErrorMessage { get; set; }

        public List<ValidationErrorDetail> ErrorMessages { get; set; }

        public string GetHelpAction { get { return "TwoInTwelveMonths"; } set { } }
    }
}