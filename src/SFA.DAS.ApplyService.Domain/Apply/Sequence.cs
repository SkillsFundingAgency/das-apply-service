using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class Sequence
    {
        public string SequenceId { get; set; }
        public string Title { get; set; }
        public string LinkTitle { get; set; }
        public bool Active { get; set; }
        public bool Complete { get; set; }
        public List<Section> Sections { get; set; }
        public List<NextSequence> NextSequences { get; set; }
        public string Actor { get; set; }
        public int? Order { get; set; }
    }
}