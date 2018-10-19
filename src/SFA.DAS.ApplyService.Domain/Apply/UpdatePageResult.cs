using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class UpdatePageResult
    {
        public Page Page { get; set; }
        public bool ValidationPassed { get; set; }
        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }
    }
}