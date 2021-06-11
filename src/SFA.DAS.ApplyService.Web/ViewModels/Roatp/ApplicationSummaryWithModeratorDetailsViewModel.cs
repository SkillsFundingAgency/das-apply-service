using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ApplicationSummaryWithModeratorDetailsViewModel : ApplicationSummaryViewModel
    {
        public List<AssessorSequence> Sequences { get; set; }
        public List<PageWithGuidance> PagesWithGuidance { get; set; }

        public bool ModerationPassOverturnedToFail { get; set; }
        public bool ModerationPassAndApproved { get; set; }
        public string OversightExternalComments { get; set; }
    }
}