using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Domain.Review
{
    public class Outcome
    {
        public string SectionId { get; set; }
        public string PageId { get; set; }
        public string QuestionId { get; set; }
        public List<Check> Checks { get; set; }
        public string Result { get; set; }
        public string Message { get; set; }
    }

    public class Check
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
