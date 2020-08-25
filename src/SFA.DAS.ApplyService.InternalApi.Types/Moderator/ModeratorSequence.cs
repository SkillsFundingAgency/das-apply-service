using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Types.Moderator
{
    public class ModeratorSequence
    {
        public Guid Id { get; set; }
        public int SequenceNumber { get; set; }
        public string SequenceTitle { get; set; }
        public List<ModeratorSection> Sections { get; set; }
    }
}
