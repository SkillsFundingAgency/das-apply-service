using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.Configuration
{
    public class NotRequiredOverride
    {
        public int SequenceId { get; set; }
        public int SectionId { get; set; }
        public List<NotRequiredCondition> Conditions { get; set; }       
    }

    public class NotRequiredCondition
    {
        public string ConditionalCheckField { get; set; }
        public string MustEqual { get; set; }
        public string Value { get; set; }
    }
}
