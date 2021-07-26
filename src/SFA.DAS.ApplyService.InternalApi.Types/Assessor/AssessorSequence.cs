using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Types.Assessor
{
    public class AssessorSequence
    {
        public Guid Id { get; set; }
        public int SequenceNumber { get; set; }
        public string SequenceTitle { get; set; }
        public List<AssessorSection> Sections { get; set; }
    }
}
