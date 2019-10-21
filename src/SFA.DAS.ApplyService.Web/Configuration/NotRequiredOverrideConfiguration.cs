using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Configuration
{
    public class NotRequiredOverrideConfiguration
    {
        public int SequenceId { get; set; }
        public int SectionId { get; set; }
        public string ConditionalCheckField { get; set; }
        public string MustEqual { get; set; }
    }
}
