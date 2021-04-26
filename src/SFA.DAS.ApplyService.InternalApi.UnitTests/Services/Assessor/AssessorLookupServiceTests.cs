using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.InternalApi.Services.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Services.Assessor
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

        [TestCase(RoatpPlanningApprenticeshipTrainingQuestionIdConstants.ApplicationFrameworks_MainEmployer)]
        [TestCase(RoatpPlanningApprenticeshipTrainingQuestionIdConstants.ApplicationFrameworks_Supporting)]
        public void GetLabelForQuestion_when_valid_question_returns_title(string validQuestionId)
        {
            var actualLabel = _assessorLookupService.GetLabelForQuestion(validQuestionId);

            Assert.That(actualLabel, Is.Not.Null);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("invalid")]
        public void GetLabelForQuestion_when_invalid_question_returns_null(string invalidQuestionId)
        {
            var actualLabel = _assessorLookupService.GetLabelForQuestion(invalidQuestionId);

            Assert.That(actualLabel, Is.Null);
        }

        [Test]
        public void GetSectorIdsForSectorPageId()
        {
            var pageId = RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.AgricultureEnvironmentalAndAnimalCare.WhatStandardsOffered;
            var result = _assessorLookupService.GetSectorQuestionIdsForSectorPageId(pageId);

            Assert.AreEqual(
                Newtonsoft.Json.JsonConvert.SerializeObject(result)
                , Newtonsoft.Json.JsonConvert.SerializeObject(RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.AgricultureEnvironmentalAndAnimalCare.SectorQuestionIds));
          }

        [Test]
        public void GetSectorNameForSectorPageId()
        {
            var pageId = RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.AgricultureEnvironmentalAndAnimalCare.WhatStandardsOffered;
            var actualName = _assessorLookupService.GetSectorNameForPage(pageId);

            Assert.AreEqual(actualName, RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.AgricultureEnvironmentalAndAnimalCare.Name);
        }
    }
}
