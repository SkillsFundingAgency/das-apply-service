using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Application.Apply.UpdatePageAnswers;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Web.Controllers.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.UnitTests.Controllers
{
    [TestFixture]
    public class RoatpWhosInControlApplicationControllerTests
    {
        private Mock<IQnaApiClient> _client;
        private RoatpWhosInControlApplicationController _controller;

        private TabularData _directors;
        private TabularData _pscs;

        [SetUp]
        public void Before_each_test()
        {
            _client = new Mock<IQnaApiClient>();
            _controller = new RoatpWhosInControlApplicationController(_client.Object);

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

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCompany)).ReturnsAsync(verifiedCompaniesHouseAnswer);
            
            var directorsDataAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseDirectors,
                Value = "{}"
            };
            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHouseDirectors)).ReturnsAsync(directorsDataAnswer);

            var pscsDataAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHousePSCs,
                Value = "{}"
            };
            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHousePscs)).ReturnsAsync(pscsDataAnswer);

            var verifiedCharityCommissionAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                Value = ""
            };
            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity)).ReturnsAsync(verifiedCharityCommissionAnswer);
            
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

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCompany)).ReturnsAsync(verifiedCompaniesHouseAnswer);

            var directorsDataAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseDirectors,
                Value = "{}"
            };
            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHouseDirectors)).ReturnsAsync(directorsDataAnswer);

            var pscsDataAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHousePSCs,
                Value = "{}"
            };
            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHousePscs)).ReturnsAsync(pscsDataAnswer);

            var verifiedCharityCommissionAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                Value = "TRUE"
            };

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity)).ReturnsAsync(verifiedCharityCommissionAnswer);

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

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCompany)).ReturnsAsync(verifiedCompaniesHouseAnswer);

            var verifiedCharityCommissionAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                Value = "TRUE"
            };
            
            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity)).ReturnsAsync(verifiedCharityCommissionAnswer);
            
            var trusteesDataAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CharityCommissionTrustees,
                Value = "{}"
            };
            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CharityCommissionTrustees)).ReturnsAsync(trusteesDataAnswer);

            var result = _controller.StartPage(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ConfirmTrusteesNoDob");
        }

        [Test]
        public void Start_page_routes_to_add_people_in_control_if_not_verified_with_companies_house_or_charity_commission()
        {
            var verifiedCompaniesHouseAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany,
                Value = ""
            };

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCompany)).ReturnsAsync(verifiedCompaniesHouseAnswer);

            var verifiedCharityCommissionAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                Value = ""
            };

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity)).ReturnsAsync(verifiedCharityCommissionAnswer);

            var result = _controller.StartPage(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("AddPeopleInControl");
        }

        [Test]
        public void Confirm_directors_pscs_presents_lists_of_directors_and_pscs()
        {
            var directorsJson = JsonConvert.SerializeObject(_directors);

            var directorsAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseDirectors,
                Value = directorsJson
            };

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHouseDirectors)).ReturnsAsync(directorsAnswer);

            var pscsJson = JsonConvert.SerializeObject(_pscs);

            var pscsAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHousePSCs,
                Value = pscsJson
            };

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHousePscs)).ReturnsAsync(pscsAnswer);

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
            var directorsJson = JsonConvert.SerializeObject(
            new TabularData {
                DataRows = new List<TabularDataRow>()
            });

            var directorsAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseDirectors,
                Value = directorsJson
            };

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHouseDirectors)).ReturnsAsync(directorsAnswer);

            var pscsJson = JsonConvert.SerializeObject(_pscs);

            var pscsAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHousePSCs,
                Value = pscsJson
            };

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHousePscs)).ReturnsAsync(pscsAnswer);

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
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseDirectors,
                Value = directorsJson
            };

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHouseDirectors)).ReturnsAsync(directorsAnswer);

            var pscsJson = JsonConvert.SerializeObject(_pscs);

            var pscsAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHousePSCs,
                Value = pscsJson
            };

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHousePscs)).ReturnsAsync(pscsAnswer);

            var sequences = new List<ApplicationSequence>();
            sequences.Add(new ApplicationSequence
            {
                SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                Id = Guid.NewGuid()
            });

            _client.Setup(x => x.GetSequences(It.IsAny<Guid>())).ReturnsAsync(sequences);

            var sections = new List<ApplicationSection>();
            sections.Add(new ApplicationSection
            {
                SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                Id = Guid.NewGuid()
            });

            _client.Setup(x => x.GetSections(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(sections);

            var updateResult = new UpdatePageAnswersResult
            {
                ValidationPassed = true
            };
            _client.Setup(x => x.UpdatePageAnswers(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<List<Answer>>())).ReturnsAsync(updateResult);

            var verifiedCharityAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                Value = ""
            };

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity)).ReturnsAsync(verifiedCharityAnswer);

            var result = _controller.DirectorsPscsConfirmed(Guid.NewGuid()).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("TaskList");

        }

        [Test]
        public void Directors_pscs_confirmed_redirects_to_confirm_trustees_if_also_verified_as_charity()
        {
            var directorsJson = JsonConvert.SerializeObject(_directors);

            var directorsAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseDirectors,
                Value = directorsJson
            };

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHouseDirectors)).ReturnsAsync(directorsAnswer);

            var pscsJson = JsonConvert.SerializeObject(_pscs);

            var pscsAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHousePSCs,
                Value = pscsJson
            };

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHousePscs)).ReturnsAsync(pscsAnswer);

            var sequences = new List<ApplicationSequence>();
            sequences.Add(new ApplicationSequence
            {
                SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                Id = Guid.NewGuid()
            });

            _client.Setup(x => x.GetSequences(It.IsAny<Guid>())).ReturnsAsync(sequences);

            var sections = new List<ApplicationSection>();
            sections.Add(new ApplicationSection
            {
                SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                Id = Guid.NewGuid()
            });

            _client.Setup(x => x.GetSections(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(sections);

            var updateResult = new UpdatePageAnswersResult
            {
                ValidationPassed = true
            };
            _client.Setup(x => x.UpdatePageAnswers(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<List<Answer>>())).ReturnsAsync(updateResult);

            var verifiedCharityAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                Value = "TRUE"
            };

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity)).ReturnsAsync(verifiedCharityAnswer);

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

            var trusteesJson = JsonConvert.SerializeObject(trustees);

            var trusteesAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CharityCommissionTrustees,
                Value = trusteesJson
            };

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CharityCommissionTrustees)).ReturnsAsync(trusteesAnswer);

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

            var trusteesJson = JsonConvert.SerializeObject(trustees);

            var trusteesAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CharityCommissionTrustees,
                Value = trusteesJson
            };

            _client.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CharityCommissionTrustees)).ReturnsAsync(trusteesAnswer);

            var result = _controller.ConfirmTrusteesNoDob(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as ConfirmTrusteesViewModel;
            model.Should().NotBeNull();

            int trusteesCount = model.Trustees.TableData.DataRows.Count;
            trusteesCount.Should().Be(1);
        }
    }
}
