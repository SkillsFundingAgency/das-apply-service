using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.InternalApi.Services;
using System.Linq;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Services
{
    [TestFixture]
    public class AssessorLookupServiceTests
    {
        private AssessorLookupService _assessorLookupService;

        [SetUp]
        public void TestSetup()
        {
            _assessorLookupService = new AssessorLookupService();
        }

        [TestCase(RoatpWorkflowSequenceIds.ProtectingYourApprentices)]
        [TestCase(RoatpWorkflowSequenceIds.ReadinessToEngage)]
        [TestCase(RoatpWorkflowSequenceIds.PlanningApprenticeshipTraining)]
        public void GetTitleForSequence_when_valid_sequence_returns_title(int validSequenceId)
        {
            var actualTitle = _assessorLookupService.GetTitleForSequence(validSequenceId);

            Assert.That(actualTitle, Is.Not.Null);
        }

        [TestCase(-123)]
        [TestCase(-10)]
        [TestCase(10)]
        [TestCase(123)]
        public void GetTitleForSequence_when_invalid_sequence_returns_null(int invalidSequenceId)
        {
            var actualTitle = _assessorLookupService.GetTitleForSequence(invalidSequenceId);

            Assert.That(actualTitle, Is.Null);
        }

        [TestCase(RoatpWorkflowPageIds.ProtectingYourApprentices.ContinuityPlan)]
        [TestCase(RoatpWorkflowPageIds.ReadinessToEngage.EngagedWithEmployers)]
        [TestCase(RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Main)]
        public void GetTitleForPage_when_valid_page_returns_title(string validPageId)
        {
            var actualTitle = _assessorLookupService.GetTitleForPage(validPageId);

            Assert.That(actualTitle, Is.Not.Null);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("invalid")]
        public void GetTitleForPage_when_invalid_page_returns_null(string invalidPageId)
        {
            var actualTitle = _assessorLookupService.GetTitleForPage(invalidPageId);

            Assert.That(actualTitle, Is.Null);
        }
    }
}
