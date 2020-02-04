
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.Configuration
{
    public class TaskListConfiguration
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Sequential { get; set; }

        public List<StartupPage> StartupPages { get; set; }
    }


    public class StartupPage
    {
        public int ProviderTypeId { get; set; }
        public string PageId { get; set; }
    }
}
