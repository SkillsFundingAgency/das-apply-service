using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class ApplicationSectionPageResponse
    {
        public Page Page { get; set; } // simplify page data to minimum required to render in Assessor
        public bool IsLastPage { get; set; }
    }
}
