using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;

namespace SFA.DAS.ApplyService.Web.UnitTests.Services
{
    [TestFixture]
    public class ResetQnaDetailsServiceTests
    {
        private ResetQnaDetailsService _service;
        private Mock<IQnaApiClient> _qnaApiClient;

        private Guid _applicationId;


        [SetUp]
        public void Before_each_test()
        {
            _applicationId = Guid.NewGuid();
            _qnaApiClient = new Mock<IQnaApiClient>();
        }

        [TestCase(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, RoatpWorkflowPageIds.ExperienceAndAccreditations.OfficeForStudents, true)]
        [TestCase(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, RoatpWorkflowPageIds.ExperienceAndAccreditations.InitialTeacherTraining, true)]
        [TestCase(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, RoatpWorkflowPageIds.ExperienceAndAccreditations.SubcontractorDeclaration, true)]
        [TestCase(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, RoatpWorkflowPageIds.ExperienceAndAccreditations.HasHadFullInspection, false)]
        [TestCase(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, RoatpWorkflowPageIds.ExperienceAndAccreditations.ReceivedFullInspectionGradeForApprenticeships, false)]
        [TestCase(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, RoatpWorkflowPageIds.ExperienceAndAccreditations.FullInspectionOverallEffectivenessGrade, false)]
        [TestCase(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, RoatpWorkflowPageIds.ExperienceAndAccreditations.HasHadMonitoringVisit, false)]
        [TestCase(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, RoatpWorkflowPageIds.ExperienceAndAccreditations.Has2MonitoringVisitsGradedInadequate, false)]
        [TestCase(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, RoatpWorkflowPageIds.ExperienceAndAccreditations.HasMonitoringVisitGradedInadequateInLast18Months, false)]
        [TestCase(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, RoatpWorkflowPageIds.ExperienceAndAccreditations.HasMaintainedFundingSinceInspection, false)]
        [TestCase(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, RoatpWorkflowPageIds.ExperienceAndAccreditations.HasHadShortInspectionWithinLast3Years, false)]
        [TestCase(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, RoatpWorkflowPageIds.ExperienceAndAccreditations.HasMaintainedFullGradeInShortInspection, false)]
        [TestCase(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, RoatpWorkflowPageIds.ExperienceAndAccreditations.FullInspectionApprenticeshipGradeNonOfsFunded, false)]
        [TestCase(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, RoatpWorkflowPageIds.ExperienceAndAccreditations.FullInspectionApprenticeshipGradeOfsFunded, false)]
        [TestCase(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, RoatpWorkflowPageIds.ExperienceAndAccreditations.GradeWithinLast3YearsOfsFunded, false)]
        [TestCase(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, RoatpWorkflowPageIds.ExperienceAndAccreditations.GradeWithinLast3YearsNonOfsFunded, false)]
        [TestCase(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, RoatpWorkflowPageIds.ExperienceAndAccreditations.IsPostGradTrainingOnlyApprenticeship, false)]
        [TestCase(RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, RoatpWorkflowPageIds.ExperienceAndAccreditations.SubcontractorContractFile, false)]
        [TestCase(RoatpWorkflowSequenceIds.Preamble, RoatpWorkflowSectionIds.Preamble, RoatpPreambleQuestionIdConstants.UKPRN, false)]
        [TestCase(RoatpWorkflowSequenceIds.FinancialEvidence, RoatpWorkflowSectionIds.FinancialEvidence.WhatYouWillNeed, RoatpWorkflowPageIds.YourOrganisationsFinancialEvidence.FinancialEvidence_CompanyOrCharity, false)]
        [TestCase(RoatpWorkflowSequenceIds.CriminalComplianceChecks, RoatpWorkflowSectionIds.CriminalComplianceChecks.WhatYouWillNeed, RoatpWorkflowPageIds.CriminalComplianceChecks.CompositionCreditors, false)]
        [TestCase(RoatpWorkflowSequenceIds.ProtectingYourApprentices, RoatpWorkflowSectionIds.ProtectingYourApprentices.WhatYouWillNeed, RoatpWorkflowPageIds.ProtectingYourApprentices.ContinuityPlan, false)]
        [TestCase(RoatpWorkflowSequenceIds.ReadinessToEngage, RoatpWorkflowSectionIds.ReadinessToEngage.WhatYouWillNeed, RoatpWorkflowPageIds.ReadinessToEngage.ComplaintsPolicy, false)]
        [TestCase(RoatpWorkflowSequenceIds.PlanningApprenticeshipTraining, RoatpWorkflowSectionIds.PlanningApprenticeshipTraining.WhatYouWillNeed, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.RecruitNewStaff, false)]
        [TestCase(RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining, RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.WhatYouWillNeed, RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ChooseYourOrganisationsSectors, false)]
        [TestCase(RoatpWorkflowSequenceIds.EvaluatingApprenticeshipTraining, RoatpWorkflowSectionIds.EvaluatingApprenticeshipTraining.WhatYouWillNeed, RoatpWorkflowPageIds.EvaluatingApprenticeshipTraining.CollectApprenticeshipData, false)]
        [TestCase(RoatpWorkflowSequenceIds.FinancialEvidence, RoatpWorkflowSectionIds.Finish.ApplicationPermissionsAndChecks, RoatpWorkflowPageIds.Finish.ApplicationPermissionsChecksShutterPage, false)]
        public async Task ResetPagesComplete_ValuesInsertedToCheckValidity_Verified(int sequenceNo, int sectionNo, string pageId, bool resetEndpointCalled)
        {
            _qnaApiClient.Setup(x => x.ResetSectionPagesIncomplete(_applicationId,It.IsAny<int>(),It.IsAny<int>(), It.IsAny<List<string>>())).Returns(Task.CompletedTask);
            _service = new ResetQnaDetailsService(_qnaApiClient.Object);
            await _service.ResetPagesComplete(_applicationId, sequenceNo,sectionNo, pageId);

            var responseCount = Times.Once();
            if (!resetEndpointCalled)
                responseCount = Times.Never();

            _qnaApiClient.Verify(x => x.ResetSectionPagesIncomplete(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<string>>()),responseCount);
        }
    }
}

