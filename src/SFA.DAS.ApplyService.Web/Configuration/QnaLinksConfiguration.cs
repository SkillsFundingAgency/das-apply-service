using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.Configuration
{
    public class QnaLinksConfiguration
    {
        public string PageId { get; set; }
        public List<Link> Links { get; set; }
    }

    public class Link
    {
        public string Caption { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
