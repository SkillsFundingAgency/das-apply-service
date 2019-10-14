using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class Next
    {
        public string Action { get; set; }
        public string ReturnId { get; set; }
        public List<Condition> Conditions { get; set; }
        public bool ConditionMet { get; set; }
    }
}