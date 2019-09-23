
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.Configuration
{
    public class TaskListConfiguration
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public List<StartupPage> StartupPages { get; set; }
    }


    public class StartupPage
    {
        public string ProviderType { get; set; }
        public string PageId { get; set; }
    }
}
