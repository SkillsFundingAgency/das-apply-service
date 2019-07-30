
namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Apply;
    using Domain.Roatp;
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
        public static string CharityCommissionTrusteeManualEntry = "PRE-60";
        public static string UkrlpVerificationCharityRegNumber = "PRE-61";
        public static string CharityCommissionCharityName = "PRE-62";
        public static string CharityCommissionRegistrationDate = "PRE-64";
        public static string UkrlpVerificationSoleTraderPartnership = "PRE-70";
        public static string UkrlpPrimaryVerificationSource = "PRE-80";
        public static string OnRoatp = "PRE-90";
        public static string RoatpCurrentStatus = "PRE-91";
        public static string RoatpRemovedReason = "PRE-92";
        public static string RoatpStatusDate = "PRE-93";
        public static string RoatpProviderRoute = "PRE-94";     
        public static string ApplyProviderRouteMain = "YO-1.1";              
        public static string ApplyProviderRouteEmployer = "YO-1.2";
        public static string ApplyProviderRouteSupporting = "YO-1.3";
        public static string COAStage1Application = "COA-1";                  
    }

    public static class RoatpPreambleQuestionBuilder
    {
        public static List<Answer> CreatePreambleQuestions(ApplicationDetails applicationDetails)
        {
            var questions = new List<Answer>();

            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UKPRN,
                Value = applicationDetails.UKPRN.ToString()
            });

            CreateUkrlpQuestionAnswers(applicationDetails, questions);

            CreateCompaniesHouseQuestionAnswers(applicationDetails, questions);

            CreateCharityCommissionQuestionAnswers(applicationDetails, questions);

            CreateRoatpQuestionAnswers(applicationDetails, questions);

            CreateApplyQuestionAnswers(applicationDetails, questions);

            return questions;
        }

        private static void CreateApplyQuestionAnswers(ApplicationDetails applicationDetails, List<Answer> questions)
        {
            switch (applicationDetails?.ApplicationRoute?.Id)
            {
                case ApplicationRoute.MainProviderApplicationRoute:
                {
                    questions.Add(new Answer
                    {
                        QuestionId = RoatpPreambleQuestionIdConstants.ApplyProviderRouteMain,
                        Value = "TRUE"
                    });
                    break;
                }
                case ApplicationRoute.EmployerProviderApplicationRoute:
                {
                    questions.Add(new Answer
                    {
                        QuestionId = RoatpPreambleQuestionIdConstants.ApplyProviderRouteEmployer,
                        Value = "TRUE"
                    });
                    break;
                }
                case ApplicationRoute.SupportingProviderApplicationRoute:
                {
                    questions.Add(new Answer
                    {
                        QuestionId = RoatpPreambleQuestionIdConstants.ApplyProviderRouteSupporting,
                        Value = "TRUE"
                    });
                    break;
                }
            }

            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.COAStage1Application,
                Value = "TRUE"
            });
        }

        private static void CreateUkrlpQuestionAnswers(ApplicationDetails applicationDetails, List<Answer> questions)
        {
            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalName,
                Value = applicationDetails.UkrlpLookupDetails?.ProviderName
            });

            questions.Add(new Answer
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

            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UKrlpNoWebsite,
                Value = ukrlpNoWebsiteAddress
            });

            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine1,
                Value = applicationDetails.UkrlpLookupDetails?.PrimaryContactDetails?.ContactAddress?.Address1
            });

            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine2,
                Value = applicationDetails.UkrlpLookupDetails?.PrimaryContactDetails?.ContactAddress?.Address2
            });

            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine3,
                Value = applicationDetails.UkrlpLookupDetails?.PrimaryContactDetails?.ContactAddress?.Address3
            });

            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine4,
                Value = applicationDetails.UkrlpLookupDetails?.PrimaryContactDetails?.ContactAddress?.Address4
            });

            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressTown,
                Value = applicationDetails.UkrlpLookupDetails?.PrimaryContactDetails?.ContactAddress?.Town
            });

            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalAddressPostcode,
                Value = applicationDetails.UkrlpLookupDetails?.PrimaryContactDetails?.ContactAddress?.PostCode
            });

            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpTradingName,
                Value = applicationDetails.UkrlpLookupDetails?.ProviderAliases?[0].Alias
            });

            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompanyNumber,
                Value = applicationDetails.UkrlpLookupDetails?.VerificationDetails?.FirstOrDefault(x => x.VerificationAuthority == VerificationAuthorities.CompaniesHouseAuthority)?.VerificationId
            });
            
            questions.Add(new Answer
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

            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationSoleTraderPartnership,
                Value = soleTraderPartnershipVerification
            });

            var primaryVerificationSource = applicationDetails.UkrlpLookupDetails?.VerificationDetails?.FirstOrDefault(x => x.PrimaryVerificationSource == true);
            if (primaryVerificationSource != null)
            {
                questions.Add(new Answer
                {
                    QuestionId = RoatpPreambleQuestionIdConstants.UkrlpPrimaryVerificationSource,
                    Value = primaryVerificationSource.VerificationAuthority
                });
            }
        }
        
        private static void CreateCompaniesHouseQuestionAnswers(ApplicationDetails applicationDetails, List<Answer> questions)
        {
            var manualEntryRequired = string.Empty;
            if (applicationDetails.CompanySummary != null && applicationDetails.CompanySummary.ManualEntryRequired)
            {
                manualEntryRequired = applicationDetails.CompanySummary.ManualEntryRequired.ToString().ToUpper();
            }

            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseManualEntryRequired,
                Value = manualEntryRequired
            });

            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseCompanyName,
                Value = applicationDetails.CompanySummary?.CompanyName
            });

            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseCompanyType,
                Value = applicationDetails.CompanySummary?.CompanyTypeDescription
            });

            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseCompanyStatus,
                Value = applicationDetails.CompanySummary?.Status
            });

            var incorporationDate = string.Empty;
            if (applicationDetails.CompanySummary != null && applicationDetails.CompanySummary.IncorporationDate.HasValue)
            {
                incorporationDate = applicationDetails.CompanySummary.IncorporationDate.Value.ToString();
            }

            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseIncorporationDate,
                Value = incorporationDate
            });
        }

        private static void CreateCharityCommissionQuestionAnswers(ApplicationDetails applicationDetails, List<Answer> questions)
        {
            var trusteeManualEntryRequired = string.Empty;
            if (applicationDetails.CharitySummary != null && applicationDetails.CharitySummary.TrusteeManualEntryRequired)
            {
                trusteeManualEntryRequired = applicationDetails.CharitySummary.TrusteeManualEntryRequired.ToString().ToUpper();
            }

            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CharityCommissionTrusteeManualEntry,
                Value = trusteeManualEntryRequired
            });

            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CharityCommissionCharityName,
                Value = applicationDetails.CharitySummary?.CharityName
            });

            var incorporationDate = string.Empty;
            if (applicationDetails.CharitySummary != null && applicationDetails.CharitySummary.IncorporatedOn.HasValue)
            {
                incorporationDate = applicationDetails.CharitySummary.IncorporatedOn.Value.ToString();
            }
            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CharityCommissionRegistrationDate,
                Value = incorporationDate
            });
        }

        private static void CreateRoatpQuestionAnswers(ApplicationDetails applicationDetails, List<Answer> questions)
        {
            var onRoatpRegister = string.Empty;
            if (applicationDetails.RoatpRegisterStatus != null &&
                applicationDetails.RoatpRegisterStatus.UkprnOnRegister)
            {
                onRoatpRegister = "TRUE";
            }
            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.OnRoatp,
                Value = onRoatpRegister
            });

            if (onRoatpRegister == string.Empty)
            {
                return;
            }

            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.RoatpCurrentStatus,
                Value = applicationDetails.RoatpRegisterStatus.StatusId.ToString()
            });

            questions.Add(new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.RoatpProviderRoute,
                Value = applicationDetails.RoatpRegisterStatus.ProviderTypeId.ToString()
            });

            if (applicationDetails.RoatpRegisterStatus.StatusId == OrganisationStatus.Removed)
            {
                questions.Add(new Answer
                {
                    QuestionId = RoatpPreambleQuestionIdConstants.RoatpRemovedReason,
                    Value = applicationDetails.RoatpRegisterStatus.RemovedReasonId.ToString()
                });
            }

            if (applicationDetails.RoatpRegisterStatus.StatusDate.HasValue)
            {
                questions.Add(new Answer
                {
                    QuestionId = RoatpPreambleQuestionIdConstants.RoatpStatusDate,
                    Value = applicationDetails.RoatpRegisterStatus.StatusDate.ToString()
                });
            }
           
        }
    }

}
