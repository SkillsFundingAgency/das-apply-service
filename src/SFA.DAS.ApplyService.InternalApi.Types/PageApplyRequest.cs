using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.InternalApi.Types
{
    public class PageApplyRequest
    {
        public List<Answer> Answers { get; set; }
        public bool SaveNewAnswers { get; set; }
    }
}