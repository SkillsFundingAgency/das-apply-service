﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Financial.Applications;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.Financial.OpenFinancialApplicationsHandlerTests
{
    [TestFixture]
    public class OpenFinancialApplicationsHandlerTests
    {
        private Mock<IApplyRepository> _applyRepository;
        private OpenFinancialApplicationsHandler _handler;

        private List<RoatpFinancialSummaryItem> _openApplications;

        [SetUp]
        public void Setup()
        {
            var openFinancialApplication = new RoatpFinancialSummaryItem { ApplicationStatus = ApplicationStatus.GatewayAssessed, FinancialReviewStatus = FinancialReviewStatus.New, DeclaredInApplication = "Not exempt", SubmittedDate = DateTime.Now };
            var openExemptFinancialApplication = new RoatpFinancialSummaryItem { ApplicationStatus = ApplicationStatus.GatewayAssessed, FinancialReviewStatus = FinancialReviewStatus.New, DeclaredInApplication = "Exempt", SubmittedDate = DateTime.Now };

            _openApplications = new List<RoatpFinancialSummaryItem> { openFinancialApplication, openExemptFinancialApplication };

            _applyRepository = new Mock<IApplyRepository>();
            _applyRepository.Setup(r => r.GetOpenFinancialApplications()).ReturnsAsync(_openApplications);

            _handler = new OpenFinancialApplicationsHandler(_applyRepository.Object);
        }

        [Test]
        public async Task OpenFinancialApplicationsHandler_returns_expected_applications()
        {
            var request = new OpenFinancialApplicationsRequest();
            var result = await _handler.Handle(request, new CancellationToken());

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(_openApplications);
        }
    }
}