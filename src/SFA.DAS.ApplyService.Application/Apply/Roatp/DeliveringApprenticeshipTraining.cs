using SFA.DAS.ApplyService.Domain.Sectors;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public static partial class RoatpWorkflowPageIds
    {
        public static class DeliveringApprenticeshipTraining
        {
            public const string OverallAccountability = "7100";
            public const string ManagementHierarchy = "7200";
            public const string QualityAndHighStandards = "7300";
            public const string QualityAndHighStandardsMonitoring = "7305";
            public const string QualityAndHighStandardsResponsible = "7310";
            public const string QualityAndHighStandardsExpectations = "7320";
            public const string TeamResponsible = "7500";
            public const string PersonResponsible_SoleTrader = "7510";
            public const string PersonResponsible = "7520";
            public const string DevelopAndDeliverTraining_Team = "7530";
            public const string OverallManager_MainEmployer = "7540";
            public const string OverallManager_Supporting = "7590";
            public const string HowTeamWorked_Organisations = "7570";
            public const string HowTeamWorked_OrganisationsAndEmployers = "7591";
            public const string HowTeamWorked_Employers = "7592";
            public const string DevelopAndDeliverTraining_Person = "7560";
            public const string HowPersonWorked_Organisations = "7593";
            public const string HowPersonWorked_OrganisationsAndEmployers = "7594";
            public const string HowPersonWorked_Employers = "7595";
            public const string ChooseYourOrganisationsSectors = "7600";
            public const string ProfessionalDevelopmentPolicy = "7700";
            public const string ImproveEmployeeSectorExpertise = "7710";
            public const string ImproveEmployeeKnowledge = "7720";
            public const string DeliveringTrainingOther = "other";

            public static class AgricultureEnvironmentalAndAnimalCare
            {
                public const string Name = "Agriculture, environmental and animal care";
                public const string MostExperiencedEmployee = "7610";
                public const string EmployeesExperience = "7611";
                public const string TypeOfTrainingDelivered = "7612";
                public const string HowTrainingHasBeenDelivered = "7613";

                public static SectorQuestionIds SectorQuestionIds =>
                    new SectorQuestionIds
                    {
                        FullName = "DAT-7610.1",
                        JobRole = "DAT-7610.2",
                        TimeInRole = "DAT-7610.3",
                        ExperienceOfDelivering = "DAT-76111",
                        DoTheyHaveQualifications = "DAT-76112",
                        AwardingBodies = "DAT-76113",
                        TradeMemberships = "DAT-76114",
                        WhatTypeOfTrainingDelivered = "DAT-76121",
                        HowHaveTheyDeliveredTraining = "DAT-76131",
                        ExperienceOfDeliveringTraining = "DAT-76132",
                        TypicalDurationOfTraining = "DAT-76133"
                    };
            }

            public static class BusinessAndAdministration
            {
                public const string Name = "Business and administration";
                public const string MostExperiencedEmployee = "7615";
                public const string EmployeesExperience = "7616";
                public const string TypeOfTrainingDelivered = "7617";
                public const string HowTrainingHasBeenDelivered = "7618";

                public static SectorQuestionIds SectorQuestionIds =>
                    new SectorQuestionIds
                    {
                        FullName = "DAT-7615.1",
                        JobRole = "DAT-7615.2",
                        TimeInRole = "DAT-7615.3",
                        ExperienceOfDelivering = "DAT-76161",
                        DoTheyHaveQualifications = "DAT-76162",
                        AwardingBodies = "DAT-76163",
                        TradeMemberships = "DAT-76164",
                        WhatTypeOfTrainingDelivered = "DAT-76171",
                        HowHaveTheyDeliveredTraining = "DAT-76181",
                        ExperienceOfDeliveringTraining = "DAT-76182",
                        TypicalDurationOfTraining = "DAT-76183"
                    };
            }

            public static class CareServices
            {
                public const string Name = "Care Services";
                public const string MostExperiencedEmployee = "7620";
                public const string EmployeesExperience = "7621";
                public const string TypeOfTrainingDelivered = "7622";
                public const string HowTrainingHasBeenDelivered = "7623";

                public static SectorQuestionIds SectorQuestionIds =>
                    new SectorQuestionIds
                    {
                        FullName = "DAT-7620.1",
                        JobRole = "DAT-7620.2",
                        TimeInRole = "DAT-7620.3",
                        ExperienceOfDelivering = "DAT-76211",
                        DoTheyHaveQualifications = "DAT-76212",
                        AwardingBodies = "DAT-76213",
                        TradeMemberships = "DAT-76214",
                        WhatTypeOfTrainingDelivered = "DAT-76221",
                        HowHaveTheyDeliveredTraining = "DAT-76231",
                        ExperienceOfDeliveringTraining = "DAT-76232",
                        TypicalDurationOfTraining = "DAT-76233"
                    };
            }

            public static class CateringAndHospitality
            {
                public const string Name = "Catering and hospitality";
                public const string MostExperiencedEmployee = "7625";
                public const string EmployeesExperience = "7626";
                public const string TypeOfTrainingDelivered = "7627";
                public const string HowTrainingHasBeenDelivered = "7628";

                public static SectorQuestionIds SectorQuestionIds =>
                    new SectorQuestionIds
                    {
                        FullName = "DAT-7625.1",
                        JobRole = "DAT-7625.2",
                        TimeInRole = "DAT-7625.3",
                        ExperienceOfDelivering = "DAT-76261",
                        DoTheyHaveQualifications = "DAT-76262",
                        AwardingBodies = "DAT-76263",
                        TradeMemberships = "DAT-76264",
                        WhatTypeOfTrainingDelivered = "DAT-76271",
                        HowHaveTheyDeliveredTraining = "DAT-76281",
                        ExperienceOfDeliveringTraining = "DAT-76282",
                        TypicalDurationOfTraining = "DAT-76283"
                    };
            }

            public static class Construction
            {
                public const string Name = "Construction";
                public const string MostExperiencedEmployee = "7630";
                public const string EmployeesExperience = "7631";
                public const string TypeOfTrainingDelivered = "7632";
                public const string HowTrainingHasBeenDelivered = "7633";

                public static SectorQuestionIds SectorQuestionIds =>
                    new SectorQuestionIds
                    {
                        FullName = "DAT-7630.1",
                        JobRole = "DAT-7630.2",
                        TimeInRole = "DAT-7630.3",
                        ExperienceOfDelivering = "DAT-76311",
                        DoTheyHaveQualifications = "DAT-76312",
                        AwardingBodies = "DAT-76313",
                        TradeMemberships = "DAT-76314",
                        WhatTypeOfTrainingDelivered = "DAT-76321",
                        HowHaveTheyDeliveredTraining = "DAT-76331",
                        ExperienceOfDeliveringTraining = "DAT-76332",
                        TypicalDurationOfTraining = "DAT-76333"
                    };
            }

            public static class CreativeAndDesign
            {
                public const string Name = "Creative and design";
                public const string MostExperiencedEmployee = "7635";
                public const string EmployeesExperience = "7636";
                public const string TypeOfTrainingDelivered = "7637";
                public const string HowTrainingHasBeenDelivered = "7638";

                public static SectorQuestionIds SectorQuestionIds =>
                    new SectorQuestionIds
                    {
                        FullName = "DAT-7635.1",
                        JobRole = "DAT-7635.2",
                        TimeInRole = "DAT-7635.3",
                        ExperienceOfDelivering = "DAT-76361",
                        DoTheyHaveQualifications = "DAT-76362",
                        AwardingBodies = "DAT-76363",
                        TradeMemberships = "DAT-76364",
                        WhatTypeOfTrainingDelivered = "DAT-76371",
                        HowHaveTheyDeliveredTraining = "DAT-76381",
                        ExperienceOfDeliveringTraining = "DAT-76382",
                        TypicalDurationOfTraining = "DAT-76383"
                    };
            }

            public static class Digital
            {
                public const string Name = "Digital";
                public const string MostExperiencedEmployee = "7640";
                public const string EmployeesExperience = "7641";
                public const string TypeOfTrainingDelivered = "7642";
                public const string HowTrainingHasBeenDelivered = "7643";

                public static SectorQuestionIds SectorQuestionIds =>
                    new SectorQuestionIds
                    {
                        FullName = "DAT-7640.1",
                        JobRole = "DAT-7640.2",
                        TimeInRole = "DAT-7640.3",
                        ExperienceOfDelivering = "DAT-76411",
                        DoTheyHaveQualifications = "DAT-76412",
                        AwardingBodies = "DAT-76413",
                        TradeMemberships = "DAT-76414",
                        WhatTypeOfTrainingDelivered = "DAT-76421",
                        HowHaveTheyDeliveredTraining = "DAT-76431",
                        ExperienceOfDeliveringTraining = "DAT-76432",
                        TypicalDurationOfTraining = "DAT-76433"
                    };
            }

            public static class EducationAndChildcare
            {
                public const string Name = "Education and childcare";
                public const string MostExperiencedEmployee = "7645";
                public const string EmployeesExperience = "7646";
                public const string TypeOfTrainingDelivered = "7647";
                public const string HowTrainingHasBeenDelivered = "7648";

                public static SectorQuestionIds SectorQuestionIds =>
                    new SectorQuestionIds
                    {
                        FullName = "DAT-7645.1",
                        JobRole = "DAT-7645.2",
                        TimeInRole = "DAT-7645.3",
                        ExperienceOfDelivering = "DAT-76461",
                        DoTheyHaveQualifications = "DAT-76462",
                        AwardingBodies = "DAT-76463",
                        TradeMemberships = "DAT-76464",
                        WhatTypeOfTrainingDelivered = "DAT-76471",
                        HowHaveTheyDeliveredTraining = "DAT-76481",
                        ExperienceOfDeliveringTraining = "DAT-76482",
                        TypicalDurationOfTraining = "DAT-76483"
                    };
            }

            public static class EngineeringAndManufacturing
            {
                public const string Name = "Engineering and manufacturing";
                public const string MostExperiencedEmployee = "7650";
                public const string EmployeesExperience = "7651";
                public const string TypeOfTrainingDelivered = "7652";
                public const string HowTrainingHasBeenDelivered = "7653";

                public static SectorQuestionIds SectorQuestionIds =>
                    new SectorQuestionIds
                    {
                        FullName = "DAT-7650.1",
                        JobRole = "DAT-7650.2",
                        TimeInRole = "DAT-7650.3",
                        ExperienceOfDelivering = "DAT-76511",
                        DoTheyHaveQualifications = "DAT-76512",
                        AwardingBodies = "DAT-76513",
                        TradeMemberships = "DAT-76514",
                        WhatTypeOfTrainingDelivered = "DAT-76521",
                        HowHaveTheyDeliveredTraining = "DAT-76531",
                        ExperienceOfDeliveringTraining = "DAT-76532",
                        TypicalDurationOfTraining = "DAT-76533"
                    };
            }

            public static class HairAndBeauty
            {
                public const string Name = "Hair and Beauty";
                public const string MostExperiencedEmployee = "7655";
                public const string EmployeesExperience = "7656";
                public const string TypeOfTrainingDelivered = "7657";
                public const string HowTrainingHasBeenDelivered = "7658";

                public static SectorQuestionIds SectorQuestionIds =>
                    new SectorQuestionIds
                    {
                        FullName = "DAT-7655.1",
                        JobRole = "DAT-7655.2",
                        TimeInRole = "DAT-7655.3",
                        ExperienceOfDelivering = "DAT-76561",
                        DoTheyHaveQualifications = "DAT-76562",
                        AwardingBodies = "DAT-76563",
                        TradeMemberships = "DAT-76564",
                        WhatTypeOfTrainingDelivered = "DAT-76571",
                        HowHaveTheyDeliveredTraining = "DAT-76581",
                        ExperienceOfDeliveringTraining = "DAT-76582",
                        TypicalDurationOfTraining = "DAT-76583"
                    };
            }

            public static class HealthAndScience
            {
                public const string Name = "Health and Science";
                public const string MostExperiencedEmployee = "7660";
                public const string EmployeesExperience = "7661";
                public const string TypeOfTrainingDelivered = "7662";
                public const string HowTrainingHasBeenDelivered = "7663";

                public static SectorQuestionIds SectorQuestionIds =>
                    new SectorQuestionIds
                    {
                        FullName = "DAT-7660.1",
                        JobRole = "DAT-7660.2",
                        TimeInRole = "DAT-7660.3",
                        ExperienceOfDelivering = "DAT-76611",
                        DoTheyHaveQualifications = "DAT-76612",
                        AwardingBodies = "DAT-76613",
                        TradeMemberships = "DAT-76614",
                        WhatTypeOfTrainingDelivered = "DAT-76621",
                        HowHaveTheyDeliveredTraining = "DAT-76631",
                        ExperienceOfDeliveringTraining = "DAT-76632",
                        TypicalDurationOfTraining = "DAT-76633"
                    };
            }

            public static class LegalFinanceAndAccounting
            {
                public const string Name = "Legal, finance and accounting";
                public const string MostExperiencedEmployee = "7665";
                public const string EmployeesExperience = "7666";
                public const string TypeOfTrainingDelivered = "7667";
                public const string HowTrainingHasBeenDelivered = "7668";

                public static SectorQuestionIds SectorQuestionIds =>
                    new SectorQuestionIds
                    {
                        FullName = "DAT-7665.1",
                        JobRole = "DAT-7665.2",
                        TimeInRole = "DAT-7665.3",
                        ExperienceOfDelivering = "DAT-76661",
                        DoTheyHaveQualifications = "DAT-76662",
                        AwardingBodies = "DAT-76663",
                        TradeMemberships = "DAT-76664",
                        WhatTypeOfTrainingDelivered = "DAT-76671",
                        HowHaveTheyDeliveredTraining = "DAT-76681",
                        ExperienceOfDeliveringTraining = "DAT-76682",
                        TypicalDurationOfTraining = "DAT-76683"
                    };
            }

            public static class ProtectiveServices
            {
                public const string Name = "Protective services";
                public const string MostExperiencedEmployee = "7670";
                public const string EmployeesExperience = "7671";
                public const string TypeOfTrainingDelivered = "7672";
                public const string HowTrainingHasBeenDelivered = "7673";

                public static SectorQuestionIds SectorQuestionIds =>
                    new SectorQuestionIds
                    {
                        FullName = "DAT-7670.1",
                        JobRole = "DAT-7670.2",
                        TimeInRole = "DAT-7670.3",
                        ExperienceOfDelivering = "DAT-76711",
                        DoTheyHaveQualifications = "DAT-76712",
                        AwardingBodies = "DAT-76713",
                        TradeMemberships = "DAT-76714",
                        WhatTypeOfTrainingDelivered = "DAT-76721",
                        HowHaveTheyDeliveredTraining = "DAT-76731",
                        ExperienceOfDeliveringTraining = "DAT-76732",
                        TypicalDurationOfTraining = "DAT-76733"
                    };
            }

            public static class SalesMarketingAndProcurement
            {
                public const string Name = "Sales, marketing and procurement";
                public const string MostExperiencedEmployee = "7675";
                public const string EmployeesExperience = "7676";
                public const string TypeOfTrainingDelivered = "7677";
                public const string HowTrainingHasBeenDelivered = "7678";

                public static SectorQuestionIds SectorQuestionIds =>
                    new SectorQuestionIds
                    {
                        FullName = "DAT-7675.1",
                        JobRole = "DAT-7675.2",
                        TimeInRole = "DAT-7675.3",
                        ExperienceOfDelivering = "DAT-76761",
                        DoTheyHaveQualifications = "DAT-76762",
                        AwardingBodies = "DAT-76763",
                        TradeMemberships = "DAT-76764",
                        WhatTypeOfTrainingDelivered = "DAT-76771",
                        HowHaveTheyDeliveredTraining = "DAT-76781",
                        ExperienceOfDeliveringTraining = "DAT-76782",
                        TypicalDurationOfTraining = "DAT-76783"
                    };
            }

            public static class TransportAndLogistics
            {
                public const string Name = "Transport and logistics";
                public const string MostExperiencedEmployee = "7680";
                public const string EmployeesExperience = "7681";
                public const string TypeOfTrainingDelivered = "7682";
                public const string HowTrainingHasBeenDelivered = "7683";

                public static SectorQuestionIds SectorQuestionIds =>
                    new SectorQuestionIds
                    {
                        FullName = "DAT-7680.1",
                        JobRole = "DAT-7680.2",
                        TimeInRole = "DAT-7680.3",
                        ExperienceOfDelivering = "DAT-76811",
                        DoTheyHaveQualifications = "DAT-76812",
                        AwardingBodies = "DAT-76813",
                        TradeMemberships = "DAT-76814",
                        WhatTypeOfTrainingDelivered = "DAT-76821",
                        HowHaveTheyDeliveredTraining = "DAT-76831",
                        ExperienceOfDeliveringTraining = "DAT-76832",
                        TypicalDurationOfTraining = "DAT-76833"
                    };
            }
        }
    }
}