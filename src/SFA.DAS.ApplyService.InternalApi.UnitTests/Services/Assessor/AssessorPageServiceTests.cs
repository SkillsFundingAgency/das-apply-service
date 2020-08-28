using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Services.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Services.Assessor
{
    [TestFixture]
    public class AssessorPageServiceTests
    {

        private readonly Guid _applicationId = Guid.NewGuid();
        private const int _sequenceNumber = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining;
        private const int _sectionNumber = RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy;
        private const string _firstPageId = RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.OverallAccountability;
        private readonly string _lastPageId = RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ManagementHierarchy;

        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private Mock<IAssessorSequenceService> _assessorSequenceService;
        private AssessorLookupService _assessorLookupService;
        private AssessorPageService _assessorPageService;

        [SetUp]
        public void TestSetup()
        {
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _assessorSequenceService = new Mock<IAssessorSequenceService>();
            _assessorLookupService = new AssessorLookupService();
            _assessorPageService = new AssessorPageService(_qnaApiClient.Object, _assessorSequenceService.Object, _assessorLookupService);

            _assessorSequenceService.Setup(x => x.IsValidSequenceNumber(It.IsAny<int>())).Returns(true);

            var section = new ApplicationSection
            {
                ApplicationId = _applicationId,
                SequenceId = _sequenceNumber,
                SectionId = _sectionNumber,
                QnAData = new QnAData
                {
                    Pages = new List<Page> { GenerateQnAPage(_firstPageId), GenerateQnAPage(_lastPageId) }
                }
            };

            _qnaApiClient.Setup(x => x.GetSectionBySectionNo(section.ApplicationId, section.SequenceId, section.SectionId)).ReturnsAsync(section);
            _qnaApiClient.Setup(x => x.SkipPageBySectionNo(section.ApplicationId, section.SequenceId, section.SectionId, _firstPageId)).ReturnsAsync(new SkipPageResponse { NextAction = "NextPage", NextActionId = _lastPageId });
            _qnaApiClient.Setup(x => x.SkipPageBySectionNo(section.ApplicationId, section.SequenceId, section.SectionId, _lastPageId)).ReturnsAsync(new SkipPageResponse { NextAction = "ReturnToSection" });
        }

        [Test]
        public async Task GetPage_returns_null_if_invalid_sequence()
        {
            var invalidSequenceId = int.MinValue;
            _assessorSequenceService.Setup(x => x.IsValidSequenceNumber(invalidSequenceId)).Returns(true);

            var actualPage = await _assessorPageService.GetPage(_applicationId, invalidSequenceId, _sectionNumber, _firstPageId);
            Assert.That(actualPage, Is.Null);
        }

        [Test]
        public async Task GetPage_when_pageId_is_null_gets_first_page_in_section()
        {
            var actualPage = await _assessorPageService.GetPage(_applicationId, _sequenceNumber, _sectionNumber, null);

            Assert.That(actualPage, Is.Not.Null);
            Assert.That(actualPage.PageId, Is.EqualTo(_firstPageId));
            Assert.That(actualPage.NextPageId, Is.EqualTo(_lastPageId));
        }

        [Test]
        public async Task GetAssessorPage_when_first_page_gets_expected_page()
        {
            var actualPage = await _assessorPageService.GetPage(_applicationId, _sequenceNumber, _sectionNumber, _firstPageId);

            Assert.That(actualPage, Is.Not.Null);
            Assert.That(actualPage.PageId, Is.EqualTo(_firstPageId));
            Assert.That(actualPage.NextPageId, Is.EqualTo(_lastPageId));
        }

        [Test]
        public async Task GetAssessorPage_when_last_page_gets_expected_page()
        {
            var actualPage = await _assessorPageService.GetPage(_applicationId, _sequenceNumber, _sectionNumber, _lastPageId);

            Assert.That(actualPage, Is.Not.Null);
            Assert.That(actualPage.PageId, Is.EqualTo(_lastPageId));
            Assert.That(actualPage.NextPageId, Is.Null);
        }

        private static Page GenerateQnAPage(string pageId)
        {
            return new Page
            {
                PageId = pageId,
                Questions = new List<Question>
                {
                    new Question
                    {
                        QuestionId = $"Q{pageId}",
                        QuestionBodyText = "QuestionBodyText",
                        Input = new Input { Type = "TextArea" }
                    }
                },
                PageOfAnswers = new List<PageOfAnswers> { new PageOfAnswers { Answers = new List<Answer> { new Answer { QuestionId = $"Q{pageId}", Value = "Value" } } } }
            };
        }
    }
}
