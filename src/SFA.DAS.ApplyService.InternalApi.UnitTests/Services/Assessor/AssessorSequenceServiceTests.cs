using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Services.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Services.Assessor
{
    [TestFixture]
    public class AssessorSequenceServiceTests
    {
        private readonly Guid _applicationId = Guid.NewGuid();
        private Mock<IMediator> _mediator;
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private AssessorLookupService _lookupService;
        private AssessorSequenceService _sequenceService;

        [SetUp]
        public void TestSetup()
        {
            _mediator = new Mock<IMediator>();
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _lookupService = new AssessorLookupService();

            _sequenceService = new AssessorSequenceService(_mediator.Object, _qnaApiClient.Object, _lookupService);

            var application = new Apply
            {
                ApplicationId = _applicationId,
                ApplyData = new ApplyData
                {
                    Sequences = new List<ApplySequence>
                    {
                        new ApplySequence { SequenceNo = RoatpWorkflowSequenceIds.Preamble },
                        new ApplySequence { SequenceNo = RoatpWorkflowSequenceIds.YourOrganisation },
                        new ApplySequence { SequenceNo = RoatpWorkflowSequenceIds.FinancialEvidence },
                        new ApplySequence { SequenceNo = RoatpWorkflowSequenceIds.CriminalComplianceChecks },
                        new ApplySequence { SequenceNo = RoatpWorkflowSequenceIds.ProtectingYourApprentices },
                        new ApplySequence { SequenceNo = RoatpWorkflowSequenceIds.ReadinessToEngage },
                        new ApplySequence { SequenceNo = RoatpWorkflowSequenceIds.PlanningApprenticeshipTraining },
                        new ApplySequence { SequenceNo = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining },
                        new ApplySequence { SequenceNo = RoatpWorkflowSequenceIds.EvaluatingApprenticeshipTraining },
                        new ApplySequence { SequenceNo = RoatpWorkflowSequenceIds.Finish }
                    }
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
        }

        [TestCase(RoatpWorkflowSequenceIds.Preamble, false)]
        [TestCase(RoatpWorkflowSequenceIds.YourOrganisation, false)]
        [TestCase(RoatpWorkflowSequenceIds.FinancialEvidence, false)]
        [TestCase(RoatpWorkflowSequenceIds.CriminalComplianceChecks, false)]
        [TestCase(RoatpWorkflowSequenceIds.ProtectingYourApprentices, true)]
        [TestCase(RoatpWorkflowSequenceIds.ReadinessToEngage, true)]
        [TestCase(RoatpWorkflowSequenceIds.PlanningApprenticeshipTraining, true)]
        [TestCase(RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining, true)]
        [TestCase(RoatpWorkflowSequenceIds.EvaluatingApprenticeshipTraining, true)]
        [TestCase(RoatpWorkflowSequenceIds.Finish, false)]
        public void IsValidSequenceNumber_returns_expected_result(int sequenceNumber, bool expectedResult)
        {
            var actualResult = _sequenceService.IsValidSequenceNumber(sequenceNumber);

            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task GetSequences_returns_expected_list_of_sequences()
        {
            var expectedSequenceNumbers = new List<int>
                                        {
                                            RoatpWorkflowSequenceIds.ProtectingYourApprentices,
                                            RoatpWorkflowSequenceIds.ReadinessToEngage,
                                            RoatpWorkflowSequenceIds.PlanningApprenticeshipTraining,
                                            RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
                                            RoatpWorkflowSequenceIds.EvaluatingApprenticeshipTraining
                                        };

            var actualSequences = await _sequenceService.GetSequences(_applicationId);

            Assert.That(actualSequences, Is.Not.Null);
            Assert.That(actualSequences.Select(seq => seq.SequenceNumber), Is.EquivalentTo(expectedSequenceNumbers));
        }
    }
}
