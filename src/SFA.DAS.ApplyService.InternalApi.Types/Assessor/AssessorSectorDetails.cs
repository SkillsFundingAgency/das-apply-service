namespace SFA.DAS.ApplyService.InternalApi.Types.Assessor
{
    public class AssessorSectorDetails
    {
        public string SectorName { get; set; }
        public string FullName { get; set; }
        public string JobRole { get; set; }
        public string TimeInRole { get; set; }

        public string ExperienceOfDelivering { get; set; }
        public string WhereDidTheyGainThisExperience { get; set; }
        public string DoTheyHaveQualifications { get; set; }
        public string WhatQualificationsDoTheyHave { get; set; }
        public string ApprovedByAwardingBodies { get; set; }
        public string NamesOfAwardingBodies { get; set; }
        public string DoTheyHaveTradeBodyMemberships { get; set; }
        public string NamesOfTradeBodyMemberships { get; set; }

        public string WhatTypeOfTrainingDelivered { get; set; }

        public string HowHaveTheyDeliveredTraining { get; set; }

        public string[] HowIsTrainingDelivered => HowHaveTheyDeliveredTraining?.Split(',');
        public string ExperienceOfDeliveringTraining { get; set; }
        public string TypicalDurationOfTraining { get; set; }
    }
}
