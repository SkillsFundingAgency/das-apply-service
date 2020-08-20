using System;

namespace SFA.DAS.ApplyService.InternalApi.Types.Moderator
{
    public class ModeratorSection
    {
        public Guid Id { get; set; }
        public int SectionNumber { get; set; }
        public string LinkTitle { get; set; }
        public string Status { get; set; }
    }
}
