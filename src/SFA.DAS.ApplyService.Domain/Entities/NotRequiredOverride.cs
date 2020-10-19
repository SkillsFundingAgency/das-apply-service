using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class NotRequiredOverride
    {
        public int SequenceId { get; set; }
        public int SectionId { get; set; }
        public List<NotRequiredCondition> Conditions { get; set; }

        public bool AllConditionsMet
        {
            get
            {
                var allConditionsMet = true;

                if (Conditions != null)
                {
                    foreach (var condition in Conditions)
                    {
                        if (condition.Value != condition.MustEqual)
                        {
                            allConditionsMet = false;
                            break;
                        }
                    }
                }

                return allConditionsMet;
            }
        }
    }

    public class NotRequiredCondition
    {
        public string ConditionalCheckField { get; set; }
        public string MustEqual { get; set; }
        public string Value { get; set; }
    }

    public class NotRequiredOverrideConfiguration
    {
        public IEnumerable<NotRequiredOverride> NotRequiredOverrides { get; set; }
    }
}
