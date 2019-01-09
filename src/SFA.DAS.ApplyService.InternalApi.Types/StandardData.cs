using System;

namespace SFA.DAS.ApplyService.InternalApi.Types
{
    public class StandardData
    {
        public int? Level { get; set; }
        public string Category { get; set; }
        public string IfaStatus { get; set; }

        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime? LastDateForNewStarts { get; set; }
        public bool IfaOnly { get; set; }

        public int? Duration { get; set; }
        public int? MaxFunding { get; set; }
        public string Ssa1 { get; set; }
        public string Ssa2 { get; set; }
        public string OverviewOfRole { get; set; }
        public DateTime? PublishedDate { get; set; }
        public bool? IsPublished { get; set; }

        public bool? IsActiveStandardInWin { get; set; }

        public string FatUri { get; set; }
        public string IfaUri { get; set; }
    }
}
