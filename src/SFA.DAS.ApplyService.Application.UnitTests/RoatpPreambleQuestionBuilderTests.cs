
namespace SFA.DAS.ApplyService.Application.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apply.Roatp;
    using Domain.CharityCommission;
    using Domain.CompaniesHouse;
    using Domain.Roatp;
    using Domain.Ukrlp;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class RoatpPreambleQuestionBuilderTests
    {
        private ApplicationDetails _applicationDetails;

        [SetUp]
        public void Before_each_test()
        {
            _applicationDetails = new ApplicationDetails
            {
                UKPRN = 10001234,
                UkrlpLookupDetails = new ProviderDetails
                {
                    ProviderName = "Provider Legal Name",
                    ContactDetails = new List<ProviderContact>
                    {
                        new ProviderContact
                        {
                            ContactType = "L",
                            ContactWebsiteAddress = "www.myweb.com",
                            ContactAddress = new ContactAddress
                            {
                                Address1 = "Address line 1",
                                Address2 = "Address line 2",
                                Address3 = "Address line 3",
                                Address4 = "Address line 4",
                                Town = "Town",
                                PostCode = "TS1 1ST"
                            }
                        }
                    },
                    ProviderAliases = new List<ProviderAlias>
                    {
                        new ProviderAlias { Alias = "Provider alias" }
                    }
                },
                CompanySummary = new CompaniesHouseSummary()
            };
        }

        [Test]
        public void Preamble_questions_contains_TwoInTwelveMonths()
        {
            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.TwoInTwelveMonths);
            question.Should().NotBeNull();
            question.Value.Should().Be("NO");
        }

        [Test]
        public void Preamble_questions_contains_UKPRN()
        {
            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UKPRN);
            question.Should().NotBeNull();
            question.Value.Should().Be(_applicationDetails.UKPRN.ToString());
        }

        [Test]
        public void Preamble_questions_contains_legal_name()
        {
            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalName);
            question.Should().NotBeNull();
            question.Value.Should().Be(_applicationDetails.UkrlpLookupDetails.ProviderName);
        }

        [Test]
        public void Preamble_questions_contains_web_site()
        {
            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpWebsite);
            question.Should().NotBeNull();
            question.Value.Should().Be(_applicationDetails.UkrlpLookupDetails.PrimaryContactDetails.ContactWebsiteAddress);
        }

        [TestCase("http://www.myweb.com", "")]
        [TestCase("", "TRUE")]
        [TestCase(null, "TRUE")]
        [TestCase(" ", "TRUE")]
        public void Preamble_questions_contains_no_web_site_flag(string websiteUrl, string expectedValue)
        {
            _applicationDetails.UkrlpLookupDetails.PrimaryContactDetails.ContactWebsiteAddress = websiteUrl;
            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UKrlpNoWebsite);
            question.Should().NotBeNull();
            question.Value.Should().Be(expectedValue);
        }

        [Test]
        public void Preamble_questions_contains_legal_address_line_1()
        {
            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine1);
            question.Should().NotBeNull();
            question.Value.Should().Be(_applicationDetails.UkrlpLookupDetails.PrimaryContactDetails.ContactAddress.Address1);
        }

        [Test]
        public void Preamble_questions_contains_legal_address_line_2()
        {
            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine2);
            question.Should().NotBeNull();
            question.Value.Should().Be(_applicationDetails.UkrlpLookupDetails.PrimaryContactDetails.ContactAddress.Address2);
        }

        [Test]
        public void Preamble_questions_contains_legal_address_line_3()
        {
            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine3);
            question.Should().NotBeNull();
            question.Value.Should().Be(_applicationDetails.UkrlpLookupDetails.PrimaryContactDetails.ContactAddress.Address3);
        }

        [Test]
        public void Preamble_questions_contains_legal_address_line_4()
        {
            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressLine4);
            question.Should().NotBeNull();
            question.Value.Should().Be(_applicationDetails.UkrlpLookupDetails.PrimaryContactDetails.ContactAddress.Address4);
        }

        [Test]
        public void Preamble_questions_contains_legal_address_town()
        {
            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressTown);
            question.Should().NotBeNull();
            question.Value.Should().Be(_applicationDetails.UkrlpLookupDetails.PrimaryContactDetails.ContactAddress.Town);
        }

        [Test]
        public void Preamble_questions_contains_legal_address_postcode()
        {
            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpLegalAddressPostcode);
            question.Should().NotBeNull();
            question.Value.Should().Be(_applicationDetails.UkrlpLookupDetails.PrimaryContactDetails.ContactAddress.PostCode);
        }

        [Test]
        public void Preamble_questions_contains_trading_name()
        {
            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpTradingName);
            question.Should().NotBeNull();
            question.Value.Should().Be(_applicationDetails.UkrlpLookupDetails.ProviderAliases[0].Alias);
        }

        [TestCase(true, "TRUE")]
        [TestCase(false, "")]
        public void Preamble_questions_contains_companies_house_manual_entry_required_flag(bool manualEntryFlag, string expectedValue)
        {
            _applicationDetails.CompanySummary.ManualEntryRequired = manualEntryFlag;

            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.CompaniesHouseManualEntryRequired);
            question.Should().NotBeNull();
            question.Value.Should().Be(expectedValue);
        }

        [Test]
        public void Preamble_questions_contains_UKRLP_verification_company_number()
        {
            _applicationDetails.UkrlpLookupDetails.VerificationDetails = new List<VerificationDetails>
            {
                new VerificationDetails { VerificationAuthority = VerificationAuthorities.CompaniesHouseAuthority, VerificationId = "01234567" }
            };

            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpVerificationCompanyNumber);
            question.Should().NotBeNull();
            question.Value.Should().Be(_applicationDetails.UkrlpLookupDetails.VerificationDetails.FirstOrDefault(x => x.VerificationAuthority == VerificationAuthorities.CompaniesHouseAuthority).VerificationId);
        }

        [Test]
        public void Preamble_questions_contains_companies_house_company_name()
        {
            _applicationDetails.CompanySummary = new CompaniesHouseSummary
            {
                CompanyName = "Company Name"
            };

            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.CompaniesHouseCompanyName);
            question.Should().NotBeNull();
            question.Value.Should().Be(_applicationDetails.CompanySummary.CompanyName);
        }

        [Test]
        public void Preamble_questions_contains_companies_house_company_type()
        {
            _applicationDetails.CompanySummary = new CompaniesHouseSummary
            {
                CompanyType = "ltd"
            };

            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.CompaniesHouseCompanyType);
            question.Should().NotBeNull();
            question.Value.Should().Be(_applicationDetails.CompanySummary.CompanyTypeDescription);
        }

        [Test]
        public void Preamble_questions_contains_companies_house_company_status()
        {
            _applicationDetails.CompanySummary = new CompaniesHouseSummary
            {
                Status = CompaniesHouseSummary.CompanyStatusActive
            };

            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.CompaniesHouseCompanyStatus);
            question.Should().NotBeNull();
            question.Value.Should().Be(_applicationDetails.CompanySummary.Status);
        }

        [Test]
        public void Preamble_questions_contains_companies_house_incorporation_date()
        {
            _applicationDetails.CompanySummary = new CompaniesHouseSummary
            {
                IncorporationDate = new DateTime(2017, 03, 08)
            };

            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.CompaniesHouseIncorporationDate);
            question.Should().NotBeNull();
            question.Value.Should().Be(_applicationDetails.CompanySummary.IncorporationDate.Value.ToString());
        }

        [Test]
        public void Preamble_questions_contains_companies_house_verification_flag()
        {
            _applicationDetails.UkrlpLookupDetails.VerificationDetails = new List<VerificationDetails>
            {
                new VerificationDetails { VerificationAuthority = VerificationAuthorities.CompaniesHouseAuthority, VerificationId = "01234567" }
            };

            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany);
            question.Should().NotBeNull();
            question.Value.Should().Be("TRUE");
        }

        [TestCase(true, "TRUE")]
        [TestCase(false, "")]
        public void Preamble_questions_contains_charity_commission_trustee_manual_entry_flag(bool manualEntryRequired, string expectedValue)
        {
            _applicationDetails.CharitySummary = new CharityCommissionSummary
            {
                TrusteeManualEntryRequired = manualEntryRequired
            };

            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.CharityCommissionTrusteeManualEntry);
            question.Should().NotBeNull();
            question.Value.Should().Be(expectedValue);
        }

        [Test]
        public void Preamble_questions_contains_UKRLP_verification_charity_reg_number()
        {
            _applicationDetails.UkrlpLookupDetails.VerificationDetails = new List<VerificationDetails>
            {
                new VerificationDetails { VerificationAuthority = VerificationAuthorities.CharityCommissionAuthority, VerificationId = "1234567" }
            };

            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();
            
            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpVerificationCharityRegNumber);
            question.Should().NotBeNull();
            question.Value.Should().Be(_applicationDetails.UkrlpLookupDetails.VerificationDetails.FirstOrDefault(x => x.VerificationAuthority == VerificationAuthorities.CharityCommissionAuthority).VerificationId);
        }

        [Test]
        public void Preamble_questions_contains_charity_commission_charity_name()
        {
            _applicationDetails.CharitySummary = new CharityCommissionSummary
            {
                CharityName = "Charity name"
            };

            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.CharityCommissionCharityName);
            question.Should().NotBeNull();
            question.Value.Should().Be(_applicationDetails.CharitySummary.CharityName);
        }

        [Test]
        public void Preamble_questions_contains_charity_commission_registration_date()
        {
            _applicationDetails.CharitySummary = new CharityCommissionSummary
            {
                IncorporatedOn = new DateTime(2015, 03, 17)
            };

            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.CharityCommissionRegistrationDate);
            question.Should().NotBeNull();
            question.Value.Should().Be(_applicationDetails.CharitySummary.IncorporatedOn.Value.ToString());
        }

        [Test]
        public void Preamble_questions_contains_charity_commission_verification_flag()
        {
            _applicationDetails.UkrlpLookupDetails.VerificationDetails = new List<VerificationDetails>
            {
                new VerificationDetails { VerificationAuthority = VerificationAuthorities.CharityCommissionAuthority, VerificationId = "1234567" }
            };

            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity);
            question.Should().NotBeNull();
            question.Value.Should().Be("TRUE");
        }

        [Test]
        public void Preamble_questions_contains_UKRLP_verification_sole_trader_partnership()
        {
            _applicationDetails.UkrlpLookupDetails.VerificationDetails = new List<VerificationDetails>
            {
                new VerificationDetails
                {
                    VerificationAuthority = VerificationAuthorities.SoleTraderPartnershipAuthority,
                    VerificationId = "Y"
                }
            };

            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpVerificationSoleTraderPartnership);
            question.Should().NotBeNull();
            question.Value.Should().Be("TRUE");
        }

        [Test]
        public void Preamble_questions_contains_UKRLP_primary_verification_source()
        {
            _applicationDetails.UkrlpLookupDetails.VerificationDetails = new List<VerificationDetails>
            {
                new VerificationDetails { VerificationAuthority = VerificationAuthorities.CharityCommissionAuthority, VerificationId = "1234567" },
                new VerificationDetails { VerificationAuthority = VerificationAuthorities.CompaniesHouseAuthority, VerificationId = "01234567", PrimaryVerificationSource = true }
            };

            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.UkrlpPrimaryVerificationSource);
            question.Should().NotBeNull();
            question.Value.Should().Be(VerificationAuthorities.CompaniesHouseAuthority);
        }

        [Test]
        public void Preamble_questions_contains_on_roatp_flag()
        {
            _applicationDetails.RoatpRegisterStatus = new OrganisationRegisterStatus{ UkprnOnRegister = true };

            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.OnRoatp);
            question.Should().NotBeNull();
            question.Value.Should().Be("TRUE");
        }

        [Test]
        public void Preamble_questions_contains_roatp_current_status()
        {
            _applicationDetails.RoatpRegisterStatus = new OrganisationRegisterStatus { UkprnOnRegister = true, StatusId = OrganisationStatus.Active };

            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.RoatpCurrentStatus);
            question.Should().NotBeNull();
            question.Value.Should().Be(OrganisationStatus.Active.ToString());
        }

        [Test]
        public void Preamble_questions_contains_roatp_removed_reason()
        {
            _applicationDetails.RoatpRegisterStatus = new OrganisationRegisterStatus
            {
                UkprnOnRegister = true, StatusId = OrganisationStatus.Removed, RemovedReasonId = RemovedReason.ChangeOfTradingStatus 
            };

            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.RoatpRemovedReason);
            question.Should().NotBeNull();
            question.Value.Should().Be(RemovedReason.ChangeOfTradingStatus.ToString());
        }

        [Test]
        public void Preamble_questions_contains_roatp_status_date()
        {
            _applicationDetails.RoatpRegisterStatus = new OrganisationRegisterStatus
            {
                UkprnOnRegister = true, StatusId = OrganisationStatus.Active, StatusDate = new DateTime(2014, 02, 02)
            };

            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.RoatpStatusDate);
            question.Should().NotBeNull();
            question.Value.Should().Be(_applicationDetails.RoatpRegisterStatus.StatusDate.Value.ToString());
        }

        [Test]
        public void Preamble_questions_contains_roatp_provider_route()
        {
            _applicationDetails.RoatpRegisterStatus = new OrganisationRegisterStatus { UkprnOnRegister = true, ProviderTypeId = ApplicationRoute.MainProviderApplicationRoute };

            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.RoatpProviderRoute);
            question.Should().NotBeNull();
            question.Value.Should().Be(ApplicationRoute.MainProviderApplicationRoute.ToString());
        }

        [Test]
        public void Preamble_questions_contains_apply_main_provider_route()
        {
            _applicationDetails.ApplicationRoute = new ApplicationRoute { Id = ApplicationRoute.MainProviderApplicationRoute };
            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(
                x => x.QuestionId == RoatpPreambleQuestionIdConstants.ApplyProviderRoute);
            question.Should().NotBeNull();
            question.Value.Should().Be("1");
        }

        [Test]
        public void Preamble_questions_contains_apply_employer_provider_route()
        {
            _applicationDetails.ApplicationRoute = new ApplicationRoute { Id = ApplicationRoute.EmployerProviderApplicationRoute };
            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(
                x => x.QuestionId == RoatpPreambleQuestionIdConstants.ApplyProviderRoute);
            question.Should().NotBeNull();
            question.Value.Should().Be("2");
        }

        [Test]
        public void Preamble_questions_contains_apply_supporting_provider_route()
        {
            _applicationDetails.ApplicationRoute = new ApplicationRoute { Id = ApplicationRoute.SupportingProviderApplicationRoute };
            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(
                x => x.QuestionId == RoatpPreambleQuestionIdConstants.ApplyProviderRoute);
            question.Should().NotBeNull();
            question.Value.Should().Be("3");
        }

        [Test]
        public void Preamble_questions_contains_conditions_of_acceptance_stage_1_application()
        {
            var questions = RoatpPreambleQuestionBuilder.CreatePreambleQuestions(_applicationDetails);

            questions.Should().NotBeNull();

            var question = questions.FirstOrDefault(x => x.QuestionId == RoatpPreambleQuestionIdConstants.COAStage1Application);
            question.Should().NotBeNull();
            question.Value.Should().Be("TRUE");
        }
    }
}
