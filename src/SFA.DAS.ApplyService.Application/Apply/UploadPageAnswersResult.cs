using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ApplyService.Application.Apply
{
    public class UploadPageAnswersResult
    {
        public bool ValidationPassed { get; set; }
        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }
        public string NextAction { get; set; }
        public string NextActionId { get; set; }
    }
}
