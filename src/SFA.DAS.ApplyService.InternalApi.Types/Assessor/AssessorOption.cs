using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Types.Assessor
{
    public class AssessorOption
    {
        public List<AssessorQuestion> FurtherQuestions { get; set; }
        public string Value { get; set; }
        public string Label { get; set; }
        public string HintText { get; set; }
    }
}
