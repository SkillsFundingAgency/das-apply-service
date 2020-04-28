using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class RoatpAssessorControllerTests
    {
        private readonly Guid _applicationId = Guid.NewGuid();

        private Mock<ILogger<RoatpAssessorController>> _logger;
        private Mock<IApplyRepository> _applyRepository;
        private Mock<IInternalQnaApiClient> _qnaApiClient;

        private RoatpAssessorController _controller;

        [SetUp]
        public void Setup()
        {
            _applyRepository = new Mock<IApplyRepository>();     
            _logger = new Mock<ILogger<RoatpAssessorController>>();

            SetupQnaApiClient();

            _controller = new RoatpAssessorController(_logger.Object, _applyRepository.Object, _qnaApiClient.Object);
        }

        private void SetupQnaApiClient()
        {
            _qnaApiClient = new Mock<IInternalQnaApiClient>();

            var sections = new List<ApplicationSection>
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

            _qnaApiClient.Setup(x => x.GetSections(_applicationId)).ReturnsAsync(sections);
        }

        [Test]
        public async Task GetAssessorOverview_gets_expected_sequences()
        {
            var expectedSequences = new List<int>
                                        { 
                                            RoatpWorkflowSequenceIds.ProtectingYourApprentices,
                                            RoatpWorkflowSequenceIds.ReadinessToEngage,
                                            RoatpWorkflowSequenceIds.PlanningApprenticeshipTraining,
                                            RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
                                            RoatpWorkflowSequenceIds.EvaluatingApprenticeshipTraining
                                        };

            var actualSequences = await _controller.GetAssessorOverview(_applicationId);

            Assert.That(actualSequences, Is.Not.Null);
            Assert.That(actualSequences.Select(seq => seq.SequenceNumber), Is.EquivalentTo(expectedSequences));
        }
    }
}
