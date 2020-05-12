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
using SFA.DAS.ApplyService.Domain.CompaniesHouse;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Types;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    public class OrganisationSummaryControllerGetPscTests
    {
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private Mock<IApplyRepository> _applyRepository;
        private Mock<ILogger<OrganisationSummaryController>> _logger;

        private OrganisationSummaryController _controller;

        private readonly Guid _applicationId = Guid.NewGuid();

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
        public void get_no_people_in_control_if_tag_doesnt_exist()
        {
            var result = (OkObjectResult)_controller.GetPscsFromSubmitted(_applicationId).Result;

            result.Should().BeOfType<OkObjectResult>();

            result.Value.Should().BeOfType<List<PersonInControl>>();
            var peopleInControl = (List<PersonInControl>)result.Value;

              Assert.AreEqual(0,peopleInControl.Count);
        }


        [TestCase("name 1", "Jan 1990", "alphabetically first name", "Feb 1992")]
        [TestCase("name 2", "Jan 1990", "name 1", "Jan 1989")]

        public void get_list_of_pscs_submitted(string name1, string dateOfBirth1, string name2, string dateOfBirth2)
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
                .Setup(x => x.GetTabularDataByTag(_applicationId, RoatpWorkflowQuestionTags.CompaniesHousePscs))
                .ReturnsAsync(consumedTabularData);

            var result = (OkObjectResult)_controller.GetPscsFromSubmitted(_applicationId).Result;

            result.Should().BeOfType<OkObjectResult>();

            result.Value.Should().BeOfType<List<PersonInControl>>();
            var peopleInControl = (List<PersonInControl>)result.Value;
            Assert.AreEqual(2, peopleInControl.Count);
            Assert.AreEqual(peopleInControl[0].Name, expectedPersonInControl1.Name);
            Assert.AreEqual(peopleInControl[0].MonthYearOfBirth, expectedPersonInControl1.MonthYearOfBirth);
            Assert.AreEqual(peopleInControl[1].Name, expectedPersonInControl2.Name);
            Assert.AreEqual(peopleInControl[1].MonthYearOfBirth, expectedPersonInControl2.MonthYearOfBirth);
        }

        [Test]
        public void get_no_psc_if_apply_data_doesnt_exist()
        {
            var result = (OkObjectResult)_controller.GetPscsFromCompaniesHouse(_applicationId).Result;

            result.Should().BeOfType<OkObjectResult>();

            result.Value.Should().BeOfType<List<PersonInControl>>();
            var peopleInControl = (List<PersonInControl>)result.Value;

            Assert.AreEqual(0, peopleInControl.Count);
        }

        [TestCase("name 1", "1 Jan 1980", "alphabetically first name", "1 Feb 1992")]
        [TestCase("name 2", "Jan 1990", "name 1", "Jan 1989")]

        public void get_list_of_pscs_from_apply(string name1, DateTime? dateOfBirth1, string name2, DateTime? dateOfBirth2)
        {
            var expectedPersonInControl2 = new PersonInControl { Name = name1, MonthYearOfBirth = $"{dateOfBirth1:MMM yyyy}" };
            var expectedPersonInControl1 = new PersonInControl { Name = name2, MonthYearOfBirth = $"{dateOfBirth2:MMM yyyy}" };

            var consumedApplyData = new ApplyData
            {
                GatewayReviewDetails = new ApplyGatewayDetails
                {
                    CompaniesHouseDetails = new CompaniesHouseSummary
                    {
                        PersonsWithSignificantControl = new List<PersonSignificantControlInformation>
                        {
                            new PersonSignificantControlInformation {Name = name1, DateOfBirth = dateOfBirth1 },
                            new PersonSignificantControlInformation {Name = name2, DateOfBirth = dateOfBirth2}
                        }
                    }
                }
            };

            _applyRepository.Setup(x => x.GetApplyData(_applicationId))
                .ReturnsAsync(consumedApplyData);

            var result = (OkObjectResult)_controller.GetPscsFromCompaniesHouse(_applicationId).Result;

            result.Should().BeOfType<OkObjectResult>();

            result.Value.Should().BeOfType<List<PersonInControl>>();
            var peopleInControl = (List<PersonInControl>)result.Value;
            Assert.AreEqual(2, peopleInControl.Count);
            Assert.AreEqual(peopleInControl[0].Name, expectedPersonInControl1.Name);
            Assert.AreEqual(peopleInControl[0].MonthYearOfBirth, expectedPersonInControl1.MonthYearOfBirth);
            Assert.AreEqual(peopleInControl[1].Name, expectedPersonInControl2.Name);
            Assert.AreEqual(peopleInControl[1].MonthYearOfBirth, expectedPersonInControl2.MonthYearOfBirth);
        }

    }
}
