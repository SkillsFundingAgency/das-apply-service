using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Financial.Applications;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.FinancialApplicationsStatusCountsHandlerTests
{
    [TestFixture]
    public class FinancialApplicationsStatusCountsHandlerTests
    {
        private Mock<IApplyRepository> _applyRepository;
        private FinancialApplicationsStatusCountsHandler _handler;

        [SetUp]
        public void Setup()
        {
            var openFinancialApplication = new RoatpFinancialSummaryItem { ApplicationStatus = ApplicationStatus.GatewayAssessed, FinancialReviewStatus = FinancialReviewStatus.New, DeclaredInApplication = "Not exempt",  SubmittedDate = DateTime.Now };
            var openExemptFinancialApplication = new RoatpFinancialSummaryItem { ApplicationStatus = ApplicationStatus.GatewayAssessed, FinancialReviewStatus = FinancialReviewStatus.New, DeclaredInApplication = "Exempt", SubmittedDate = DateTime.Now };
            var clarificationFinancialApplication = new RoatpFinancialSummaryItem { ApplicationStatus = ApplicationStatus.GatewayAssessed, FinancialReviewStatus = FinancialReviewStatus.Clarification, SubmittedDate = DateTime.Now, FeedbackAddedDate = DateTime.Now };
            var closedFinancialApplication = new RoatpFinancialSummaryItem { ApplicationStatus = ApplicationStatus.GatewayAssessed, FinancialReviewStatus = FinancialReviewStatus.Approved, SubmittedDate = DateTime.Now, ClosedDate = DateTime.Now };

            var openApplications = new List<RoatpFinancialSummaryItem> { openFinancialApplication, openExemptFinancialApplication };
            var clarificationApplications = new List<RoatpFinancialSummaryItem> { clarificationFinancialApplication };
            var closedApplications = new List<RoatpFinancialSummaryItem> { closedFinancialApplication };

            _applyRepository = new Mock<IApplyRepository>();

            // Note: These 3 methods have been added for reference as the concrete version of GetFinancialApplicationsStatusCounts currently calls into these to get their counts.
            _applyRepository.Setup(r => r.GetOpenFinancialApplications()).ReturnsAsync(openApplications);
            _applyRepository.Setup(r => r.GetClarificationFinancialApplications()).ReturnsAsync(clarificationApplications);
            _applyRepository.Setup(r => r.GetClosedFinancialApplications()).ReturnsAsync(closedApplications);

            _applyRepository.Setup(r => r.GetFinancialApplicationsStatusCounts()).ReturnsAsync(new RoatpFinancialApplicationsStatusCounts
            {
                ApplicationsOpen = openApplications.Count,
                ApplicationsWithClarification = clarificationApplications.Count,
                ApplicationsClosed = closedApplications.Count
            });

            _handler = new FinancialApplicationsStatusCountsHandler(_applyRepository.Object);
        }

        [Test]
        public async Task StatusCounts_should_be_as_expected()
        {
            int expectedOpenCount = 2;
            int expectedClarificationCount = 1;
            int expectedClosedCount = 1;

            var request = new FinancialApplicationsStatusCountsRequest();
            var result = await _handler.Handle(request, new CancellationToken());

            result.Should().NotBeNull();
            result.ApplicationsOpen.Should().Be(expectedOpenCount);
            result.ApplicationsWithClarification.Should().Be(expectedClarificationCount);
            result.ApplicationsClosed.Should().Be(expectedClosedCount);
        }
    }
}
