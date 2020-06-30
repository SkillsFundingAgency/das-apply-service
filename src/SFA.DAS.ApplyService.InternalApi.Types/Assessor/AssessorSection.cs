using System;

namespace SFA.DAS.ApplyService.InternalApi.Types.Assessor
{
    public class AssessorSection
    {
        public Guid Id { get; set; }
        public int SectionNumber { get; set; }
        public string LinkTitle { get; set; }
        public string Status { get; set; }
    }
}
