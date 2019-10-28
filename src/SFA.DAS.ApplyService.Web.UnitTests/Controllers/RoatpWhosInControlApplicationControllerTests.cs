﻿using FluentAssertions;
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

namespace SFA.DAS.ApplyService.Web.UnitTests.Controllers
{
    [TestFixture]
    public class RoatpWhosInControlApplicationControllerTests
    {
        private Mock<IQnaApiClient> _qnaClient;
        private Mock<IApplicationApiClient> _applicationClient;
        private Mock<IAnswerFormService> _answerFormService;
        private RoatpWhosInControlApplicationController _controller;

        private TabularData _directors;
        private TabularData _pscs;

        [SetUp]
        public void Before_each_test()
        {
            _qnaClient = new Mock<IQnaApiClient>();
            _applicationClient = new Mock<IApplicationApiClient>();
            _answerFormService = new Mock<IAnswerFormService>();
            _controller = new RoatpWhosInControlApplicationController(_qnaClient.Object, 
                                                                      _applicationClient.Object, 
                                                                      _answerFormService.Object);
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

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCompany)).ReturnsAsync(verifiedCompaniesHouseAnswer);
            
            var directorsDataAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseDirectors,
                Value = "{}"
            };
            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHouseDirectors)).ReturnsAsync(directorsDataAnswer);

            var pscsDataAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHousePSCs,
                Value = "{}"
            };
            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHousePscs)).ReturnsAsync(pscsDataAnswer);

            var verifiedCharityCommissionAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                Value = ""
            };
            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity)).ReturnsAsync(verifiedCharityCommissionAnswer);
            
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

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCompany)).ReturnsAsync(verifiedCompaniesHouseAnswer);

            var directorsDataAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseDirectors,
                Value = "{}"
            };
            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHouseDirectors)).ReturnsAsync(directorsDataAnswer);

            var pscsDataAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHousePSCs,
                Value = "{}"
            };
            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHousePscs)).ReturnsAsync(pscsDataAnswer);

            var verifiedCharityCommissionAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                Value = "TRUE"
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity)).ReturnsAsync(verifiedCharityCommissionAnswer);

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

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCompany)).ReturnsAsync(verifiedCompaniesHouseAnswer);

            var verifiedCharityCommissionAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                Value = "TRUE"
            };
            
            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity)).ReturnsAsync(verifiedCharityCommissionAnswer);
            
            var trusteesDataAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CharityCommissionTrustees,
                Value = "{}"
            };
            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CharityCommissionTrustees)).ReturnsAsync(trusteesDataAnswer);

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

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCompany)).ReturnsAsync(verifiedCompaniesHouseAnswer);

            var verifiedCharityCommissionAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                Value = ""
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity)).ReturnsAsync(verifiedCharityCommissionAnswer);

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

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHouseDirectors)).ReturnsAsync(directorsAnswer);

            var pscsJson = JsonConvert.SerializeObject(_pscs);

            var pscsAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHousePSCs,
                Value = pscsJson
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHousePscs)).ReturnsAsync(pscsAnswer);

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

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHouseDirectors)).ReturnsAsync(directorsAnswer);

            var pscsJson = JsonConvert.SerializeObject(_pscs);

            var pscsAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHousePSCs,
                Value = pscsJson
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHousePscs)).ReturnsAsync(pscsAnswer);

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

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHouseDirectors)).ReturnsAsync(directorsAnswer);

            var pscsJson = JsonConvert.SerializeObject(_pscs);

            var pscsAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHousePSCs,
                Value = pscsJson
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHousePscs)).ReturnsAsync(pscsAnswer);

            var sequences = new List<ApplicationSequence>();
            sequences.Add(new ApplicationSequence
            {
                SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                Id = Guid.NewGuid()
            });

            _qnaClient.Setup(x => x.GetSequences(It.IsAny<Guid>())).ReturnsAsync(sequences);

            var sections = new List<ApplicationSection>();
            sections.Add(new ApplicationSection
            {
                SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                Id = Guid.NewGuid()
            });

            _qnaClient.Setup(x => x.GetSections(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(sections);

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

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity)).ReturnsAsync(verifiedCharityAnswer);

            _applicationClient.Setup(x => x.MarkSectionAsCompleted(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true).Verifiable();

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
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseDirectors,
                Value = directorsJson
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHouseDirectors)).ReturnsAsync(directorsAnswer);

            var pscsJson = JsonConvert.SerializeObject(_pscs);

            var pscsAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHousePSCs,
                Value = pscsJson
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CompaniesHousePscs)).ReturnsAsync(pscsAnswer);

            var sequences = new List<ApplicationSequence>();
            sequences.Add(new ApplicationSequence
            {
                SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                Id = Guid.NewGuid()
            });

            _qnaClient.Setup(x => x.GetSequences(It.IsAny<Guid>())).ReturnsAsync(sequences);

            var sections = new List<ApplicationSection>();
            sections.Add(new ApplicationSection
            {
                SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                Id = Guid.NewGuid()
            });

            _qnaClient.Setup(x => x.GetSections(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(sections);

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

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCharity)).ReturnsAsync(verifiedCharityAnswer);

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

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CharityCommissionTrustees)).ReturnsAsync(trusteesAnswer);

            var verifiedCompanyAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany,
                Value = "TRUE"
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCompany)).ReturnsAsync(verifiedCompanyAnswer);

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

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CharityCommissionTrustees)).ReturnsAsync(trusteesAnswer);

            var verifiedCompanyAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany,
                Value = "TRUE"
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpVerificationCompany)).ReturnsAsync(verifiedCompanyAnswer);

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
        }

        [Test]
        public void Confirm_trustees_dob_presents_trustees_with_prefilled_dates_of_birth()
        {

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

            var trusteesJson = JsonConvert.SerializeObject(trustees);

            var trusteesAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CharityCommissionTrustees,
                Value = trusteesJson
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CharityCommissionTrustees)).ReturnsAsync(trusteesAnswer);

            var viewModel = new ConfirmTrusteesDateOfBirthViewModel
            {
                ApplicationId = Guid.NewGuid()
            };

            var result = _controller.TrusteesDobsConfirmed(viewModel).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as ConfirmTrusteesDateOfBirthViewModel;
            model.Should().NotBeNull();
            model.ErrorMessages.Count.Should().Be(2);
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

            var trusteesJson = JsonConvert.SerializeObject(trustees);

            var trusteesAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CharityCommissionTrustees,
                Value = trusteesJson
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CharityCommissionTrustees)).ReturnsAsync(trusteesAnswer);

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

            var trusteesJson = JsonConvert.SerializeObject(trustees);

            var trusteesAnswer = new Answer
            {
                QuestionId = RoatpPreambleQuestionIdConstants.CharityCommissionTrustees,
                Value = trusteesJson
            };

            _qnaClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.CharityCommissionTrustees)).ReturnsAsync(trusteesAnswer);

            var sequences = new List<ApplicationSequence>();
            sequences.Add(new ApplicationSequence
            {
                SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                Id = Guid.NewGuid()
            });

            _qnaClient.Setup(x => x.GetSequences(It.IsAny<Guid>())).ReturnsAsync(sequences);

            var sections = new List<ApplicationSection>();
            sections.Add(new ApplicationSection
            {
                SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                Id = Guid.NewGuid()
            });

            _qnaClient.Setup(x => x.GetSections(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(sections);

            var updateResult = new SetPageAnswersResponse
            {
                ValidationPassed = true
            };
            _qnaClient.Setup(x => x.UpdatePageAnswers(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<List<Answer>>())).ReturnsAsync(updateResult);

            _applicationClient.Setup(x => x.MarkSectionAsCompleted(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true).Verifiable();

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
    }
}
