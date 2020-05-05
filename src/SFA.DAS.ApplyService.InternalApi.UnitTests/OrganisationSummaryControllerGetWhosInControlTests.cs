using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    public class OrganisationSummaryControllerGetWhosInControlTests
    {
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private Mock<IApplyRepository> _applyRepository;
        private Mock<ILogger<OrganisationSummaryController>> _logger;

        private OrganisationSummaryController _controller;

        private readonly Guid _applicationId = new Guid("742d2fc4-69bd-47b6-b93f-c059d59db0c0");

        [SetUp]
        public void Before_each_test()
        {
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _applyRepository = new Mock<IApplyRepository>();
            _logger = new Mock<ILogger<OrganisationSummaryController>>();
            _controller = new OrganisationSummaryController(_qnaApiClient.Object,_applyRepository.Object, _logger.Object);
            _qnaApiClient.Setup(x => x.GetTabularDataByTag(_applicationId, It.IsAny<string>())).ReturnsAsync((TabularData)null);
            _applyRepository.Setup(x => x.GetApplyData(It.IsAny<Guid>())).ReturnsAsync((ApplyData) null);
        }

        [Test]
        public void get_no_tags_if_tag_doesnt_exist()
        {
            var result = (OkObjectResult)_controller.GetWhosInControlFromSubmitted(_applicationId).Result;

            result.Should().BeOfType<OkObjectResult>();

            result.Value.Should().BeOfType<List<PersonInControl>>();
            var peopleInControl = (List<PersonInControl>)result.Value;

              Assert.AreEqual(0,peopleInControl.Count);
        }

        [TestCase("name 1", "Jan 1990", "alphabetically first name", "Feb 1992")]
        [TestCase("name 2", "Jan 1990", "name 1", "Jan 1989")]

        public void get_list_of_whos_in_control_submitted_when_AddPeopleInControl_tag_filled(string name1, string dateOfBirth1, string name2, string dateOfBirth2)
        {
            var expectedPersonInControl2 = new PersonInControl { Name = name1, MonthYearOfBirth = dateOfBirth1 };
            var expectedPersonInControl1 = new PersonInControl { Name = name2, MonthYearOfBirth = dateOfBirth2 };

            var consumedTabularData = new TabularData
            {
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow {Columns = new List<string> {name1, dateOfBirth1}},
                    new TabularDataRow {Columns = new List<string> {name2, dateOfBirth2}}
                }
            };

            _qnaApiClient
                .Setup(x => x.GetTabularDataByTag(_applicationId, RoatpWorkflowQuestionTags.AddPeopleInControl))
                .ReturnsAsync(consumedTabularData);

            _qnaApiClient
                .Setup(x => x.GetTabularDataByTag(_applicationId, RoatpWorkflowQuestionTags.AddPartners))
                .ReturnsAsync((TabularData)null);
            
            _qnaApiClient
                .Setup(x => x.GetAnswerByTag(_applicationId, RoatpWorkflowQuestionTags.SoleTradeDob,null))
                .ReturnsAsync((Answer)null);

            _qnaApiClient
                .Setup(x => x.GetAnswerByTag(_applicationId, RoatpWorkflowQuestionTags.UkrlpLegalName, null))
                .ReturnsAsync((Answer)null);

            var result = (OkObjectResult)_controller.GetWhosInControlFromSubmitted(_applicationId).Result;

            result.Should().BeOfType<OkObjectResult>();

            result.Value.Should().BeOfType<List<PersonInControl>>();
            var peopleInControl = (List<PersonInControl>)result.Value;
            Assert.AreEqual(2, peopleInControl.Count);
            Assert.AreEqual(peopleInControl[0].Name, expectedPersonInControl1.Name);
            Assert.AreEqual(peopleInControl[0].MonthYearOfBirth, expectedPersonInControl1.MonthYearOfBirth);
            Assert.AreEqual(peopleInControl[1].Name, expectedPersonInControl2.Name);
            Assert.AreEqual(peopleInControl[1].MonthYearOfBirth, expectedPersonInControl2.MonthYearOfBirth);

            _qnaApiClient.Verify(x => x.GetTabularDataByTag(_applicationId, RoatpWorkflowQuestionTags.AddPeopleInControl), Times.Once);
            _qnaApiClient.Verify(x=>x.GetTabularDataByTag(It.IsAny<Guid>(),RoatpWorkflowQuestionTags.AddPartners), Times.Never);
            _qnaApiClient.Verify(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.SoleTradeDob, null), Times.Never);
            _qnaApiClient.Verify(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpLegalName, null), Times.Never);

        }

        [TestCase("name 1", "Jan 1990", "alphabetically first name", "Feb 1992")]
        [TestCase("name 2", "Jan 1990", "name 1", "Jan 1989")]

        public void get_list_of_whos_in_control_submitted_when_AddPartners_tag_filled(string name1, string dateOfBirth1, string name2, string dateOfBirth2)
        {
            var expectedPersonInControl2 = new PersonInControl { Name = name1, MonthYearOfBirth = dateOfBirth1 };
            var expectedPersonInControl1 = new PersonInControl { Name = name2, MonthYearOfBirth = dateOfBirth2 };

            var consumedTabularData = new TabularData
            {
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow {Columns = new List<string> {name1, dateOfBirth1}},
                    new TabularDataRow {Columns = new List<string> {name2, dateOfBirth2}}
                }
            };

            _qnaApiClient
                .Setup(x => x.GetTabularDataByTag(_applicationId, RoatpWorkflowQuestionTags.AddPeopleInControl))
                .ReturnsAsync((TabularData)null);

            _qnaApiClient
                .Setup(x => x.GetTabularDataByTag(_applicationId, RoatpWorkflowQuestionTags.AddPartners))
                .ReturnsAsync(consumedTabularData);

            _qnaApiClient
                .Setup(x => x.GetAnswerByTag(_applicationId, RoatpWorkflowQuestionTags.SoleTradeDob, null))
                .ReturnsAsync((Answer)null);

            _qnaApiClient
                .Setup(x => x.GetAnswerByTag(_applicationId, RoatpWorkflowQuestionTags.UkrlpLegalName, null))
                .ReturnsAsync((Answer)null);

            var result = (OkObjectResult)_controller.GetWhosInControlFromSubmitted(_applicationId).Result;

            result.Should().BeOfType<OkObjectResult>();

            result.Value.Should().BeOfType<List<PersonInControl>>();
            var peopleInControl = (List<PersonInControl>)result.Value;
            Assert.AreEqual(2, peopleInControl.Count);
            Assert.AreEqual(peopleInControl[0].Name, expectedPersonInControl1.Name);
            Assert.AreEqual(peopleInControl[0].MonthYearOfBirth, expectedPersonInControl1.MonthYearOfBirth);
            Assert.AreEqual(peopleInControl[1].Name, expectedPersonInControl2.Name);
            Assert.AreEqual(peopleInControl[1].MonthYearOfBirth, expectedPersonInControl2.MonthYearOfBirth);

            _qnaApiClient.Verify(x => x.GetTabularDataByTag(_applicationId, RoatpWorkflowQuestionTags.AddPeopleInControl), Times.Once);
            _qnaApiClient.Verify(x => x.GetTabularDataByTag(_applicationId, RoatpWorkflowQuestionTags.AddPartners), Times.Once);
            _qnaApiClient.Verify(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.SoleTradeDob, null), Times.Never);
            _qnaApiClient.Verify(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpLegalName, null), Times.Never);

        }

        [TestCase("name 1", "2,1991", "Feb 1991")]
        [TestCase("name 2", "1,1990", "Jan 1990")]

        public void get_list_of_whos_in_control_submitted_when_SoleTradeDob_tag_filled(string legalName,
            string soleTraderDob, string formattedDob)
        {
            var expectedPersonInControl1 = new PersonInControl {Name = legalName, MonthYearOfBirth = soleTraderDob};

            _qnaApiClient
                .Setup(x => x.GetTabularDataByTag(_applicationId, RoatpWorkflowQuestionTags.AddPeopleInControl))
                .ReturnsAsync((TabularData) null);

            _qnaApiClient
                .Setup(x => x.GetTabularDataByTag(_applicationId, RoatpWorkflowQuestionTags.AddPartners))
                .ReturnsAsync((TabularData) null);

            _qnaApiClient
                .Setup(x => x.GetAnswerByTag(_applicationId, RoatpWorkflowQuestionTags.SoleTradeDob, null))
                .ReturnsAsync(new Answer {Value = soleTraderDob});

            _qnaApiClient
                .Setup(x => x.GetAnswerByTag(_applicationId, RoatpWorkflowQuestionTags.UkrlpLegalName, null))
                .ReturnsAsync(new Answer {Value = legalName});

            var result = (OkObjectResult) _controller.GetWhosInControlFromSubmitted(_applicationId).Result;

            result.Should().BeOfType<OkObjectResult>();

            result.Value.Should().BeOfType<List<PersonInControl>>();
            var peopleInControl = (List<PersonInControl>) result.Value;
            Assert.AreEqual(1, peopleInControl.Count);
            Assert.AreEqual(peopleInControl[0].Name, expectedPersonInControl1.Name);
            Assert.AreEqual(peopleInControl[0].MonthYearOfBirth, formattedDob);

            _qnaApiClient.Verify(
                x => x.GetTabularDataByTag(_applicationId, RoatpWorkflowQuestionTags.AddPeopleInControl), Times.Once);
            _qnaApiClient.Verify(x => x.GetTabularDataByTag(_applicationId, RoatpWorkflowQuestionTags.AddPartners),
                Times.Once);
            _qnaApiClient.Verify(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.SoleTradeDob, null),
                Times.Once);
            _qnaApiClient.Verify(
                x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpLegalName, null), Times.Once);
        }

        [TestCase("name 1")]
        [TestCase("name 2")]
        public void get_no_list_of_whos_in_control_submitted_when_SoleTradeDob_tag_not_filled(string legalName)
        {
            _qnaApiClient
                .Setup(x => x.GetTabularDataByTag(_applicationId, RoatpWorkflowQuestionTags.AddPeopleInControl))
                .ReturnsAsync((TabularData)null);

            _qnaApiClient
                .Setup(x => x.GetTabularDataByTag(_applicationId, RoatpWorkflowQuestionTags.AddPartners))
                .ReturnsAsync((TabularData)null);

            _qnaApiClient
                .Setup(x => x.GetAnswerByTag(_applicationId, RoatpWorkflowQuestionTags.SoleTradeDob, null))
                .ReturnsAsync(new Answer { Value = null });

            _qnaApiClient
                .Setup(x => x.GetAnswerByTag(_applicationId, RoatpWorkflowQuestionTags.UkrlpLegalName, null))
                .ReturnsAsync(new Answer { Value = legalName });

            var result = (OkObjectResult)_controller.GetWhosInControlFromSubmitted(_applicationId).Result;

            result.Should().BeOfType<OkObjectResult>();

            result.Value.Should().BeOfType<List<PersonInControl>>();
            var peopleInControl = (List<PersonInControl>)result.Value;
            Assert.AreEqual(0, peopleInControl.Count);

            _qnaApiClient.Verify(
                x => x.GetTabularDataByTag(_applicationId, RoatpWorkflowQuestionTags.AddPeopleInControl), Times.Once);
            _qnaApiClient.Verify(x => x.GetTabularDataByTag(_applicationId, RoatpWorkflowQuestionTags.AddPartners),
                Times.Once);
            _qnaApiClient.Verify(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.SoleTradeDob, null),
                Times.Once);
            _qnaApiClient.Verify(
                x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpLegalName, null), Times.Never);
        }
    }
}
