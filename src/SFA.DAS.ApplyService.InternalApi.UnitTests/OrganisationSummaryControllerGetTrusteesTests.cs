﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.CharityCommission;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using Trustee = SFA.DAS.ApplyService.Domain.CharityCommission.Trustee;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    public class OrganisationSummaryControllerGetTrusteesTests
    {
        private Mock<IMediator> _mediator;
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private Mock<ILogger<OrganisationSummaryController>> _logger;

        private OrganisationSummaryController _controller;

        private readonly Guid _applicationId = new Guid("742d2fc4-69bd-47b6-b93f-c059d59db0c0");

        [SetUp]
        public void Before_each_test()
        {
            _mediator = new Mock<IMediator>();
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _logger = new Mock<ILogger<OrganisationSummaryController>>();
            _controller = new OrganisationSummaryController(_mediator.Object, _qnaApiClient.Object, _logger.Object);
            _qnaApiClient.Setup(x => x.GetTabularDataByTag(_applicationId, It.IsAny<string>())).ReturnsAsync((TabularData)null);
        }


        [Test]
        public async Task get_no_trustees_if_tag_doesnt_exist()
        {
            var result = await _controller.GetDirectorsFromSubmitted(_applicationId) as OkObjectResult;

            result.Value.Should().BeOfType<List<PersonInControl>>();
            var peopleInControl = (List<PersonInControl>)result.Value;

            Assert.AreEqual(0, peopleInControl.Count);
        }


        [TestCase("name 1", "Jan 1990", "alphabetically first name", "Feb 1992")]
        [TestCase("name 2", "Jan 1990", "name 1", "Jan 1989")]

        public async Task get_list_of_trustees_submitted(string name1, string dateOfBirth1, string name2, string dateOfBirth2)
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
                .Setup(x => x.GetTabularDataByTag(_applicationId, RoatpWorkflowQuestionTags.CharityCommissionTrustees))
                .ReturnsAsync(consumedTabularData);

            var result = await _controller.GetTrusteesFromSubmitted(_applicationId) as OkObjectResult;

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
