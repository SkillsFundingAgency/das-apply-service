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

    public static class RoatpYourOrganisationQuestionIdConstants
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
        public static string OrganisationTypeMainSupporting = "YO-140";
        public static string OrganisationTypeEmployer = "YO-150";
        public static string EducationalInstituteTypeMainSupporting = "YO-160";
        public static string EducationalInstituteTypeEmployer = "YO-161";
        public static string PublicBodyTypeMainSupporting = "YO-170";
        public static string PublicBodyTypeEmployer = "YO-171";
        public static string SchoolMainSupporting = "YO-180";
        public static string SchoolEmployer = "YO-181";
        public static string RegisteredESFAMainSupporting = "YO-200";
        public static string RegisteredESFAEmployer = "YO-201";
        public static string FundedESFAMainSupporting = "YO-210";
        public static string FundedESFAEmployer = "YO-211";
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

    public static class RoatpCriminalComplianceChecksQuestionIdConstants
    {
        public static string CompositionCreditors = "CC-20";
        public static string OrganisationFailedToRepayFunds = "CC-21";
        public static string OrganisationContractTermination = "CC-22";
    }

    public static class RoatpDeliveringApprenticeshipTrainingQuestionIdConstants
    {
        public const string ManagementHierarchy = "DAT-720";
        public const string OverallManagerResponsible_MainEmployer = "DAT-754.1";
        public const string OverallManagerExperience_MainEmployer = "DAT-754.2";
        public const string OverallManagerResponsible_Supporting = "DAT-759.1";
        public const string OverallManagerExperience_Supporting = "DAT-759.2";
    }

    public static class RoatpPlanningApprenticeshipTrainingQuestionIdConstants
    {
        public const string ApplicationFrameworks_MainEmployer = "PAT-50";
        public const string ApplicationFrameworks_Supporting = "PAT-60";
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

    public static partial class RoatpWorkflowPageIds
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
            public static string PublicBodyTypeMainSupporting = "170";
            public static string PublicBodyTypeEmployer = "171";
            public static string EducationalInstituteTypeMainSupporting = "160";
            public static string EducationalInstituteTypeEmployer = "161";
            public static string SchoolMainSupporting = "180";
            public static string SchoolEmployer = "181";
            public static string RegisteredESFAMainSupporting = "200";
            public static string RegisteredESFAEmployer = "201";
            public static string FundedESFAMainSupporting = "210";
            public static string FundedESFAEmployer = "211";
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
            public const string HealthAndSafetyOverallResponsibility = "4045";
            public const string ActingAsASubcontractor = "4050";
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
