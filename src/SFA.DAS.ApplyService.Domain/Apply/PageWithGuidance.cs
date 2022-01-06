using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class PageWithGuidance
    {
        public string PageId { get; set; }
        public List<string> GuidanceInformation { get; set; }
    }
}
