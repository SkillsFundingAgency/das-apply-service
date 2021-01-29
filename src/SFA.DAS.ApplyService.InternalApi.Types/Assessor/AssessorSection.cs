using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Types.Assessor
{
    public class AssessorSection
    {
        public int SequenceNumber { get; set; }
        public int SectionNumber { get; set; }
        public string LinkTitle { get; set; }
        public string Status { get; set; }

        // Internal use only. Don't expose publicly
        [Newtonsoft.Json.JsonIgnore]
        public List<Page> Pages { get; set; }
    }
}
