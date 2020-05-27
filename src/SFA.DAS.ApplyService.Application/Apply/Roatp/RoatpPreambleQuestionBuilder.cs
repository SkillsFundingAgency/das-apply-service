
using SFA.DAS.ApplyService.Domain.Sectors;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Apply;
    using Domain.Roatp;
    using Newtonsoft.Json;
    using SFA.DAS.ApplyService.Domain.Ukrlp;

    public static class RoatpPreambleQuestionIdConstants
    {
        public static string UKPRN = "PRE-10";
        public static string UkrlpLegalName = "PRE-20";
        public static string UkrlpWebsite = "PRE-30";
        public static string UKrlpNoWebsite = "PRE-31";
        public static string UkrlpLegalAddressLine1 = "PRE-40";
        public static string UkrlpLegalAddressLine2 = "PRE-41";
        public static string UkrlpLegalAddressLine3 = "PRE-42";
        public static string UkrlpLegalAddressLine4 = "PRE-43";
        public static string UkrlpLegalAddressTown = "PRE-44";
        public static string UkrlpLegalAddressPostcode = "PRE-45";
        public static string UkrlpTradingName = "PRE-46";
        public static string CompaniesHouseManualEntryRequired = "PRE-50";
        public static string UkrlpVerificationCompanyNumber = "PRE-51";
        public static string CompaniesHouseCompanyName = "PRE-52";
        public static string CompaniesHouseCompanyType = "PRE-53";
        public static string CompaniesHouseCompanyStatus = "PRE-54";
        public static string CompaniesHouseIncorporationDate = "PRE-55";
        public static string UkrlpVerificationCompany = "PRE-56";
        public static string CharityCommissionTrusteeManualEntry = "PRE-60";
        public static string UkrlpVerificationCharityRegNumber = "PRE-61";
        public static string CharityCommissionCharityName = "PRE-62";
        public static string CharityCommissionRegistrationDate = "PRE-64";
        public static string UkrlpVerificationCharity = "PRE-65";
        public static string UkrlpVerificationSoleTraderPartnership = "PRE-70";
        public static string UkrlpPrimaryVerificationSource = "PRE-80";
        public static string OnRoatp = "PRE-90";
        public static string RoatpCurrentStatus = "PRE-91";
        public static string RoatpRemovedReason = "PRE-92";
        public static string RoatpStatusDate = "PRE-93";
        public static string RoatpProviderRoute = "PRE-94";
        public static string LevyPayingEmployer = "PRE-95";
        public static string ApplyProviderRoute = "YO-1";
       
        public static string COAStage1Application = "COA-1";                  
    }

    public class RoatpYourOrganisationQuestionIdConstants
    {
        public static string WebsiteManuallyEntered = "YO-41";
        public static string IcoNumber = "YO-30";
        public static string CompaniesHouseDirectors = "YO-70";
        public static string CompaniesHousePSCs = "YO-71";
        public static string CompaniesHouseDetailsConfirmed = "YO-75";
        public static string CharityCommissionTrustees = "YO-80";
        public static string CharityCommissionDetailsConfirmed = "YO-85";
        public static string SoleTradeOrPartnership = "YO-100";
        public static string PartnershipType = "YO-101";
        public static string AddPartners = "YO-110";
        public static string AddSoleTradeDob = "YO-120";
        public static string AddPeopleInControl = "YO-130";
        public static string OfficeForStudents = "YO-235";
        public static string InitialTeacherTraining = "YO-240";
        public static string HasHadFullInspection = "YO-260";
        public static string ReceivedFullInspectionGradeForApprenticeships = "YO-270";
        public static string FullInspectionOverallEffectivenessGrade = "YO-280";
        public static string HasHadMonitoringVisit = "YO-290";
        public static string HasMaintainedFundingSinceInspection = "YO-320";
        public static string HasHadShortInspectionWithinLast3Years = "YO-330";
        public static string HasMaintainedFullGradeInShortInspection = "YO-340";
        public static string FullInspectionApprenticeshipGradeOfsFunded = "YO-300";
        public static string FullInspectionApprenticeshipGradeNonOfsFunded = "YO-301";
        public static string GradeWithinLast3YearsOfsFunded = "YO-310";
        public static string GradeWithinLast3YearsNonOfsFunded = "YO-311";

        public static string IsPostGradTrainingOnlyApprenticeship = "YO-250";
		public static string HasDeliveredTrainingAsSubcontractor = "YO-350";
        public static string ContractFileName = "YO-360";
    }

    public class RoatpCriminalComplianceChecksQuestionIdConstants
    {
        public static string CompositionCreditors = "CC-20";
        public static string OrganisationFailedToRepayFunds = "CC-21";
        public static string OrganisationContractTermination = "CC-22";
    }

    public class RoatpDeliveringApprenticeshipTrainingQuestionIdConstants
    {
        public static string ManagementHierarchy = "DAT-720";
    }

    public static class RoatpWorkflowSequenceIds
    {
        public const int Preamble = 0;
        public const int YourOrganisation = 1;
        public const int FinancialEvidence = 2;
        public const int CriminalComplianceChecks = 3;
        public const int ProtectingYourApprentices = 4;
        public const int ReadinessToEngage = 5;
        public const int PlanningApprenticeshipTraining = 6;
        public const int DeliveringApprenticeshipTraining = 7;
        public const int EvaluatingApprenticeshipTraining = 8;
        public const int Finish = 9;
        public const int ConditionsOfAcceptance = 99;
    }

    public static class RoatpWorkflowSectionIds
    {
        public const int Preamble = 1;
        public const int ConditionsOfAcceptance = 1;

        public static class YourOrganisation
        {
            public const int WhatYouWillNeed = 1;
            public const int OrganisationDetails = 2;
            public const int WhosInControl = 3;
            public const int DescribeYourOrganisation = 4;
            public const int ExperienceAndAccreditations = 5;
        }

        public static class FinancialEvidence
        {
            public const int WhatYouWillNeed = 1;
            public const int YourOrganisationsFinancialEvidence = 2;
        }

        public static class CriminalComplianceChecks
        {
            public const int WhatYouWillNeed = 1;
            public const int ChecksOnYourOrganisation = 2;
            public const int WhatYouWillNeed_CheckOnWhosInControl = 3;
            public const int CheckOnWhosInControl = 4;
        }

        public static class ProtectingYourApprentices
        {
            public const int WhatYouWillNeed = 1;
        }

        public static class ReadinessToEngage
        {
            public const int WhatYouWillNeed = 1;
        }

        public static class PlanningApprenticeshipTraining
        {
            public const int WhatYouWillNeed = 1;
        }

        public static class DeliveringApprenticeshipTraining
        {
            public const int WhatYouWillNeed = 1;
            public const int ManagementHierarchy = 3;
            public const int YourSectorsAndEmployees = 6;
        }

        public static class EvaluatingApprenticeshipTraining
        {
            public const int WhatYouWillNeed = 1;
        }

        public static class Finish
        {
            public const int ApplicationPermissionsAndChecks = 1;
            public const int CommercialInConfidenceInformation = 2;
            public const int TermsAndConditions = 3;
            public const int SubmitApplication = 4;
        }
    }

    public static class RoatpWorkflowPageIds
    {
        public static string Preamble = "1";
        public static string ProviderRoute = "2";
        public static string YourOrganisationIntroductionMain = "10";
        public static string YourOrganisationIntroductionEmployer = "11";
        public static string YourOrganisationIntroductionSupporting = "12";
        public static string YourOrganisationParentCompanyCheck = "20";
        public static string YourOrganisationParentCompanyDetails = "21";
        public static string WebsiteManuallyEntered = "40";
        public static string YourOrganisationIcoNumber = "30";
        public static string ConditionsOfAcceptance = "999999";

        public static class WhosInControl
        {
            public static string CompaniesHouseStartPage = "70";
            public static string CharityCommissionStartPage = "80";
            public static string CharityCommissionConfirmTrustees = "80";
            public static string CharityCommissionNoTrustees = "90";
            public static string SoleTraderPartnership = "100";
            public static string PartnershipType = "101";
            public static string AddPartners = "110";
            public static string AddSoleTraderDob = "120";
            public static string AddPeopleInControl = "130";
        }

        public static class ManagementHierarchy
        {
              public static string AddManagementHierarchy = "7200";
        }

        public static class DescribeYourOrganisation
        {
            public static string MainSupportingStartPage = "140";
            public static string EmployerStartPage = "150";
        }

        public static class ExperienceAndAccreditations
        { 
            public static string OfficeForStudents = "235";
            public static string InitialTeacherTraining = "240";
            public static string HasHadFullInspection = "260";
            public static string ReceivedFullInspectionGradeForApprenticeships = "270";
            public static string FullInspectionOverallEffectivenessGrade = "280";
            public static string HasHadMonitoringVisit = "290";
            public static string HasMaintainedFundingSinceInspection = "320";
            public static string HasHadShortInspectionWithinLast3Years = "330";
            public static string HasMaintainedFullGradeInShortInspection = "340";
            public static string FullInspectionApprenticeshipGradeNonOfsFunded = "300";
            public static string FullInspectionApprenticeshipGradeOfsFunded = "301";
            public static string GradeWithinLast3YearsOfsFunded = "310";
            public static string GradeWithinLast3YearsNonOfsFunded = "311";
            public static string IsPostGradTrainingOnlyApprenticeship = "250";
			public static string SubcontractorDeclaration = "350";
            public static string SubcontractorContractFile = "360";
        }

        public static class CriminalComplianceChecks
        {
            public static string CompositionCreditors = "3100";
            public static string OrganisationFailedToRepayFunds = "3110";
            public static string OrganisationContractTermination = "3120";
        }

        public static class ProtectingYourApprentices
        {
            public const string ContinuityPlan = "4010";
            public const string EqualityAndDiversityPolicy = "4020";
            public const string SafeguardingPolicy = "4030";
            public const string SafeguardingOverallResponsibility = "4035";
            public const string SafeguardingPolicyIncludesPreventDutyPolicy = "4037";
            public const string PreventDutyPolicy = "4038";
            public const string HealthAndSafetyPolicy = "4040";
            public const string HealthAndSafetyOverallResponsibility = "4050";
            public const string ActingAsASubcontractor = "4060";
        }

        public static class ReadinessToEngage
        {
            public const string EngagedWithEmployers = "5100";
            public const string RelationshipWithEmployers = "5110";
            public const string RelationshipWithEmployersResponsible = "5120";
            public const string PromoteApprenticeshipsToEmployers = "5130";
            public const string ComplaintsPolicy = "5200";
            public const string ComplaintsPolicyWebsite = "5210";
            public const string ContractForServicesTemplate = "5300";
            public const string CommitmentStatementTemplate = "5400";
            public const string PriorLearningAssessment = "5500";
            public const string PriorLearningQualifications = "5510";
            public const string SubcontractorsUse = "5600";
            public const string SubcontractorsDueDiligence = "5610";
        }

        public static class PlanningApprenticeshipTraining
        {
            public const string TypeOfApprenticeshipTraining_Main = "6020";
            public const string TypeOfApprenticeshipTraining_Employer = "6022";
            public const string TypeOfApprenticeshipTraining_Supporting = "6023";
            public const string ApplicationStandards = "6030";
            public const string ApplicationFrameworks_MainEmployer = "6050";
            public const string ApplicationFrameworks_Supporting = "6060";
            public const string OrganisationTransition_MainEmployer = "6052";
            public const string OrganisationTransition_Supporting = "6062";
            public const string OnlyDeliveringApprenticeshipFrameworks_MainEmployer = "6054";
            public const string OnlyDeliveringApprenticeshipFrameworks_Supporting = "6064";
            public const string ReadyToDeliverTraining = "6200";
            public const string ReadyToDeliverTrainingResponsible = "6700";
            public const string EngagingWithAwardingBodies = "6800";
            public const string EngagingWithAssessmentOrganisations = "6900";
            public const string EnsureApprenticesAreSupported = "6300";
            public const string EnsureApprenticesAreSupportedHow = "6310";
            public const string EnsureApprenticesAreSupportedOtherWays = "6320";
            public const string ForecastingStarts = "6400";
            public const string ReadyToDeliverAgainstStarts = "6410";
            public const string RecruitNewStaff = "6420";
            public const string RatioOfStaffToApprentices = "6430";
            public const string OnTheJobTrainingTeachingMethods = "6500";
            public const string OnTheJobTrainingTeachingRelevance = "6510";
            public const string AddressWhereApprenticesWillBeTrained = "6600";
        }

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

        public static class EvaluatingApprenticeshipTraining
        {
            public const string QualityProcessEvaluating = "8100";
            public const string QualityProcessImprovements = "8110";
            public const string QualityProcessIncludesApprenticeshipTraining = "8200";
            public const string QualityProcessQuality_MainEmployer = "8210";
            public const string QualityProcessQuality_Supporting = "8220";
            public const string QualityProcessReviewing = "8230";
            public const string CollectApprenticeshipData = "8300";
            public const string IndividualisedLearnerRecordData = "8310";
        }

        public static class Finish
        {
            public static string ApplicationPermissionsChecksShutterPage = "10005";
            public static string TermsConditionsCOAPart2ShutterPage = "10006";
            public static string TermsConditionsCOAPart3ShutterPage = "10007";
        }
    }

    public static class RoatpWorkflowQuestionTags
    {
        public static string ProviderRoute = "ApplyProviderRoute";
        public static string UKPRN = "UKPRN";
        public static string UkrlpLegalName = "UKRLPLegalName";
        public static string UkrlpVerificationCompany = "UKRLPVerificationCompany";
        public static string CompaniesHouseDirectors = "CompaniesHouseDirectors";
        public static string CompaniesHousePscs = "CompaniesHousePSCs";
        public static string ManualEntryRequiredCompaniesHouse = "CHManualEntryRequired";
        public static string UkrlpVerificationCharity = "UKRLPVerificationCharity";
        public static string CharityCommissionTrustees = "CharityTrustees";
        public static string ManualEntryRequiredCharityCommission = "CCTrusteeManualEntry";
        public static string UkrlpVerificationSoleTraderPartnership = "UKRLPVerificationSoleTraderPartnership";
        public static string SoleTraderOrPartnership = "SoleTradeOrPartnership";
        public static string PartnershipType = "PartnershipType";
        public static string AddPartners = "AddPartners";
        public static string SoleTradeDob = "AddSoleTradeDOB";
        public static string AddPeopleInControl = "AddPeopleInControl";
        public static string FinishPermissionPersonalDetails = "FinishPermissionPersonalDetails";
        public static string FinishAccuratePersonalDetails = "FinishAccuratePersonalDetails";
        public static string FinishPermissionSubmitApplication = "FinishPermissionSubmitApp";
        public static string FinishCommercialInConfidence = "FinishCommercialConfidence";
        public static string FinishCOA2MainEmployer = "COAPart2MainEmployer";
        public static string FinishCOA2Supporting = "COAPart2Supporting";
        public static string FinishCOA3MainEmployer = "COAPart3MainEmployer";
        public static string FinishCOA3Supporting = "COAPart3Supporting";
        public static string AddManagementHierarchy = "AddManagementHierarchy";
        public static string UKRLPPrimaryVerificationSource = "UKRLPPrimaryVerificationSource";
        public static string UKRLPVerificationCompanyNumber = "UKRLPVerificationCompanyNumber";
        public static string UKRLPVerificationCharityRegNumber = "UKRLPVerificationCharityRegNumber";
    }

    public static class RoatpPreambleQuestionBuilder
    {

        public static List<PreambleAnswer> CreatePreambleQuestions(ApplicationDetails applicationDetails)
        {
            var questions = new List<PreambleAnswer>();

            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UKPRN,
                Value = applicationDetails.UKPRN.ToString()
            });

            CreateUkrlpQuestionAnswers(applicationDetails, questions);

            CreateCompaniesHouseQuestionAnswers(applicationDetails, questions);

            CreateCharityCommissionQuestionAnswers(applicationDetails, questions);

            CreateRoatpQuestionAnswers(applicationDetails, questions);

            CreateApplyQuestionAnswers(applicationDetails, questions);
            CreateLevyEmployerQuestionAnswers(applicationDetails, questions);

            return questions;
        }
        
        public static List<PreambleAnswer> CreateCompaniesHouseWhosInControlQuestions(ApplicationDetails applicationDetails)
        {
            var questions = new List<PreambleAnswer>();

            CreateCompaniesHouseDirectorsData(applicationDetails, questions);
            CreateCompaniesHousePscData(applicationDetails, questions);
            CreateBlankCompaniesHouseConfirmationQuestion(questions);
            return questions;
        }

        public static List<PreambleAnswer> CreateCharityCommissionWhosInControlQuestions(ApplicationDetails applicationDetails)
        {
            var questions = new List<PreambleAnswer>();
            CreateCharityTrusteeData(applicationDetails, questions);
            CreateBlankCharityCommissionConfirmationQuestion(questions);

            return questions;
        }

        private static void CreateApplyQuestionAnswers(ApplicationDetails applicationDetails, List<PreambleAnswer> questions)
        {
            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.ApplyProviderRoute,
                Value = applicationDetails.ApplicationRoute?.Id.ToString(),
                PageId = RoatpWorkflowPageIds.ProviderRoute,
                SequenceId = RoatpWorkflowSequenceIds.Preamble,
                SectionId = RoatpWorkflowSectionIds.Preamble
            });

            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.COAStage1Application,
                Value = "TRUE",
                PageId = RoatpWorkflowPageIds.ConditionsOfAcceptance,
                SequenceId = RoatpWorkflowSequenceIds.ConditionsOfAcceptance,
                SectionId = RoatpWorkflowSectionIds.ConditionsOfAcceptance
            });
        }

        private static void CreateUkrlpQuestionAnswers(ApplicationDetails applicationDetails, List<PreambleAnswer> questions)
        {
            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalName,
                Value = applicationDetails.UkrlpLookupDetails?.ProviderName
            });

            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpWebsite,
                Value = applicationDetails.UkrlpLookupDetails?.PrimaryContactDetails?.ContactWebsiteAddress
            });

            var ukrlpNoWebsiteAddress = string.Empty;
            if (String.IsNullOrWhiteSpace(applicationDetails.UkrlpLookupDetails?.PrimaryContactDetails
                ?.ContactWebsiteAddress))
            {
                ukrlpNoWebsiteAddress = "TRUE";
            }

            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UKrlpNoWebsite,
                Value = ukrlpNoWebsiteAddress
            });

            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine1,
                Value = applicationDetails.UkrlpLookupDetails?.PrimaryContactDetails?.ContactAddress?.Address1
            });

            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine2,
                Value = applicationDetails.UkrlpLookupDetails?.PrimaryContactDetails?.ContactAddress?.Address2
            });

            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine3,
                Value = applicationDetails.UkrlpLookupDetails?.PrimaryContactDetails?.ContactAddress?.Address3
            });

            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine4,
                Value = applicationDetails.UkrlpLookupDetails?.PrimaryContactDetails?.ContactAddress?.Address4
            });

            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressTown,
                Value = applicationDetails.UkrlpLookupDetails?.PrimaryContactDetails?.ContactAddress?.Town
            });

            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressPostcode,
                Value = applicationDetails.UkrlpLookupDetails?.PrimaryContactDetails?.ContactAddress?.PostCode
            });

            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpTradingName,
                Value = applicationDetails.UkrlpLookupDetails?.ProviderAliases?[0].Alias
            });

            var companiesHouseVerification = applicationDetails.UkrlpLookupDetails?.VerificationDetails?.FirstOrDefault(x => x.VerificationAuthority == VerificationAuthorities.CompaniesHouseAuthority);

            if (companiesHouseVerification != null)
            {
                questions.Add(new PreambleAnswer
                {
                    QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany,
                    Value = "TRUE"
                });
                
                questions.Add(new PreambleAnswer
                {
                    QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompanyNumber,
                    Value = companiesHouseVerification.VerificationId
                });
            }
            else
            {
                questions.Add(new PreambleAnswer
                {
                    QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompanyNumber,
                    Value = string.Empty
                });

                questions.Add(new PreambleAnswer
                {
                    QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany,
                    Value = string.Empty
                });
            }

            if (applicationDetails.UkrlpLookupDetails?.VerificationDetails?.FirstOrDefault(x => x.VerificationAuthority == VerificationAuthorities.CharityCommissionAuthority) != null)
            {
                questions.Add(new PreambleAnswer
                {
                    QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                    Value = "TRUE"
                });
            }
            else
            {
                questions.Add(new PreambleAnswer
                {
                    QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                    Value = string.Empty
                });
            }

            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharityRegNumber,
                Value = applicationDetails.UkrlpLookupDetails?.VerificationDetails?.FirstOrDefault(x => x.VerificationAuthority == VerificationAuthorities.CharityCommissionAuthority)?.VerificationId
            });

            var soleTraderPartnershipVerification = string.Empty;
            if (applicationDetails.UkrlpLookupDetails?.VerificationDetails?.FirstOrDefault(x =>
                    x.VerificationAuthority == VerificationAuthorities.SoleTraderPartnershipAuthority) != null)
            {
                soleTraderPartnershipVerification = "TRUE";
            }           

            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationSoleTraderPartnership,
                Value = soleTraderPartnershipVerification
            });

            var primaryVerificationSource = applicationDetails.UkrlpLookupDetails?.VerificationDetails?.FirstOrDefault(x => x.PrimaryVerificationSource == true);
            if (primaryVerificationSource != null)
            {
                questions.Add(new PreambleAnswer
                {
                    QuestionId = RoatpPreambleQuestionIdConstants.UkrlpPrimaryVerificationSource,
                    Value = primaryVerificationSource.VerificationAuthority
                });
            }
        }
        
        private static void CreateCompaniesHouseQuestionAnswers(ApplicationDetails applicationDetails, List<PreambleAnswer> questions)
        {
            var manualEntryRequired = string.Empty;
            if (applicationDetails.CompanySummary != null && applicationDetails.CompanySummary.ManualEntryRequired)
            {
                manualEntryRequired = applicationDetails.CompanySummary.ManualEntryRequired.ToString().ToUpper();
            }

            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseManualEntryRequired,
                Value = manualEntryRequired
            });

            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseCompanyName,
                Value = applicationDetails.CompanySummary?.CompanyName
            });

            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseCompanyType,
                Value = applicationDetails.CompanySummary?.CompanyTypeDescription
            });

            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseCompanyStatus,
                Value = applicationDetails.CompanySummary?.Status
            });

            var incorporationDate = string.Empty;
            if (applicationDetails.CompanySummary != null && applicationDetails.CompanySummary.IncorporationDate.HasValue)
            {
                incorporationDate = applicationDetails.CompanySummary.IncorporationDate.Value.ToString();
            }

            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseIncorporationDate,
                Value = incorporationDate
            });
        }

        private static void CreateCompaniesHouseDirectorsData(ApplicationDetails applicationDetails, List<PreambleAnswer> questions)
        {
            if (applicationDetails.CompanySummary.Directors != null && applicationDetails.CompanySummary.Directors.Count > 0)
            {
                var table = new TabularData
                {
                    Caption = "Company directors",
                    HeadingTitles = new List<string> { "Name", "Date of birth" },
                    DataRows = new List<TabularDataRow>()
                };

                foreach (var director in applicationDetails.CompanySummary.Directors)
                {
                    var dataRow = new TabularDataRow
                    {
                        Id = director.Id,
                        Columns = new List<string> { director.Name, FormatDateOfBirth(director.DateOfBirth) }
                    };
                    table.DataRows.Add(dataRow);
                }

                questions.Add(new PreambleAnswer
                {
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.CompaniesHouseDirectors,
                    Value = JsonConvert.SerializeObject(table),
                    PageId = RoatpWorkflowPageIds.WhosInControl.CompaniesHouseStartPage,
                    SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                    SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhosInControl
                });
            }
            else
            {
                questions.Add(new PreambleAnswer
                {
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.CompaniesHouseDirectors,
                    Value = string.Empty,
                    PageId = RoatpWorkflowPageIds.WhosInControl.CompaniesHouseStartPage,
                    SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                    SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhosInControl
                });
            }
        }

        private static void CreateCompaniesHousePscData(ApplicationDetails applicationDetails, List<PreambleAnswer> questions)
        {
            if (applicationDetails.CompanySummary.PersonsWithSignificantControl != null && applicationDetails.CompanySummary.PersonsWithSignificantControl.Count > 0)
            {
                var table = new TabularData
                {
                    Caption = "People with significant control (PSCs)",
                    HeadingTitles = new List<string> { "Name", "Date of birth" },
                    DataRows = new List<TabularDataRow>()
                };

                foreach (var person in applicationDetails.CompanySummary.PersonsWithSignificantControl)
                {
                    var dataRow = new TabularDataRow
                    {
                        Id = person.Id,
                        Columns = new List<string> { person.Name, FormatDateOfBirth(person.DateOfBirth) }
                    };
                    table.DataRows.Add(dataRow);
                }

                questions.Add(new PreambleAnswer
                {
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.CompaniesHousePSCs,
                    Value = JsonConvert.SerializeObject(table),
                    PageId = RoatpWorkflowPageIds.WhosInControl.CompaniesHouseStartPage,
                    SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                    SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhosInControl
                });
            }
            else
            {
                questions.Add(new PreambleAnswer
                {
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.CompaniesHousePSCs,
                    Value = string.Empty,
                    PageId = RoatpWorkflowPageIds.WhosInControl.CompaniesHouseStartPage,
                    SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                    SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhosInControl
                });
            }
        }

        private static void CreateCharityTrusteeData(ApplicationDetails applicationDetails, List<PreambleAnswer> questions)
        {
            if (applicationDetails.CharitySummary.Trustees != null && applicationDetails.CharitySummary.Trustees.Count > 0)
            {
                var table = new TabularData
                {
                    HeadingTitles = new List<string> { "Name" },
                    DataRows = new List<TabularDataRow>()
                };

                foreach (var trustee in applicationDetails.CharitySummary.Trustees)
                {
                    var dataRow = new TabularDataRow
                    {
                        Id = trustee.Id,
                        Columns = new List<string> { trustee.Name }
                    };
                    table.DataRows.Add(dataRow);
                }

                questions.Add(new PreambleAnswer
                {
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.CharityCommissionTrustees,
                    Value = JsonConvert.SerializeObject(table),
                    PageId = RoatpWorkflowPageIds.WhosInControl.CharityCommissionStartPage,
                    SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                    SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhosInControl
                });
            }
            else
            {
                questions.Add(new PreambleAnswer
                {
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.CharityCommissionTrustees,
                    Value = string.Empty,
                    PageId = RoatpWorkflowPageIds.WhosInControl.CharityCommissionStartPage,
                    SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                    SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhosInControl
                });
            }
        }

        private static string FormatDateOfBirth(DateTime? dateOfBirth)
        {
            if (!dateOfBirth.HasValue)
            {
                return string.Empty;
            }

            if (dateOfBirth.Value.Year == 1 && dateOfBirth.Value.Month == 1)
            {
                return string.Empty;
            }

            return dateOfBirth.Value.ToString("MMM yyyy");
        }
        
        private static void CreateCharityCommissionQuestionAnswers(ApplicationDetails applicationDetails, List<PreambleAnswer> questions)
        {
            var trusteeManualEntryRequired = string.Empty;
            if (applicationDetails.CharitySummary != null && applicationDetails.CharitySummary.TrusteeManualEntryRequired)
            {
                trusteeManualEntryRequired = applicationDetails.CharitySummary.TrusteeManualEntryRequired.ToString().ToUpper();
            }

            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CharityCommissionTrusteeManualEntry,
                Value = trusteeManualEntryRequired
            });

            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CharityCommissionCharityName,
                Value = applicationDetails.CharitySummary?.CharityName
            });

            var incorporationDate = string.Empty;
            if (applicationDetails.CharitySummary != null && applicationDetails.CharitySummary.IncorporatedOn.HasValue)
            {
                incorporationDate = applicationDetails.CharitySummary.IncorporatedOn.Value.ToString();
            }
            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CharityCommissionRegistrationDate,
                Value = incorporationDate
            });
        }
        
        private static void CreateBlankCompaniesHouseConfirmationQuestion(List<PreambleAnswer> questions)
        {
            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpYourOrganisationQuestionIdConstants.CompaniesHouseDetailsConfirmed,
                Value = string.Empty,
                PageId = RoatpWorkflowPageIds.WhosInControl.CompaniesHouseStartPage,
                SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhosInControl
            });
        }

        private static void CreateBlankCharityCommissionConfirmationQuestion(List<PreambleAnswer> questions)
        {
            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpYourOrganisationQuestionIdConstants.CharityCommissionDetailsConfirmed,
                Value = string.Empty,
                PageId = RoatpWorkflowPageIds.WhosInControl.CharityCommissionConfirmTrustees,
                SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhosInControl
            });
        }

        private static void CreateRoatpQuestionAnswers(ApplicationDetails applicationDetails, List<PreambleAnswer> questions)
        {
            var onRoatpRegister = string.Empty;
            if (applicationDetails.RoatpRegisterStatus != null &&
                applicationDetails.RoatpRegisterStatus.UkprnOnRegister)
            {
                onRoatpRegister = "TRUE";
            }
            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.OnRoatp,
                Value = onRoatpRegister
            });

            string registerStatus = string.Empty;
            if (onRoatpRegister == "TRUE")
            {
                registerStatus = applicationDetails.RoatpRegisterStatus.StatusId.ToString();
            }

            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.RoatpCurrentStatus,
                Value = registerStatus
            });

            string providerRoute = string.Empty;
            if (onRoatpRegister == "TRUE")
            {
                providerRoute = applicationDetails.RoatpRegisterStatus.ProviderTypeId.ToString();
            }

            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.RoatpProviderRoute,
                Value = providerRoute
            });

            if (onRoatpRegister == "TRUE" && applicationDetails.RoatpRegisterStatus.StatusId == OrganisationStatus.Removed)
            {
                questions.Add(new PreambleAnswer
                {
                    QuestionId = RoatpPreambleQuestionIdConstants.RoatpRemovedReason,
                    Value = applicationDetails.RoatpRegisterStatus.RemovedReasonId.ToString()
                });
            }
            else
            {
                questions.Add(new PreambleAnswer
                {
                    QuestionId = RoatpPreambleQuestionIdConstants.RoatpRemovedReason,
                    Value = string.Empty
                });
            }

            if (onRoatpRegister == "TRUE" && applicationDetails.RoatpRegisterStatus.StatusDate.HasValue)
            {
                questions.Add(new PreambleAnswer
                {
                    QuestionId = RoatpPreambleQuestionIdConstants.RoatpStatusDate,
                    Value = applicationDetails.RoatpRegisterStatus.StatusDate.ToString()
                });
            }
            else
            {
                questions.Add(new PreambleAnswer
                {
                    QuestionId = RoatpPreambleQuestionIdConstants.RoatpStatusDate,
                    Value = string.Empty
                });
            }

        }

        private static void CreateLevyEmployerQuestionAnswers(ApplicationDetails applicationDetails, List<PreambleAnswer> questions)
        {
            var levyPayingEmployer = string.Empty;
            if (applicationDetails.LevyPayingEmployer == "Y")
            {
                levyPayingEmployer = "TRUE";
            }
            questions.Add(new PreambleAnswer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.LevyPayingEmployer,
                Value = levyPayingEmployer
            });
        }
    }

}
