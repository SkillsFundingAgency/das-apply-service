using System.Collections.Generic;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ApplicationSummaryWithModeratorDetails : ApplicationSummaryViewModel
    {
        public List<AssessorSequence> Sequences { get; set; }
        public List<AssessorPage> PagesWithGuidance { get; set; }
    }
}