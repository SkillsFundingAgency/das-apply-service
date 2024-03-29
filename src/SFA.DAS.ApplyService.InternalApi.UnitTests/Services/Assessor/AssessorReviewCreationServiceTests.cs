﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Services.Assessor;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Services.Assessor
{
    [TestFixture]
    public class AssessorReviewCreationServiceTests
    {
        private AssessorReviewCreationService _assessorReviewCreationService;
        private Mock<IAssessorSequenceService> _assessorSequenceService;
        private Mock<IAssessorSectorService> _assessorSectorService;
        private Mock<IMediator> _mediator;

        private List<AssessorSequence> _sequences;
        private List<AssessorSector> _sectors;

        private Guid _applicationId;
        private string _assessorUserId;
        private string _assessorUserName;
        private int _assessorNumber;

        [SetUp]
        public void Arrange()
        {
            _applicationId = Guid.NewGuid();
            _assessorUserId = "TestUser";
            _assessorUserName = "TestUserName";
            _assessorNumber = 1;

            _assessorSequenceService = new Mock<IAssessorSequenceService>();
            _mediator = new Mock<IMediator>();
            _assessorSectorService = new Mock<IAssessorSectorService>();

            var sectorsSection = new AssessorSection
            {
                LinkTitle = "Sectors Section",
                SectionNumber = RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees,
                SequenceNumber = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
                Pages = new List<Page>
                            {
                                new Page{ PageId = Guid.NewGuid().ToString(), DisplayType = SectionDisplayType.PagesWithSections, LinkTitle = "First Sector Starting Page", Active = true, Complete = true },
                                new Page{ PageId = Guid.NewGuid().ToString(), DisplayType = SectionDisplayType.Questions, LinkTitle = "First Sector Question Page", Active = true, Complete = true }
                            }
            };

            var managementHierarchySection = new AssessorSection
            {
                LinkTitle = "Management Hierarchy Section",
                SectionNumber = RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy,
                SequenceNumber = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
                Pages = new List<Page>
                            {
                                new Page{ PageId = RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.OverallAccountability, Active = true, Complete = true  },
                                new Page{ PageId = RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ManagementHierarchy, Active = true, Complete = true  }
                            }
            };

            _sequences = new List<AssessorSequence>
            {
                new AssessorSequence
                {
                    Id = Guid.NewGuid(),
                    SequenceNumber = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
                    SequenceTitle = "Delivering Apprenticeship Training Sequence",
                    Sections = new List<AssessorSection>
                    {
                        sectorsSection,
                        managementHierarchySection
                    }
                }
            };

            _assessorSequenceService.Setup(x => x.GetSequences(_applicationId)).ReturnsAsync(_sequences);

            _sectors = sectorsSection.Pages.Where(pg => pg.DisplayType == SectionDisplayType.PagesWithSections && pg.Active && pg.Complete)
                                               .Select(pg => new AssessorSector { PageId = pg.PageId, Title = pg.LinkTitle })
                                               .ToList();

            _assessorSectorService.Setup(x => x.GetSectorsForEmptyReview(sectorsSection)).Returns(_sectors);

            _assessorReviewCreationService = new AssessorReviewCreationService(_assessorSequenceService.Object, _assessorSectorService.Object, _mediator.Object);
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task CreateEmptyReview_creates_empty_review_outcomes(bool injectFinancialInformationPage)
        {
            _assessorSequenceService.Setup(x => x.ShouldInjectFinancialInformationPage(_applicationId)).ReturnsAsync(injectFinancialInformationPage);

            await _assessorReviewCreationService.CreateEmptyReview(_applicationId, _assessorUserId, _assessorUserName, _assessorNumber);

            var allSections = _sequences.SelectMany(seq => seq.Sections);
            var allSectionsExcludingSectors = allSections.Where(sec => sec.SequenceNumber != RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining || sec.SectionNumber != RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees);
            var sectionPages = allSectionsExcludingSectors.SelectMany(sec => sec.Pages).ToList();

            var expectedNumberOfOutcomes = sectionPages.Count + _sectors.Count;

            if (injectFinancialInformationPage) expectedNumberOfOutcomes += 1;

            _mediator.Verify(x =>
                    x.Send(It.Is<CreateEmptyAssessorReviewRequest>(r =>
                            r.PageReviewOutcomes.Count == expectedNumberOfOutcomes &&
                            r.PageReviewOutcomes.TrueForAll(y =>
                                _sectors.Exists(s => s.PageId == y.PageId) ||
                                sectionPages.Exists(p => p.PageId == y.PageId) ||
                                y.PageId == RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ManagementHierarchy_Financial)),
                        It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
