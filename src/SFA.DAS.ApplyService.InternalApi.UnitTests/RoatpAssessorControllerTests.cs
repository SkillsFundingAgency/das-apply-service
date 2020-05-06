using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class RoatpAssessorControllerTests
    {
        private readonly Guid _applicationId = Guid.NewGuid();

        private Mock<ILogger<RoatpAssessorController>> _logger;
        private Mock<IMediator> _mediator;
        private Mock<IApplyRepository> _applyRepository;
        private Mock<IInternalQnaApiClient> _qnaApiClient;

        private RoatpAssessorController _controller;

        [SetUp]
        public void TestSetup()
        {
            _mediator = new Mock<IMediator>();
            _applyRepository = new Mock<IApplyRepository>();     
            _logger = new Mock<ILogger<RoatpAssessorController>>();
            _qnaApiClient = new Mock<IInternalQnaApiClient>();

            _controller = new RoatpAssessorController(_logger.Object, _mediator.Object, _applyRepository.Object, _qnaApiClient.Object);
        }

        [Test]
        public async Task Get_summary_returns_summary_for_the_user()
        {
            var expectedUser = "sadjkffgdji";
            var newApplications = 1;
            var inprogressApplications = 2;
            var moderationApplications = 3;
            var clarificationApplications = 4;
            var expectedResult = new RoatpAssessorSummary(newApplications, inprogressApplications, moderationApplications, clarificationApplications);
            _mediator.Setup(x => x.Send(It.Is<AssessorSummaryRequest>(y => y.UserId == expectedUser), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.AssessorSummary(expectedUser);

            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task Get_new_applications_returns_new_applications_for_the_user()
        {
            var expectedUser = "sadjkffgdji";
            var expectedResult = new List<RoatpAssessorApplicationSummary>();
            _mediator.Setup(x => x.Send(It.Is<NewAssessorApplicationsRequest>(y => y.UserId == expectedUser), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.NewApplications(expectedUser);

            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task Assign_application_sets_assessor_details()
        {
            var request = new AssignAssessorApplicationRequest { AssessorName = "sdfjfsdg", AssessorNumber = 1, AssessorUserId = "dsalkjfhjfdg" };
            var applicationid = Guid.NewGuid();

            await _controller.AssignApplication(applicationid, request);

            _mediator.Verify(x => x.Send(It.Is<AssignAssessorRequest>(r => r.ApplicationId == applicationid && r.AssessorName == request.AssessorName && r.AssessorNumber == request.AssessorNumber && r.AssessorUserId == request.AssessorUserId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Get_in_progress_applications_returns_in_progress_applications_for_the_user()
        {
            var expectedUser = "sadjkffgdji";
            var expectedResult = new List<RoatpAssessorApplicationSummary>();
            _mediator.Setup(x => x.Send(It.Is<InProgressAssessorApplicationsRequest>(y => y.UserId == expectedUser), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.InProgressApplications(expectedUser);

            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task GetAssessorOverview_gets_expected_sequences()
        {
            var allSections = new List<ApplicationSection>
            {
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.Preamble, SectionId = 1},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.YourOrganisation, SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.YourOrganisation, SectionId = RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.FinancialEvidence, SectionId = RoatpWorkflowSectionIds.FinancialEvidence.WhatYouWillNeed},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.FinancialEvidence, SectionId = RoatpWorkflowSectionIds.FinancialEvidence.YourOrganisationsFinancialEvidence},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.CriminalComplianceChecks, SectionId = RoatpWorkflowSectionIds.CriminalComplianceChecks.WhatYouWillNeed},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.CriminalComplianceChecks, SectionId = RoatpWorkflowSectionIds.CriminalComplianceChecks.ChecksOnYourOrganisation},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.ProtectingYourApprentices, SectionId = 1},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.ProtectingYourApprentices, SectionId = 2},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.ReadinessToEngage, SectionId = 1},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.ReadinessToEngage, SectionId = 2},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.PlanningApprenticeshipTraining, SectionId = 1},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.PlanningApprenticeshipTraining, SectionId = 2},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining, SectionId = 1},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining, SectionId = 2},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.EvaluatingApprenticeshipTraining, SectionId = 1},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.EvaluatingApprenticeshipTraining, SectionId = 2},
                new ApplicationSection {ApplicationId = _applicationId, SequenceId = RoatpWorkflowSequenceIds.Finish, SectionId = 1}
            };

            _qnaApiClient.Setup(x => x.GetAllApplicationSections(_applicationId)).ReturnsAsync(allSections);

            var expectedSequenceNumbers = new List<int>
                                        { 
                                            RoatpWorkflowSequenceIds.ProtectingYourApprentices,
                                            RoatpWorkflowSequenceIds.ReadinessToEngage,
                                            RoatpWorkflowSequenceIds.PlanningApprenticeshipTraining,
                                            RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
                                            RoatpWorkflowSequenceIds.EvaluatingApprenticeshipTraining
                                        };

            var actualSequences = await _controller.GetAssessorOverview(_applicationId);

            Assert.That(actualSequences, Is.Not.Null);
            Assert.That(actualSequences.Select(seq => seq.SequenceNumber), Is.EquivalentTo(expectedSequenceNumbers));
        }

        [Test]
        public async Task SubmitAssessorPageOutcome_calls_mediator()
        {
            var applicationId = Guid.NewGuid();
            var request = new SubmitAssessorPageOutcomeRequest { ApplicationId = applicationId, SequenceNumber = 1, SectionNumber = 2, PageId = "30", AssessorType = 2,
                                                                 UserId = "4fs7f-userId-7gfhh", Status = "Fail", Comment = "Very bad" };

            await _controller.SubmitAssessorPageOutcome(request);

            _mediator.Verify(x => x.Send(It.Is<SubmitAssessorPageOutcomeHandlerRequest>(r => r.ApplicationId == applicationId && r.SequenceNumber == request.SequenceNumber && r.SectionNumber == request.SectionNumber && 
                   r.PageId == request.PageId && r.AssessorType == request.AssessorType && r.UserId == request.UserId && r.Status == request.Status && r.Comment == request.Comment), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetPageReviewOutcome_returns_expected_PageReviewOutcome()
        {
            var expectedApplicationId = Guid.NewGuid();
            var expectedSequenceNumber = 1;
            var expectedSectionNumber = 2;
            var expectedPageId = "30";
            var expectedAssessorType = 2;
            var expectedUserId = "4fs7f-userId-7gfhh";

            var request = new GetPageReviewOutcomeRequest
            { 
                ApplicationId = expectedApplicationId,
                SequenceNumber = expectedSequenceNumber,
                SectionNumber = expectedSectionNumber,
                PageId = expectedPageId,
                AssessorType = expectedAssessorType,
                UserId = expectedUserId
            };

            var expectedResult = new PageReviewOutcome();
            _mediator.Setup(x => x.Send(It.Is<GetPageReviewOutcomeHandlerRequest>(r => r.ApplicationId == expectedApplicationId && r.SequenceNumber == expectedSequenceNumber &&
                        r.SectionNumber == expectedSectionNumber && r.PageId == expectedPageId && r.AssessorType == expectedAssessorType && 
                        r.UserId == expectedUserId), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetPageReviewOutcome(request);

            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task GetAssessorReviewOutcomesPerSection_returns_expected_list_of_PageReviewOutcome()
        {
            var expectedApplicationId = Guid.NewGuid();
            var expectedSequenceNumber = 1;
            var expectedSectionNumber = 2;
            var expectedAssessorType = 2;
            var expectedUserId = "4fs7f-userId-7gfhh";

            var request = new GetAssessorReviewOutcomesPerSectionRequest
            {
                ApplicationId = expectedApplicationId,
                SequenceNumber = expectedSequenceNumber,
                SectionNumber = expectedSectionNumber,
                AssessorType = expectedAssessorType,
                UserId = expectedUserId
            };

            var expectedResult = new List<PageReviewOutcome>();
            _mediator.Setup(x => x.Send(It.Is<GetAssessorReviewOutcomesPerSectionHandlerRequest>(r => r.ApplicationId == expectedApplicationId && r.SequenceNumber == expectedSequenceNumber &&
                        r.SectionNumber == expectedSectionNumber && r.AssessorType == expectedAssessorType &&
                        r.UserId == expectedUserId), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetAssessorReviewOutcomesPerSection(request);

            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task GetAllAssessorReviewOutcomes_returns_expected_list_of_PageReviewOutcome()
        {
            var expectedApplicationId = Guid.NewGuid();
            var expectedAssessorType = 2;
            var expectedUserId = "4fs7f-userId-7gfhh";

            var request = new GetAllAssessorReviewOutcomesRequest
            {
                ApplicationId = expectedApplicationId,
                AssessorType = expectedAssessorType,
                UserId = expectedUserId
            };

            var expectedResult = new List<PageReviewOutcome>();
            _mediator.Setup(x => x.Send(It.Is<GetAllAssessorReviewOutcomesHandlerRequest>(r => r.ApplicationId == expectedApplicationId && 
                        r.AssessorType == expectedAssessorType && r.UserId == expectedUserId), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetAllAssessorReviewOutcomes(request);

            Assert.AreSame(expectedResult, actualResult);
        }
    }
}
