using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.UpdatePageAnswers
{
    public class UpdatePageAnswersResult
    {
        public Page Page { get; set; }
        public bool ValidationPassed { get; set; }
        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }
    }
}