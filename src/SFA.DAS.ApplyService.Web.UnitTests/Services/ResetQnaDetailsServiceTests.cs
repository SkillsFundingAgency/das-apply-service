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
    public class ResetCompleteFlagServiceTests
    {
        private ResetCompleteFlagService _service;
        private Mock<IQnaApiClient> _qnaApiClient;

        private Guid _applicationId;


        [SetUp]
        public void Before_each_test()
        {
            _applicationId = Guid.NewGuid();
            _qnaApiClient = new Mock<IQnaApiClient>();
        }

        [TestCase( RoatpWorkflowPageIds.ExperienceAndAccreditations.OfficeForStudents, TimesCalled.Once)]
        [TestCase( RoatpWorkflowPageIds.ExperienceAndAccreditations.InitialTeacherTraining, TimesCalled.Once)]
        [TestCase( RoatpWorkflowPageIds.ExperienceAndAccreditations.SubcontractorDeclaration, TimesCalled.Once)]
        [TestCase( RoatpWorkflowPageIds.ExperienceAndAccreditations.HasHadFullInspection, TimesCalled.Never)]
        [TestCase( RoatpWorkflowPageIds.ExperienceAndAccreditations.ReceivedFullInspectionGradeForApprenticeships, TimesCalled.Never)]
        [TestCase( RoatpWorkflowPageIds.ExperienceAndAccreditations.FullInspectionOverallEffectivenessGrade, TimesCalled.Never)]
        [TestCase( RoatpWorkflowPageIds.ExperienceAndAccreditations.HasHadMonitoringVisit, TimesCalled.Never)]
        [TestCase( RoatpWorkflowPageIds.ExperienceAndAccreditations.Has2MonitoringVisitsGradedInadequate, TimesCalled.Never)]
        [TestCase( RoatpWorkflowPageIds.ExperienceAndAccreditations.HasMonitoringVisitGradedInadequateInLast18Months, TimesCalled.Never)]
        [TestCase( RoatpWorkflowPageIds.ExperienceAndAccreditations.HasMaintainedFundingSinceInspection, TimesCalled.Never)]
        [TestCase( RoatpWorkflowPageIds.ExperienceAndAccreditations.HasHadShortInspectionWithinLast3Years, TimesCalled.Never)]
        [TestCase( RoatpWorkflowPageIds.ExperienceAndAccreditations.HasMaintainedFullGradeInShortInspection, TimesCalled.Never)]
        [TestCase( RoatpWorkflowPageIds.ExperienceAndAccreditations.FullInspectionApprenticeshipGradeNonOfsFunded, TimesCalled.Never)]
        [TestCase( RoatpWorkflowPageIds.ExperienceAndAccreditations.FullInspectionApprenticeshipGradeOfsFunded, TimesCalled.Never)]
        [TestCase( RoatpWorkflowPageIds.ExperienceAndAccreditations.GradeWithinLast3YearsOfsFunded, TimesCalled.Never)]
        [TestCase( RoatpWorkflowPageIds.ExperienceAndAccreditations.GradeWithinLast3YearsNonOfsFunded, TimesCalled.Never)]
        [TestCase( RoatpWorkflowPageIds.ExperienceAndAccreditations.IsPostGradTrainingOnlyApprenticeship, TimesCalled.Never)]
        [TestCase( RoatpWorkflowPageIds.ExperienceAndAccreditations.SubcontractorContractFile, TimesCalled.Never)]
        [TestCase(RoatpPreambleQuestionIdConstants.UKPRN, TimesCalled.Never)]
        [TestCase(RoatpWorkflowPageIds.YourOrganisationsFinancialEvidence.FinancialEvidence_CompanyOrCharity, TimesCalled.Never)]
        [TestCase( RoatpWorkflowPageIds.CriminalComplianceChecks.CompositionCreditors, TimesCalled.Never)]
        [TestCase( RoatpWorkflowPageIds.ProtectingYourApprentices.ContinuityPlan, TimesCalled.Never)]
        [TestCase( RoatpWorkflowPageIds.ReadinessToEngage.ComplaintsPolicy, TimesCalled.Never)]
        [TestCase(RoatpWorkflowPageIds.PlanningApprenticeshipTraining.RecruitNewStaff, TimesCalled.Never)]
        [TestCase( RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ChooseYourOrganisationsSectors, TimesCalled.Never)]
        [TestCase(RoatpWorkflowPageIds.EvaluatingApprenticeshipTraining.CollectApprenticeshipData, TimesCalled.Never)]
        [TestCase( RoatpWorkflowPageIds.Finish.ApplicationPermissionsChecksShutterPage, TimesCalled.Never)]
        public async Task ResetPagesComplete_ValuesInsertedToCheckValidity_Verified( string pageId, TimesCalled timesCalled)
        {
            _qnaApiClient.Setup(x => x.ResetCompleteFlag(_applicationId,It.IsAny<int>(),It.IsAny<int>(), It.IsAny<List<string>>())).Returns(Task.CompletedTask);
            _service = new ResetCompleteFlagService(_qnaApiClient.Object);
            await _service.ResetPagesComplete(_applicationId,  pageId);

            var responseCount = Moq.Times.Once();
            if (timesCalled == TimesCalled.Never)
                responseCount = Moq.Times.Never();

            _qnaApiClient.Verify(x => x.ResetCompleteFlag(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<string>>()),responseCount);
        }
    }

    public enum TimesCalled
    {
        Never = 0,
        Once = 1
    }
}

