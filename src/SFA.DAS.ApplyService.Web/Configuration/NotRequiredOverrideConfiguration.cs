
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.Configuration
{
    public class NotRequiredOverrideConfiguration
    {
        public int SequenceId { get; set; }
        public int SectionId { get; set; }
        public List<NotRequiredCondition> Conditions { get; set; }       
        public bool AllConditionsMet
        {
            get
            {
                var conditionsMet = true;
                if (Conditions != null) {
                    foreach (var condition in Conditions)
                    {
                        if (condition.Value != condition.MustEqual)
                        {
                            conditionsMet = false;
                            break;
                        }
                    }
                }
                return conditionsMet;
            }
        }
    }

    public class NotRequiredCondition
    {
        public string ConditionalCheckField { get; set; }
        public string MustEqual { get; set; }
        public string Value { get; set; }
    }
}
