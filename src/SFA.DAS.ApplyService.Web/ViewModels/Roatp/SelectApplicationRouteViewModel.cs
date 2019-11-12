namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    using Domain.Roatp;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class SelectApplicationRouteViewModel : IPageViewModel
    {
        public IEnumerable<ApplicationRoute> ApplicationRoutes { get; set; }

        [Required(ErrorMessage = "Tell us your organisation's provider route")]
        [Range(1, 3, ErrorMessage = "Tell us your organisation's provider route")]
        public int ApplicationRouteId { get; set; }

        public string Title { get { return "Choose your organisation's provider route"; } set { } }

        public Guid ApplicationId => Guid.Empty;

        public string SequenceId { get; set; }
        public int SectionId { get; set; }
        public string PageId { get; set; }

        public bool GetHelpQuerySubmitted { get; set; }
    }
}
