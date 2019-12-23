
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
    }

    public static class RoatpWorkflowSequenceIds
    {
        public static int Preamble = 0;
        public static int YourOrganisation = 1;
        public static int FinancialEvidence = 2;
        public static int CriminalComplianceChecks = 3;
        public static int ProtectingYourApprentices = 4;
        public static int ReadinessToEngage = 5;
        public static int PlanningApprenticeshipTraining = 6;
        public static int DeliveringApprenticeshipTraining = 7;
        public static int EvaluatingApprenticeshipTraining = 8;
        public static int Finish = 9;
        public static int ConditionsOfAcceptance = 99;
    }

    public static class RoatpWorkflowSectionIds
    {
        public static int Preamble = 1;
        public static int ConditionsOfAcceptance = 1;

        public static class YourOrganisation
        {
            public static int ProviderRoute = 1;
            public static int WhatYouWillNeed = 2;
            public static int OrganisationDetails = 3;
            public static int WhosInControl = 4;
            public static int DescribeYourOrganisation = 5;
            public static int ExperienceAndAccreditations = 6;
        }

        public static class FinancialEvidence
        {
            public static int WhatYouWillNeed = 1;
            public static int YourOrganisationsFinancialEvidence = 2;
        }

        public static class CriminalComplianceChecks
        {
            public static int WhatYouWillNeed = 1;
            public static int ChecksOnYourOrganisation = 2;
            public static int CheckOnWhosInControl = 3;
        }

        public static class Finish
        {
            public static int ApplicationPermissionsAndChecks = 1;
            public static int CommercialInConfidenceInformation = 2;
            public static int TermsAndConditions = 3;
            public static int SubmitApplication = 4;
        }
    }

    public static class RoatpWorkflowPageIds
    {
        public static string Preamble = "1";
        public static string YourOrganisation = "2";
        public static string YourOrganisationIntroductionMain = "10";
        public static string YourOrganisationIntroductionEmployer = "11";
        public static string YourOrganisationIntroductionSupporting = "12";
        public static string YourOrganisationParentCompanyCheck = "20";
        public static string YourOrganisationParentCompanyDetails = "21";
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

        public class DescribeYourOrganisation
        {
            public static string MainSupportingStartPage = "140";
            public static string EmployerStartPage = "150";
        }

        public class ExperienceAndAccreditations
        { 
            public static string StartPage = "235";
        }

        public class Finish
        {
            public static string ApplicationPermissionsChecksShutterPage = "10005";
            public static string TermsConditionsCOAPart2ShutterPage = "10006";
            public static string TermsConditionsCOAPart3ShutterPage = "10007";
        }
    }

    public static class RoatpWorkflowQuestionTags
    {
        public static string ProviderRoute = "Apply-ProviderRoute";
        public static string UKPRN = "UKPRN";
        public static string UkrlpLegalName = "UKRLP-LegalName";
        public static string UkrlpVerificationCompany = "UKRLP-Verification-Company";
        public static string CompaniesHouseDirectors = "Companies-House-Directors";
        public static string CompaniesHousePscs = "Companies-House-PSCs";
        public static string ManualEntryRequiredCompaniesHouse = "CH-ManualEntryRequired";
        public static string UkrlpVerificationCharity = "UKRLP-Verification-Charity";
        public static string CharityCommissionTrustees = "Charity-Trustees";
        public static string ManualEntryRequiredCharityCommission = "CC-TrusteeManualEntry";
        public static string UkrlpVerificationSoleTraderPartnership = "UKRLP-Verification-SoleTraderPartnership";
        public static string SoleTraderOrPartnership = "SoleTrade-or-Partnership";
        public static string PartnershipType = "Partnership-Type";
        public static string AddPartners = "Add-Partners";
        public static string SoleTradeDob = "Add-SoleTrade-DOB";
        public static string AddPeopleInControl = "Add-PeopleInControl";
        public static string FinishPermissionPersonalDetails = "Finish-Permission-Personal-Details";
        public static string FinishAccuratePersonalDetails = "Finish-Accurate-Personal-Details";
        public static string FinishPermissionSubmitApplication = "Finish-Permission-SubmitApp";
        public static string FinishCommercialInConfidence = "Finish-Commercial-Confidence";
        public static string FinishCOA2MainEmployer = "COA-Part2-MainEmployer";
        public static string FinishCOA2Supporting = "COA-Part2-Supporting";
        public static string FinishCOA3MainEmployer = "COA-Part3-MainEmployer";
        public static string FinishCOA3Supporting = "COA-Part3-Supporting";
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
                PageId = RoatpWorkflowPageIds.YourOrganisation,
                SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                SectionId = RoatpWorkflowSectionIds.YourOrganisation.ProviderRoute
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
            if (applicationDetails.CompanySummary.PersonsSignificationControl != null && applicationDetails.CompanySummary.PersonsSignificationControl.Count > 0)
            {
                var table = new TabularData
                {
                    Caption = "People with significant control (PSCs)",
                    HeadingTitles = new List<string> { "Name", "Date of birth" },
                    DataRows = new List<TabularDataRow>()
                };

                foreach (var person in applicationDetails.CompanySummary.PersonsSignificationControl)
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
