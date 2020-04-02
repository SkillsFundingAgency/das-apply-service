using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Web.Controllers.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using System;
using System.Collections.Generic;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.InternalApi.Types;
using SFA.DAS.ApplyService.Session;

namespace SFA.DAS.ApplyService.Web.UnitTests.Controllers
{
    [TestFixture]
    public class RoatpWhosInControlApplicationControllerTests
    {
        private Mock<IQnaApiClient> _qnaClient;
        private Mock<IApplicationApiClient> _applicationClient;
        private Mock<IAnswerFormService> _answerFormService;
        private Mock<ITabularDataRepository> _tabularDataRepository;
        private Mock<ISessionService> _sessionService;
        private RoatpWhosInControlApplicationController _controller;

        private TabularData _directors;
        private TabularData _pscs;

        [SetUp]
        public void Before_each_test()
        {
            _qnaClient = new Mock<IQnaApiClient>();
            _applicationClient = new Mock<IApplicationApiClient>();
            _answerFormService = new Mock<IAnswerFormService>();
            _tabularDataRepository = new Mock<ITabularDataRepository>();
            _sessionService = new Mock<ISessionService>();
            _controller = new RoatpWhosInControlApplicationController(_qnaClient.Object, 
                                                                      _applicationClient.Object, 
                                                                      _answerFormService.Object,
                                                                      _tabularDataRepository.Object,
                                                                      _sessionService.Object);
            _directors = new TabularData
            {
                Caption = "Directors",
                HeadingTitles = new List<string>()
                {
                    "Name", "Date of birth"
                },
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string>
                        {
                            "Mr A Director", "Feb 1976"
                        }
                    },
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string>
                        {
                            "Mr B Director", "Jun 1984"
                        }
                    }
                }
            };

            _pscs = new TabularData
            {
                Caption = "PSCs",
                HeadingTitles = new List<string>()
                {
                    "Name", "Date of birth"
                },
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string>
                        {
                            "Mr C Director", "Oct 1982"
                        }
                    },
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string>
                        {
                            "Mr D Director", "Jan 1972"
                        }
                    },
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string>
                        {
                            "Mr E Director", "Nov 1980"
                        }
                    }
                }
            };
        }

        [Test]
        public void Start_page_routes_to_confirm_directors_pscs_if_provider_verified_with_companies_house()
        {
            var verifiedCompaniesHouseAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany,
                Value = "TRUE"
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCompany, It.IsAny<string>())).ReturnsAsync(verifiedCompaniesHouseAnswer);

            var manualEntryCompaniesHouseAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseManualEntryRequired,
                Value = ""
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.ManualEntryRequiredCompaniesHouse, It.IsAny<string>())).ReturnsAsync(manualEntryCompaniesHouseAnswer);
            
            var directorsDataAnswer = new Answer
            {
                QuestionId = RoatpYourOrganisationQuestionIdConstants.CompaniesHouseDirectors,
                Value = "{}"
            };
            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHouseDirectors, It.IsAny<string>())).ReturnsAsync(directorsDataAnswer);

            var pscsDataAnswer = new Answer
            {
                QuestionId = RoatpYourOrganisationQuestionIdConstants.CompaniesHousePSCs,
                Value = "{}"
            };
            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHousePscs, It.IsAny<string>())).ReturnsAsync(pscsDataAnswer);

            var verifiedCharityCommissionAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                Value = ""
            };
            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity, It.IsAny<string>())).ReturnsAsync(verifiedCharityCommissionAnswer);
            
            var result = _controller.StartPage(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ConfirmDirectorsPscs");
        }

        [Test]
        public void Start_page_routes_to_confirm_directors_pscs_if_provider_verified_with_companies_house_and_charity_commission()
        {
            var verifiedCompaniesHouseAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany,
                Value = "TRUE"
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCompany, It.IsAny<string>())).ReturnsAsync(verifiedCompaniesHouseAnswer);

            var manualEntryCompaniesHouseAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseManualEntryRequired,
                Value = ""
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.ManualEntryRequiredCompaniesHouse, It.IsAny<string>())).ReturnsAsync(manualEntryCompaniesHouseAnswer);

            var directorsDataAnswer = new Answer
            {
                QuestionId = RoatpYourOrganisationQuestionIdConstants.CompaniesHouseDirectors,
                Value = "{}"
            };
            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHouseDirectors, It.IsAny<string>())).ReturnsAsync(directorsDataAnswer);

            var pscsDataAnswer = new Answer
            {
                QuestionId = RoatpYourOrganisationQuestionIdConstants.CompaniesHousePSCs,
                Value = "{}"
            };
            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHousePscs, It.IsAny<string>())).ReturnsAsync(pscsDataAnswer);

            var verifiedCharityCommissionAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                Value = "TRUE"
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity, It.IsAny<string>())).ReturnsAsync(verifiedCharityCommissionAnswer);

            var result = _controller.StartPage(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ConfirmDirectorsPscs");
        }

        [Test]
        public void Start_page_routes_to_confirm_trustees_if_provider_verified_with_charity_commission()
        {
            var verifiedCompaniesHouseAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany,
                Value = ""
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCompany, It.IsAny<string>())).ReturnsAsync(verifiedCompaniesHouseAnswer);

            var verifiedCharityCommissionAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                Value = "TRUE"
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity, It.IsAny<string>())).ReturnsAsync(verifiedCharityCommissionAnswer);

            var manualEntryCharityCommissionAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CharityCommissionTrusteeManualEntry,
                Value = ""
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.ManualEntryRequiredCharityCommission, It.IsAny<string>())).ReturnsAsync(manualEntryCharityCommissionAnswer);

            var trusteesDataAnswer = new Answer
            {
                QuestionId = RoatpYourOrganisationQuestionIdConstants.CharityCommissionTrustees,
                Value = "{}"
            };
            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CharityCommissionTrustees, It.IsAny<string>())).ReturnsAsync(trusteesDataAnswer);

            var result = _controller.StartPage(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ConfirmTrusteesNoDob");
        }

        [Test]
        public void Start_page_routes_to_organisation_type_if_ukrlp_verification_source_sole_trader_partnership()
        {
            var verifiedCompaniesHouseAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany,
                Value = ""
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCompany, It.IsAny<string>())).ReturnsAsync(verifiedCompaniesHouseAnswer);

            var verifiedCharityCommissionAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                Value = ""
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity, It.IsAny<string>())).ReturnsAsync(verifiedCharityCommissionAnswer);
            
            var verifiedSoleTraderPartnershipAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationSoleTraderPartnership,
                Value = "TRUE"
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationSoleTraderPartnership, It.IsAny<string>())).ReturnsAsync(verifiedSoleTraderPartnershipAnswer);

            var result = _controller.StartPage(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("SoleTraderOrPartnership");
        }

        [Test]
        public void Start_page_routes_to_add_people_in_control_if_not_verified_with_companies_house_or_charity_commission_or_sole_trader_partnership()
        {
            var verifiedCompaniesHouseAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany,
                Value = ""
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCompany, It.IsAny<string>())).ReturnsAsync(verifiedCompaniesHouseAnswer);

            var verifiedCharityCommissionAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                Value = ""
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity, It.IsAny<string>())).ReturnsAsync(verifiedCharityCommissionAnswer);

            var verifiedSoleTraderPartnershipAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationSoleTraderPartnership,
                Value = ""
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationSoleTraderPartnership, It.IsAny<string>())).ReturnsAsync(verifiedSoleTraderPartnershipAnswer);
            
            var result = _controller.StartPage(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("AddPeopleInControl");
        }

        [Test]
        public void Confirm_directors_pscs_presents_lists_of_directors_and_pscs()
        {
            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHouseDirectors)).ReturnsAsync(_directors);
            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHousePscs)).ReturnsAsync(_pscs);
            
            var result = _controller.ConfirmDirectorsPscs(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();

            var model = viewResult.Model as ConfirmDirectorsPscsViewModel;
            model.Should().NotBeNull();
            int directorsCount = model.CompaniesHouseDirectors.TableData.DataRows.Count;
            directorsCount.Should().Be(2);
            int pscsCount = model.CompaniesHousePscs.TableData.DataRows.Count;
            pscsCount.Should().Be(3);
        }

        [Test]
        public void Confirm_directors_pscs_presents_list_of_pscs_but_no_directors()
        {
            var directorsData = new TabularData
            {
                DataRows = new List<TabularDataRow>()
            };

            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHouseDirectors)).ReturnsAsync(directorsData);
            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHousePscs)).ReturnsAsync(_pscs);
            
            var result = _controller.ConfirmDirectorsPscs(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();

            var model = viewResult.Model as ConfirmDirectorsPscsViewModel;
            model.Should().NotBeNull();
            int directorsCount = model.CompaniesHouseDirectors.TableData.DataRows.Count;
            directorsCount.Should().Be(0);
            int pscsCount = model.CompaniesHousePscs.TableData.DataRows.Count;
            pscsCount.Should().Be(3);
        }

        [Test]
        public void Directors_pscs_confirmed_redirects_to_task_list_if_only_verified_as_a_company()
        {
            var directorsJson = JsonConvert.SerializeObject(_directors);

            var directorsAnswer = new Answer
            {
                QuestionId = RoatpYourOrganisationQuestionIdConstants.CompaniesHouseDirectors,
                Value = directorsJson
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHouseDirectors, It.IsAny<string>())).ReturnsAsync(directorsAnswer);

            var pscsJson = JsonConvert.SerializeObject(_pscs);

            var pscsAnswer = new Answer
            {
                QuestionId = RoatpYourOrganisationQuestionIdConstants.CompaniesHousePSCs,
                Value = pscsJson
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHousePscs, It.IsAny<string>())).ReturnsAsync(pscsAnswer);

            var section = new ApplicationSection
            {
                SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                Id = Guid.NewGuid()
            };

            _qnaClient.Setup(x => x.GetSectionBySectionNo(It.IsAny<Guid>(), RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl)).ReturnsAsync(section);

            var updateResult = new SetPageAnswersResponse
            {
                ValidationPassed = true
            };
            _qnaClient.Setup(x => x.UpdatePageAnswers(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<List<Answer>>())).ReturnsAsync(updateResult);

            var verifiedCharityAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                Value = ""
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity, It.IsAny<string>())).ReturnsAsync(verifiedCharityAnswer);

            var result = _controller.DirectorsPscsConfirmed(Guid.NewGuid()).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("TaskList");

            _applicationClient.VerifyAll();
        }

        [Test]
        public void Directors_pscs_confirmed_redirects_to_confirm_trustees_if_also_verified_as_charity()
        {
            var directorsJson = JsonConvert.SerializeObject(_directors);

            var directorsAnswer = new Answer
            {
                QuestionId = RoatpYourOrganisationQuestionIdConstants.CompaniesHouseDirectors,
                Value = directorsJson
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHouseDirectors, It.IsAny<string>())).ReturnsAsync(directorsAnswer);

            var pscsJson = JsonConvert.SerializeObject(_pscs);

            var pscsAnswer = new Answer
            {
                QuestionId = RoatpYourOrganisationQuestionIdConstants.CompaniesHousePSCs,
                Value = pscsJson
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHousePscs, It.IsAny<string>())).ReturnsAsync(pscsAnswer);

            var section = new ApplicationSection
            {
                SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                Id = Guid.NewGuid()
            };

            _qnaClient.Setup(x => x.GetSectionBySectionNo(It.IsAny<Guid>(), RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl)).ReturnsAsync(section);

            var updateResult = new SetPageAnswersResponse
            {
                ValidationPassed = true
            };
            _qnaClient.Setup(x => x.UpdatePageAnswers(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<List<Answer>>())).ReturnsAsync(updateResult);

            var verifiedCharityAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                Value = "TRUE"
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity, It.IsAny<string>())).ReturnsAsync(verifiedCharityAnswer);

            var result = _controller.DirectorsPscsConfirmed(Guid.NewGuid()).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ConfirmTrusteesNoDob");
        }

        [Test]
        public void Confirm_trustees_presents_list_of_trustees()
        {
            var trustees = new TabularData
            {
                Caption = "",
                HeadingTitles = new List<string>()
                {
                    "Name"
                },
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string>
                        {
                            "Mr A Trustee"
                        }
                    },
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string>
                        {
                            "Mrs B Trustee"
                        }
                    }
                }
            };

            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CharityCommissionTrustees)).ReturnsAsync(trustees);

            var verifiedCompanyAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany,
                Value = "TRUE"
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCompany, It.IsAny<string>())).ReturnsAsync(verifiedCompanyAnswer);

            var result = _controller.ConfirmTrusteesNoDob(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as ConfirmTrusteesViewModel;
            model.Should().NotBeNull();

            int trusteesCount = model.Trustees.TableData.DataRows.Count;
            trusteesCount.Should().Be(2);
        }

        [Test]
        public void Confirm_trustees_presents_single_trustee()
        {
            var trustees = new TabularData
            {
                Caption = "",
                HeadingTitles = new List<string>()
                {
                    "Name"
                },
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string>
                        {
                            "Miss C Trustee"
                        }
                    }
                }
            };

            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CharityCommissionTrustees)).ReturnsAsync(trustees);

            var verifiedCompanyAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany,
                Value = "TRUE"
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCompany, It.IsAny<string>())).ReturnsAsync(verifiedCompanyAnswer);

            var result = _controller.ConfirmTrusteesNoDob(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as ConfirmTrusteesViewModel;
            model.Should().NotBeNull();

            int trusteesCount = model.Trustees.TableData.DataRows.Count;
            trusteesCount.Should().Be(1);
        }

        [Test]
        public void Confirm_trustees_dob_presents_trustees_with_no_dates_of_birth()
        {
            var trustees = new TabularData
            {
                Caption = "",
                HeadingTitles = new List<string>()
                {
                    "Name"
                },
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string>
                        {
                            "Miss C Trustee"
                        }
                    }
                }
            };

            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CharityCommissionTrustees)).ReturnsAsync(trustees);
            
            var result = _controller.ConfirmTrusteesDob(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as ConfirmTrusteesDateOfBirthViewModel;
            model.Should().NotBeNull();
            model.TrusteeDatesOfBirth.Count.Should().Be(1);
            model.TrusteeDatesOfBirth[0].Name.Should().Be("Miss C Trustee");
            model.TrusteeDatesOfBirth[0].DobMonth.Should().BeNullOrEmpty();
            model.TrusteeDatesOfBirth[0].DobYear.Should().BeNullOrEmpty();
        }

        [Test]
        public void Confirm_trustees_dob_presents_trustees_with_prefilled_dates_of_birth()
        {
            var trustees = new TabularData
            {
                Caption = "",
                HeadingTitles = new List<string>()
                {
                    "Name",
                    "Date of birth"
                },
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string>
                        {
                            "Miss C Trustee",
                            "Nov 1991"
                        }
                    }
                }
            };

            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CharityCommissionTrustees)).ReturnsAsync(trustees);

            var result = _controller.ConfirmTrusteesDob(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as ConfirmTrusteesDateOfBirthViewModel;
            model.Should().NotBeNull();
            model.TrusteeDatesOfBirth.Count.Should().Be(1);
            model.TrusteeDatesOfBirth[0].Name.Should().Be("Miss C Trustee");
            model.TrusteeDatesOfBirth[0].DobMonth.Should().Be("11");
            model.TrusteeDatesOfBirth[0].DobYear.Should().Be("1991");
        }

        [Test]
        public void Trustees_dob_confirmed_rejects_missing_dates_of_birth()
        {
            _answerFormService.Setup(x => x.GetAnswersFromForm(It.IsAny<HttpContext>())).Returns(new List<Answer>());

            var trustees = new TabularData
            {
                Caption = "",
                HeadingTitles = new List<string>()
                {
                    "Name"
                },
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string>
                        {
                            "Mr A Trustee"
                        }
                    },
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string>
                        {
                            "Mrs B Trustee"
                        }
                    }
                }
            };

            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CharityCommissionTrustees)).ReturnsAsync(trustees);

            var viewModel = new ConfirmTrusteesDateOfBirthViewModel
            {
                ApplicationId = Guid.NewGuid()
            };

            var result = _controller.TrusteesDobsConfirmed(viewModel).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as ConfirmTrusteesDateOfBirthViewModel;
            model.Should().NotBeNull();
            model.ErrorMessages.Count.Should().Be(1);
        }

        [Test]
        public void Trustees_dob_confirmed_rejects_invalid_date_of_birth()
        {
            var dobAnswers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = "10002000_Month",
                    Value = "13"
                },
                new Answer
                {
                    QuestionId = "10002000_Year",
                    Value = "1993"
                },
                new Answer
                {
                    QuestionId = "10003000_Month",
                    Value = "1"
                },
                new Answer
                {
                    QuestionId = "10003000_Year",
                    Value = "1994"
                }
            };
            _answerFormService.Setup(x => x.GetAnswersFromForm(It.IsAny<HttpContext>())).Returns(dobAnswers);

            var trustees = new TabularData
            {
                Caption = "",
                HeadingTitles = new List<string>()
                {
                    "Name"
                },
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = "10002000",
                        Columns = new List<string>
                        {
                            "Mr A Trustee"
                        }
                    },
                    new TabularDataRow
                    {
                        Id = "10003000",
                        Columns = new List<string>
                        {
                            "Mrs B Trustee"
                        }
                    }
                }
            };

            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CharityCommissionTrustees)).ReturnsAsync(trustees);
            
            var viewModel = new ConfirmTrusteesDateOfBirthViewModel
            {
                ApplicationId = Guid.NewGuid()
            };

            var result = _controller.TrusteesDobsConfirmed(viewModel).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as ConfirmTrusteesDateOfBirthViewModel;
            model.Should().NotBeNull();
            model.ErrorMessages.Count.Should().Be(1);
        }

        [Test]
        public void Trustees_dob_confirmed_submits_valid_dates_of_birth_to_qna()
        {
            var dobAnswers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = "10002000_Month",
                    Value = "3"
                },
                new Answer
                {
                    QuestionId = "10002000_Year",
                    Value = "1993"
                },
                new Answer
                {
                    QuestionId = "10003000_Month",
                    Value = "1"
                },
                new Answer
                {
                    QuestionId = "10003000_Year",
                    Value = "1994"
                }
            };
            _answerFormService.Setup(x => x.GetAnswersFromForm(It.IsAny<HttpContext>())).Returns(dobAnswers);

            var trustees = new TabularData
            {
                Caption = "",
                HeadingTitles = new List<string>()
                {
                    "Name"
                },
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = "10002000",
                        Columns = new List<string>
                        {
                            "Mr A Trustee"
                        }
                    },
                    new TabularDataRow
                    {
                        Id = "10003000",
                        Columns = new List<string>
                        {
                            "Mrs B Trustee"
                        }
                    }
                }
            };

            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CharityCommissionTrustees)).ReturnsAsync(trustees);

            var section = new ApplicationSection
            {
                SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                Id = Guid.NewGuid()
            };

            _qnaClient.Setup(x => x.GetSectionBySectionNo(It.IsAny<Guid>(), RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl)).ReturnsAsync(section);

            var updateResult = new SetPageAnswersResponse
            {
                ValidationPassed = true
            };
            _qnaClient.Setup(x => x.UpdatePageAnswers(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<List<Answer>>())).ReturnsAsync(updateResult);

            var viewModel = new ConfirmTrusteesDateOfBirthViewModel
            {
                ApplicationId = Guid.NewGuid()
            };

            var result = _controller.TrusteesDobsConfirmed(viewModel).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("TaskList");

            _applicationClient.VerifyAll();
        }

        [TestCase(RoatpOrganisationTypes.Partnership, "PartnershipType")]
        [TestCase(RoatpOrganisationTypes.SoleTrader, "AddSoleTradeDob")]
        public void Confirm_sole_trader_or_partnership_redirects_to_partnership_type_or_add_sole_trader_dob(string organisationType, string expectedActionName)
        {
            var section = new ApplicationSection
            {
                SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                Id = Guid.NewGuid()
            };

            _qnaClient.Setup(x => x.GetSectionBySectionNo(It.IsAny<Guid>(), RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl)).ReturnsAsync(section);

            var updateResult = new SetPageAnswersResponse
            {
                ValidationPassed = true
            };
            _qnaClient.Setup(x => x.UpdatePageAnswers(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<List<Answer>>())).ReturnsAsync(updateResult);

            var model = new SoleTraderOrPartnershipViewModel
            {
                ApplicationId = Guid.NewGuid(),
                OrganisationType = organisationType
            };

            var result = _controller.ConfirmSoleTraderOrPartnership(model).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();

            redirectResult.ActionName.Should().Be(expectedActionName);
        }

        [Test]
        public void Add_sole_trade_dob_prompts_for_date_of_birth_with_sole_trader_legal_name()
        {
            var legalNameAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalName,
                Value = "Sole Trader Name"
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpLegalName, It.IsAny<string>())).ReturnsAsync(legalNameAnswer);

            var result = _controller.AddSoleTradeDob(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as SoleTradeDobViewModel;
            model.Should().NotBeNull();
            model.SoleTraderName.Should().Be(legalNameAnswer.Value);
        }

        [Test]
        public void Add_sole_trade_dob_prefills_month_and_year_if_valid_values_previously_entered()
        {
            var legalNameAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalName,
                Value = "Sole Trader Name"
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpLegalName, It.IsAny<string>())).ReturnsAsync(legalNameAnswer);

            var dateOfBirthAnswer = new Answer
            {
                QuestionId = RoatpYourOrganisationQuestionIdConstants.AddSoleTradeDob,
                Value = "11,1991"
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.SoleTradeDob, It.IsAny<string>())).ReturnsAsync(dateOfBirthAnswer);

            var result = _controller.AddSoleTradeDob(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as SoleTradeDobViewModel;
            model.Should().NotBeNull();
            model.SoleTraderName.Should().Be(legalNameAnswer.Value);
            model.SoleTraderDobMonth.Should().Be("11");
            model.SoleTraderDobYear.Should().Be("1991");
        }

        [TestCase("", "")]
        [TestCase("1", "")]
        [TestCase("", "1991")]
        [TestCase("13", "1992")]
        [TestCase("12", "999")]
        [TestCase("10", "3000")]
        public void Confirm_sole_trade_dob_rejects_invalid_values(string dobMonth, string dobYear)
        {
            var viewModel = new SoleTradeDobViewModel
            {
                SoleTraderDobMonth = dobMonth,
                SoleTraderDobYear = dobYear,
                ApplicationId = Guid.NewGuid(),
                ErrorMessages = new List<ValidationErrorDetail>()
            };

            var result = _controller.SoleTradeDobConfirmed(viewModel).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as SoleTradeDobViewModel;
            model.Should().NotBeNull();
            model.ErrorMessages.Count.Should().BeGreaterOrEqualTo(1);
        }

        [Test]
        public void Confirm_sole_trade_redirects_to_task_list_for_valid_values()
        {
            var section = new ApplicationSection {
                SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                Id = Guid.NewGuid()
            };

            _qnaClient.Setup(x => x.GetSectionBySectionNo(It.IsAny<Guid>(), RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl)).ReturnsAsync(section);

            var updateResult = new SetPageAnswersResponse
            {
                ValidationPassed = true
            };
            _qnaClient.Setup(x => x.UpdatePageAnswers(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<List<Answer>>())).ReturnsAsync(updateResult);

            var viewModel = new SoleTradeDobViewModel
            {
                SoleTraderDobMonth = "12",
                SoleTraderDobYear = "1963",
                ApplicationId = Guid.NewGuid(),
                ErrorMessages = new List<ValidationErrorDetail>()
            };

            var result = _controller.SoleTradeDobConfirmed(viewModel).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("TaskList");
        }

        [Test]
        public void Add_partner_individual_prompts_for_name_and_date_of_birth()
        {
            var individualPartnerAnswer = new Answer
            {
                QuestionId = RoatpYourOrganisationQuestionIdConstants.AddPartners,
                Value = null
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.AddPartners, It.IsAny<string>())).ReturnsAsync(individualPartnerAnswer);

            var partnerTypeAnswer = new Answer
            {
                QuestionId = RoatpYourOrganisationQuestionIdConstants.PartnershipType,
                Value = ConfirmPartnershipTypeViewModel.PartnershipTypeIndividual
            };
            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.PartnershipType, It.IsAny<string>())).ReturnsAsync(partnerTypeAnswer);

            var result = _controller.AddPartner(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as AddEditPeopleInControlViewModel;
            model.Should().NotBeNull();
            model.PersonInControlName.Should().BeNullOrEmpty();
            model.PersonInControlDobMonth.Should().BeNullOrEmpty();
            model.PersonInControlDobYear.Should().BeNullOrEmpty();
        }
        
        [TestCase("", "", "", true)]
        [TestCase("", "", "", false)]
        [TestCase("", "1", "", true)]
        [TestCase("", "", "1991", true)]
        [TestCase("", "13", "1992", true)]
        [TestCase("", "12", "999", true)]
        [TestCase("", "10", "3000", true)]
        [TestCase("Partner name", "", "", true)]
        [TestCase("Partner name", "1", "", true)]
        [TestCase("Partner name", "", "1991", true)]
        [TestCase("Partner name", "13", "1992", true)]
        [TestCase("Partner name", "12", "999", true)]
        [TestCase("Partner name", "10", "3000", true)]
        public void Add_partner_details_rejects_invalid_values(string partnerName, string dobMonth, string dobYear, bool isIndividual)
        {
            var viewModel = new AddEditPeopleInControlViewModel
            {
                PersonInControlDobMonth = dobMonth,
                PersonInControlDobYear = dobYear,
                PersonInControlName = partnerName,
                DateOfBirthOptional = !isIndividual,
                ApplicationId = Guid.NewGuid(),
                ErrorMessages = new List<ValidationErrorDetail>()
            };

            var result = _controller.AddPartnerDetails(viewModel).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as AddEditPeopleInControlViewModel;
            model.Should().NotBeNull();
            model.ErrorMessages.Count.Should().BeGreaterOrEqualTo(1);
        }
        
        [Test]
        public void Edit_partner_replays_stored_details_for_an_individual_partner()
        {
            const int index = 1;
            var partnerTableData = new TabularData
            {
                HeadingTitles = new List<string> { "Name", "Date of birth" },
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string> { "Miss I Partner", "Mar 1976" }
                    },
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string> { "Mrs O Partner", "Jun 1975" }
                    }
                }
            };

            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.AddPartners)).ReturnsAsync(partnerTableData);

            var result = _controller.EditPartner(Guid.NewGuid(), index).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("EditPartner");

            var model = viewResult.Model as AddEditPeopleInControlViewModel;
            model.Should().NotBeNull();

            model.Index.Should().Be(index);
            model.DateOfBirthOptional.Should().BeFalse();
            model.PersonInControlName.Should().Be("Mrs O Partner");
            model.PersonInControlDobMonth.Should().Be("6");
            model.PersonInControlDobYear.Should().Be("1975");
        }

        [Test]
        public void Edit_partner_replays_stored_details_for_an_organisation_partner()
        {
            const int index = 2;
            var partnerTableData = new TabularData
            {
                HeadingTitles = new List<string> { "Name", "Date of birth" },
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string> { "Mr D Partner", "Mar 1980" }
                    },
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string> { "Partner LLP", string.Empty }
                    },
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string> { "Partner Trust", string.Empty }
                    }
                }
            };

            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.AddPartners)).ReturnsAsync(partnerTableData);

            var result = _controller.EditPartner(Guid.NewGuid(), index).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("EditPartner");

            var model = viewResult.Model as AddEditPeopleInControlViewModel;
            model.Should().NotBeNull();

            model.Index.Should().Be(index);
            model.DateOfBirthOptional.Should().BeTrue();
            model.PersonInControlName.Should().Be("Partner Trust");
            model.PersonInControlDobMonth.Should().BeNullOrEmpty();
            model.PersonInControlDobYear.Should().BeNullOrEmpty();
        }

        [Test]
        public void Edit_partner_redirects_to_confirm_page_if_invalid_index_supplied()
        {
            const int index = 1;
            var partnerTableData = new TabularData
            {
                HeadingTitles = new List<string> { "Name", "Date of birth" },
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string> { "Miss I Partner", "Mar 1976" }
                    }
                }
            };

            var answerJson = JsonConvert.SerializeObject(partnerTableData);

            var partnersAnswer = new Answer
            {
                QuestionId = RoatpYourOrganisationQuestionIdConstants.AddPartners,
                Value = answerJson
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.AddPartners, It.IsAny<string>())).ReturnsAsync(partnersAnswer);

            var result = _controller.EditPartner(Guid.NewGuid(), index).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ConfirmPartners");
        }

        [TestCase("", "", "", true)]
        [TestCase("", "", "", false)]
        [TestCase("", "1", "", true)]
        [TestCase("", "", "1991", true)]
        [TestCase("", "13", "1992", true)]
        [TestCase("", "12", "999", true)]
        [TestCase("", "10", "3000", true)]
        [TestCase("Partner name", "", "", true)]
        [TestCase("Partner name", "1", "", true)]
        [TestCase("Partner name", "", "1991", true)]
        [TestCase("Partner name", "13", "1992", true)]
        [TestCase("Partner name", "12", "999", true)]
        [TestCase("Partner name", "10", "3000", true)]
        public void Update_partner_details_rejects_invalid_values(string partnerName, string dobMonth, string dobYear, bool isIndividual)
        {
            var viewModel = new AddEditPeopleInControlViewModel
            {
                PersonInControlDobMonth = dobMonth,
                PersonInControlDobYear = dobYear,
                PersonInControlName = partnerName,
                DateOfBirthOptional = !isIndividual,
                ApplicationId = Guid.NewGuid(),
                ErrorMessages = new List<ValidationErrorDetail>()
            };

            var result = _controller.UpdatePartnerDetails(viewModel).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as AddEditPeopleInControlViewModel;
            model.Should().NotBeNull();
            model.ErrorMessages.Count.Should().BeGreaterOrEqualTo(1);
        }

        [Test]
        public void Confirm_partners_replays_single_partner()
        {
            var partnerTableData = new TabularData
            {
                HeadingTitles = new List<string> { "Name", "Date of birth" },
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string> { "Miss I Partner", "Mar 1976" }
                    }
                }
            };

            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.AddPartners)).ReturnsAsync(partnerTableData);
            
            var result = _controller.ConfirmPartners(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();

            var model = viewResult.Model as ConfirmPartnersViewModel;
            model.Should().NotBeNull();

            model.PartnerData.DataRows.Count.Should().Be(1);
            model.PartnerData.DataRows[0].Columns[0].Should().Be("Miss I Partner");
            model.PartnerData.DataRows[0].Columns[1].Should().Be("Mar 1976");
        }

        [Test]
        public void Confirm_partners_replays_multiple_partners_of_each_type()
        {
            var partnerTableData = new TabularData
            {
                HeadingTitles = new List<string> { "Name", "Date of birth" },
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string> { "Miss I Partner", "Mar 1976" }
                    },
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string> { "Org Ltd", string.Empty }
                    }
                }
            };

            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.AddPartners)).ReturnsAsync(partnerTableData);

            var result = _controller.ConfirmPartners(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();

            var model = viewResult.Model as ConfirmPartnersViewModel;
            model.Should().NotBeNull();

            model.PartnerData.DataRows.Count.Should().Be(2);
            model.PartnerData.DataRows[0].Columns[0].Should().Be("Miss I Partner");
            model.PartnerData.DataRows[0].Columns[1].Should().Be("Mar 1976");
            model.PartnerData.DataRows[1].Columns[0].Should().Be("Org Ltd");
            model.PartnerData.DataRows[1].Columns[1].Should().BeNullOrEmpty();
        }

        [TestCase("", "", "")]
        [TestCase("", "1", "")]
        [TestCase("", "", "1991")]
        [TestCase("", "13", "1992")]
        [TestCase("", "12", "999")]
        [TestCase("", "10", "3000")]
        [TestCase("Person name", "", "")]
        [TestCase("Person name", "1", "")]
        [TestCase("Person name", "", "1991")]
        [TestCase("Person name", "13", "1992")]
        [TestCase("Person name", "12", "999")]
        [TestCase("Person name", "10", "3000")]
        public void Add_people_in_control_rejects_invalid_values(string personName, string dobMonth, string dobYear)
        {
            var viewModel = new AddEditPeopleInControlViewModel
            {
                PersonInControlName = personName,
                PersonInControlDobMonth = dobMonth,
                PersonInControlDobYear = dobYear,
                ApplicationId = Guid.NewGuid(),
                ErrorMessages = new List<ValidationErrorDetail>()
            };

            var result = _controller.AddPeopleInControlDetails(viewModel).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as AddEditPeopleInControlViewModel;
            model.Should().NotBeNull();
            model.ErrorMessages.Count.Should().BeGreaterOrEqualTo(1);
        }

        [Test]
        public void Edit_people_in_control_replays_stored_details_for_an_individual_person()
        {
            const int index = 1;
            var personTableData = new TabularData
            {
                HeadingTitles = new List<string> { "Name", "Date of birth" },
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string> { "Miss I Person", "Mar 1976" }
                    },
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string> { "Mrs O Person", "Jun 1975" }
                    }
                }
            };

            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.AddPeopleInControl)).ReturnsAsync(personTableData);

            var result = _controller.EditPeopleInControl(Guid.NewGuid(), index).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("EditPeopleInControl");

            var model = viewResult.Model as AddEditPeopleInControlViewModel;
            model.Should().NotBeNull();

            model.Index.Should().Be(index);
            model.DateOfBirthOptional.Should().BeFalse();
            model.PersonInControlName.Should().Be("Mrs O Person");
            model.PersonInControlDobMonth.Should().Be("6");
            model.PersonInControlDobYear.Should().Be("1975");
        }
                
        [Test]
        public void Edit_people_in_control_redirects_to_confirm_page_if_invalid_index_supplied()
        {
            const int index = 1;
            var personTableData = new TabularData
            {
                HeadingTitles = new List<string> { "Name", "Date of birth" },
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string> { "Miss I Person", "Mar 1976" }
                    }
                }
            };

            var answerJson = JsonConvert.SerializeObject(personTableData);

            var peopleAnswer = new Answer
            {
                QuestionId = RoatpYourOrganisationQuestionIdConstants.AddPeopleInControl,
                Value = answerJson
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.AddPartners, It.IsAny<string>())).ReturnsAsync(peopleAnswer);

            var result = _controller.EditPeopleInControl(Guid.NewGuid(), index).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ConfirmPeopleInControl");
        }

        [TestCase("", "", "")]
        [TestCase("", "", "")]
        [TestCase("", "1", "")]
        [TestCase("", "", "1991")]
        [TestCase("", "13", "1992")]
        [TestCase("", "12", "999")]
        [TestCase("", "10", "3000")]
        [TestCase("Person name", "", "")]
        [TestCase("Person name", "1", "")]
        [TestCase("Person name", "", "1991")]
        [TestCase("Person name", "13", "1992")]
        [TestCase("Person name", "12", "999")]
        [TestCase("Person name", "10", "3000")]
        public void Update_people_in_control_details_rejects_invalid_values(string personName, string dobMonth, string dobYear)
        {
            var viewModel = new AddEditPeopleInControlViewModel
            {
                PersonInControlDobMonth = dobMonth,
                PersonInControlDobYear = dobYear,
                PersonInControlName = personName,
                DateOfBirthOptional = false,
                ApplicationId = Guid.NewGuid(),
                ErrorMessages = new List<ValidationErrorDetail>()
            };

            var result = _controller.UpdatePeopleInControlDetails(viewModel).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as AddEditPeopleInControlViewModel;
            model.Should().NotBeNull();
            model.ErrorMessages.Count.Should().BeGreaterOrEqualTo(1);
        }

        [Test]
        public void Remove_partner_shows_confirmation_page_with_partner_name()
        {
            var partnerData = new TabularData
            {
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Columns = new List<string> { "Mrs A Partner" , "Feb 1999" }
                    },
                    new TabularDataRow
                    {
                        Columns = new List<string> { "Mr A Rogue" , "Feb 1999" }
                    }
                }
            };

            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.AddPartners)).ReturnsAsync(partnerData);

            var index = 1;

            var result = _controller.RemovePartner(Guid.NewGuid(), index).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();

            var model = viewResult.Model as ConfirmRemovePersonInControlViewModel;
            model.Should().NotBeNull();

            model.Name.Should().Be("Mr A Rogue");
            model.Confirmation.Should().BeNullOrEmpty();
        }

        [Test]
        public void Remove_partner_redirects_if_invalid_index_supplied()
        {
            var partnerData = new TabularData
            {
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Columns = new List<string> { "Mrs A Partner" , "Feb 1999" }
                    },
                    new TabularDataRow
                    {
                        Columns = new List<string> { "Mr A Rogue" , "Feb 1999" }
                    }
                }
            };

            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.AddPartners)).ReturnsAsync(partnerData);

            var index = 2;

            var result = _controller.RemovePartner(Guid.NewGuid(), index).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();

            redirectResult.ActionName.Should().Be("ConfirmPartners");
        }


        [Test]
        public void Remove_people_in_control_shows_confirmation_page_with_person_name()
        {
            var pscData = new TabularData
            {
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Columns = new List<string> { "Mrs A Person" , "Feb 1999" }
                    },
                    new TabularDataRow
                    {
                        Columns = new List<string> { "Mr B Rogue" , "Feb 1999" }
                    }
                }
            };

            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.AddPeopleInControl)).ReturnsAsync(pscData);

            var index = 1;

            var result = _controller.RemovePeopleInControl(Guid.NewGuid(), index).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();

            var model = viewResult.Model as ConfirmRemovePersonInControlViewModel;
            model.Should().NotBeNull();

            model.Name.Should().Be("Mr B Rogue");
            model.Confirmation.Should().BeNullOrEmpty();
        }

        [Test]
        public void Remove_people_in_control_redirects_if_invalid_index_supplied()
        {
            var pscData = new TabularData
            {
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Columns = new List<string> { "Mrs A Partner" , "Feb 1999" }
                    },
                    new TabularDataRow
                    {
                        Columns = new List<string> { "Mr A Rogue" , "Feb 1999" }
                    }
                }
            };

            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.AddPeopleInControl)).ReturnsAsync(pscData);

            var index = 2;

            var result = _controller.RemovePeopleInControl(Guid.NewGuid(), index).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();

            redirectResult.ActionName.Should().Be("ConfirmPeopleInControl");
        }
        
        [Test]
        public void Remove_item_from_pscs_shows_error_if_not_confirmed()
        {
            var model = new ConfirmRemovePersonInControlViewModel
            {
                ActionName = "Action",
                ApplicationId = Guid.NewGuid(),
                Confirmation = null,
                ErrorMessages = new List<ValidationErrorDetail>(),
                Index = 1,
                Name = "Name to be removed"
            };

            var result = _controller.RemovePartnerDetails(model).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var viewModel = viewResult.Model as ConfirmRemovePersonInControlViewModel;
            model.Should().NotBeNull();
            model.ErrorMessages.Count.Should().Be(1);
            model.ErrorMessages[0].ErrorMessage.Should().Contain(model.Name); 
        }

        [Test]
        public void Remove_item_from_partners_redirects_if_chosen_not_to_remove_entry()
        {
            var model = new ConfirmRemovePersonInControlViewModel
            {
                ActionName = "Action",
                ApplicationId = Guid.NewGuid(),
                Confirmation = "N",
                ErrorMessages = new List<ValidationErrorDetail>(),
                Index = 1,
                Name = "Name to be removed"
            };

            var result = _controller.RemovePartnerDetails(model).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("ConfirmPartners");
        }

        [Test]
        public void Remove_item_from_pscs_redirects_if_chosen_not_to_remove_entry()
        {
            var model = new ConfirmRemovePersonInControlViewModel
            {
                ActionName = "Action",
                ApplicationId = Guid.NewGuid(),
                Confirmation = "N",
                ErrorMessages = new List<ValidationErrorDetail>(),
                Index = 1,
                Name = "Name to be removed"
            };

            var result = _controller.RemovePscDetails(model).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("ConfirmPeopleInControl");
        }

        [Test]
        public void Remove_item_from_partners_saves_new_answer_with_entry_removed_if_confirm_removal()
        {
            var model = new ConfirmRemovePersonInControlViewModel
            {
                ActionName = "Action",
                ApplicationId = Guid.NewGuid(),
                Confirmation = "Y",
                ErrorMessages = new List<ValidationErrorDetail>(),
                Index = 1,
                Name = "Name to be removed"
            };

            var partnerData = new TabularData
            {
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Columns = new List<string> { "Mrs A Partner" , "Feb 1999" }
                    },
                    new TabularDataRow
                    {
                        Columns = new List<string> { "Name to be removed" , "Feb 1999" }
                    }
                }
            };

            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.AddPartners)).ReturnsAsync(partnerData);

            var section = new ApplicationSection
            {
                SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                Id = Guid.NewGuid()
            };

            _qnaClient.Setup(x => x.GetSectionBySectionNo(It.IsAny<Guid>(), RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl)).ReturnsAsync(section);

            _tabularDataRepository.Setup(x => x.SaveTabularDataAnswer(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TabularData>())).ReturnsAsync(true).Verifiable();

            var result = _controller.RemovePartnerDetails(model).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("ConfirmPartners");
            _tabularDataRepository.Verify(x => x.SaveTabularDataAnswer(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TabularData>()), Times.Once);
        }

        [Test]
        public void Remove_item_from_pscs_saves_new_answer_with_entry_removed_if_confirm_removal()
        {
            var model = new ConfirmRemovePersonInControlViewModel
            {
                ActionName = "Action",
                ApplicationId = Guid.NewGuid(),
                Confirmation = "Y",
                ErrorMessages = new List<ValidationErrorDetail>(),
                Index = 1,
                Name = "Name to be removed"
            };

            var pscsData = new TabularData
            {
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Columns = new List<string> { "Mrs A Person" , "Feb 1988" }
                    },
                    new TabularDataRow
                    {
                        Columns = new List<string> { "Name to be removed" , "Feb 1987" }
                    }
                }
            };

            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.AddPeopleInControl)).ReturnsAsync(pscsData);

            var section = new ApplicationSection
            {
                SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                Id = Guid.NewGuid()
            };

            _qnaClient.Setup(x => x.GetSectionBySectionNo(It.IsAny<Guid>(), RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl)).ReturnsAsync(section);

            _tabularDataRepository.Setup(x => x.SaveTabularDataAnswer(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TabularData>())).ReturnsAsync(true).Verifiable();

            var result = _controller.RemovePscDetails(model).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("ConfirmPeopleInControl");
            _tabularDataRepository.Verify(x => x.SaveTabularDataAnswer(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TabularData>()), Times.Once);
        }
    }
}
