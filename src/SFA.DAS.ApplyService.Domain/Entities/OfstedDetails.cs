namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class OfstedDetails
    {
        public bool? HasHadFullInspection { get; set; }
        public bool? ReceivedFullInspectionGradeForApprenticeships { get; set; }
        public string FullInspectionOverallEffectivenessGrade { get; set; }
        public bool? HasHadMonitoringVisit { get; set; }
        public bool? Has2MonitoringVisitsGradedInadequate { get; set; }
        public bool? HasMonitoringVisitGradedInadequateInLast18Months { get; set; }
        public bool? HasMaintainedFundingSinceInspection { get; set; }
        public bool? HasHadShortInspectionWithinLast3Years { get; set; }
        public bool? HasMaintainedFullGradeInShortInspection { get; set; }
        public string FullInspectionApprenticeshipGrade { get; set; }
        public bool? GradeWithinTheLast3Years { get; set; }
    }
}
