﻿using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Application.Apply.Moderator;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Services;
using SFA.DAS.ApplyService.InternalApi.Types.Moderator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class RoatpModerationControllerTests
    {
        private readonly Guid _applicationId = Guid.NewGuid();
        private readonly string _userId = "user id";

        private Mock<ILogger<RoatpModerationController>> _logger;
        private Mock<IMediator> _mediator;
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private Mock<IAssessorLookupService> _assessorLookupService;

        private RoatpModerationController _controller;

        [SetUp]
        public void TestSetup()
        {
            _logger = new Mock<ILogger<RoatpModerationController>>();
            _mediator = new Mock<IMediator>();
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _assessorLookupService = new Mock<IAssessorLookupService>();

            _controller = new RoatpModerationController(_logger.Object, _mediator.Object, _qnaApiClient.Object, _assessorLookupService.Object);
        }

        [Test]
        public async Task GetModeratorOverview_gets_expected_sequences()
        {
            var application = new Apply
            {
                ApplicationId = _applicationId,
                ApplyData = new ApplyData
                {
                    Sequences = new List<ApplySequence>()
                }
            };

            _mediator.Setup(x => x.Send(It.Is<GetApplicationRequest>(y => y.ApplicationId == _applicationId), It.IsAny<CancellationToken>())).ReturnsAsync(application);

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

            var actualSequences = await _controller.GetModeratorOverview(_applicationId);

            Assert.That(actualSequences, Is.Not.Null);
            Assert.That(actualSequences.Select(seq => seq.SequenceNumber), Is.EquivalentTo(expectedSequenceNumbers));
        }

        [Test]
        public async Task GetPageReviewOutcome_returns_expected_PageReviewOutcome()
        {
            var applicationId = Guid.NewGuid();
            var request = new RoatpModerationController.GetPageReviewOutcomeRequest { SequenceNumber = 1, SectionNumber = 2, PageId = "30", UserId = _userId };

            var expectedResult = new ModeratorPageReviewOutcome();
            _mediator.Setup(x => x.Send(It.Is<GetModeratorPageReviewOutcomeRequest>(r => r.ApplicationId == applicationId && r.SequenceNumber == request.SequenceNumber && r.SectionNumber == request.SectionNumber &&
                   r.PageId == request.PageId && r.UserId == request.UserId), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetPageReviewOutcome(applicationId, request);

            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task GetPageReviewOutcomesForSection_returns_expected_list_of_PageReviewOutcome()
        {
            var applicationId = Guid.NewGuid();
            var request = new RoatpModerationController.GetPageReviewOutcomesForSectionRequest { SequenceNumber = 1, SectionNumber = 2, UserId = _userId };

            var expectedResult = new List<ModeratorPageReviewOutcome>();
            _mediator.Setup(x => x.Send(It.Is<GetModeratorPageReviewOutcomesForSectionRequest>(r => r.ApplicationId == applicationId && r.SequenceNumber == request.SequenceNumber && r.SectionNumber == request.SectionNumber &&
                   r.UserId == request.UserId), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetPageReviewOutcomesForSection(applicationId, request);

            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task GetAllPageReviewOutcomes_returns_expected_list_of_PageReviewOutcome()
        {
            var applicationId = Guid.NewGuid();
            var request = new RoatpModerationController.GetAllPageReviewOutcomesRequest { UserId = _userId };

            var expectedResult = new List<ModeratorPageReviewOutcome>();
            _mediator.Setup(x => x.Send(It.Is<GetAllModeratorPageReviewOutcomesRequest>(r => r.ApplicationId == applicationId &&
                        r.UserId == request.UserId), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetAllPageReviewOutcomes(applicationId, request);

            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task GetSectors_for_application()
        {
            var sector1PageId = "Sector1PageId";
            var sector2PageId = "Sector2PageId";
            var sector1Title = "Sector 1 Title";
            var sector2Title = "Sector 2 Title";
            var sector1Status = "Pass";
            var sector2Status = "Fail";

            var pageSector1Page1 = new Page
            {
                PageId = sector1PageId,
                DisplayType = SectionDisplayType.PagesWithSections,
                LinkTitle = sector1Title,
                Active = true,
                Complete = true,
                NotRequired = false
            };


            var pageSector1Page2WrongType = new Page
            {
                PageId = "Sector1PageId",
                DisplayType = SectionDisplayType.OtherPagesInPagesWithSections,
                LinkTitle = "Sector 1 page 2 title",
                Active = true,
                Complete = true,
                NotRequired = false
            };

            var pageSector2Page1 = new Page
            {
                PageId = sector2PageId,
                DisplayType = SectionDisplayType.PagesWithSections,
                LinkTitle = sector2Title,
                Active = true,
                Complete = true,
                NotRequired = false
            };

            var pageSector3Page1Inactive = new Page
            {
                PageId = "Sector3PageId",
                DisplayType = SectionDisplayType.PagesWithSections,
                LinkTitle = "Sector 3 title",
                Active = false,
                Complete = true,
                NotRequired = false
            };

            var pageSector4Page1Incomplete = new Page
            {
                PageId = "Sector4PageId",
                DisplayType = SectionDisplayType.PagesWithSections,
                LinkTitle = "Sector 4 title",
                Active = true,
                Complete = false,
                NotRequired = false
            };

            var pageSector5Page1NotRequired = new Page
            {
                PageId = "Sector5PageId",
                DisplayType = SectionDisplayType.PagesWithSections,
                LinkTitle = "Sector 5 title",
                Active = true,
                Complete = true,
                NotRequired = true
            };

            var section = new ApplicationSection
            {
                ApplicationId = _applicationId,
                SequenceId = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
                SectionId = RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees,
                QnAData = new QnAData
                {
                    Pages = new List<Page> { pageSector1Page1,
                        pageSector1Page2WrongType,
                        pageSector2Page1,
                        pageSector3Page1Inactive,
                        pageSector4Page1Incomplete,
                        pageSector5Page1NotRequired }
                }
            };

            _qnaApiClient.Setup(x => x.GetSectionBySectionNo(_applicationId,
                RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
                RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees)).ReturnsAsync(section);

            var _sectionStatuses = new List<ModeratorPageReviewOutcome>();
            _sectionStatuses.Add(new ModeratorPageReviewOutcome
            {
                ApplicationId = _applicationId,
                PageId = sector1PageId,
                Status = sector1Status
            });
            _sectionStatuses.Add(new ModeratorPageReviewOutcome
            {
                ApplicationId = _applicationId,
                PageId = sector2PageId,
                Status = sector2Status
            });

            var request = new RoatpModerationController.GetSectorsRequest { UserId = _userId };

            _mediator.Setup(x => x.Send(It.Is<GetModeratorPageReviewOutcomesForSectionRequest>(y => y.ApplicationId == _applicationId && y.SequenceNumber == RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining
            && y.SectionNumber == RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees && y.UserId == request.UserId), It.IsAny<CancellationToken>())).ReturnsAsync(_sectionStatuses);

            var listOfSectors = await _controller.GetSectors(_applicationId, request);

            var expectedListOfSectors = new List<ModeratorSector>
            {
                new ModeratorSector {PageId = sector1PageId, Title = sector1Title, Status = sector1Status},
                new ModeratorSector {PageId = sector2PageId, Title = sector2Title, Status = sector2Status}
            };

            Assert.AreEqual(JsonConvert.SerializeObject(expectedListOfSectors), JsonConvert.SerializeObject(listOfSectors));
        }
    }
}