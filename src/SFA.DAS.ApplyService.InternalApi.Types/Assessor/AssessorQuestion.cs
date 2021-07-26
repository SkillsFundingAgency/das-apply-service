using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Types.Assessor
{
    public class AssessorQuestion
    {
        public string QuestionId { get; set; }
        public string Label { get; set; }
        public string QuestionBodyText { get; set; }
        public string InputType { get; set; }
        public string InputPrefix { get; set; }
        public string InputSuffix { get; set; }

        public List<AssessorOption> Options { get; set; }
    }
}
