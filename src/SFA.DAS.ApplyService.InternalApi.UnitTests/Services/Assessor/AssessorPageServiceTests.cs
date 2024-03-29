﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Moderator;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Application.Services.Assessor;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;
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
        private const string _middlePageId = RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ManagementHierarchy;
        private const string _lastPageId = RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ManagementHierarchy_Financial;

        private Mock<IMediator> _mediator;
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private Mock<IAssessorSequenceService> _assessorSequenceService;
        private AssessorLookupService _assessorLookupService;
        private AssessorPageService _assessorPageService;

        [SetUp]
        public void TestSetup()
        {
            _mediator = new Mock<IMediator>();
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _assessorSequenceService = new Mock<IAssessorSequenceService>();
            _assessorLookupService = new AssessorLookupService();
            _assessorPageService = new AssessorPageService(_mediator.Object, _qnaApiClient.Object, _assessorSequenceService.Object, _assessorLookupService);

            _mediator.Setup(x => x.Send(It.Is<GetBlindAssessmentOutcomeRequest>(r => r.ApplicationId == _applicationId && r.SequenceNumber == _sequenceNumber && r.SectionNumber == _sectionNumber), It.IsAny<CancellationToken>())).ReturnsAsync(new BlindAssessmentOutcome());

            _assessorSequenceService.Setup(x => x.IsValidSequenceNumber(It.IsAny<int>())).Returns(true);

            var finanicialSection = new ApplicationSection
            {
                ApplicationId = _applicationId,
                SequenceId = RoatpWorkflowSequenceIds.FinancialEvidence,
                SectionId = RoatpWorkflowSectionIds.FinancialEvidence.YourOrganisationsFinancialEvidence,
                QnAData = new QnAData
                {
                    Pages = new List<Page> { GenerateFinanicialQnAPage() }
                }
            };
            _qnaApiClient.Setup(x => x.GetSectionBySectionNo(finanicialSection.ApplicationId, finanicialSection.SequenceId, finanicialSection.SectionId)).ReturnsAsync(finanicialSection);

            var section = new ApplicationSection
            {
                ApplicationId = _applicationId,
                SequenceId = _sequenceNumber,
                SectionId = _sectionNumber,
                QnAData = new QnAData
                {
                    Pages = new List<Page> { GenerateQnAPage(_firstPageId), GenerateQnAPage(_middlePageId), GenerateQnAPage(_lastPageId) }
                }
            };

            _qnaApiClient.Setup(x => x.GetSectionBySectionNo(section.ApplicationId, section.SequenceId, section.SectionId)).ReturnsAsync(section);
            _qnaApiClient.Setup(x => x.SkipPageBySectionNo(section.ApplicationId, section.SequenceId, section.SectionId, _firstPageId)).ReturnsAsync(new SkipPageResponse { NextAction = NextAction.NextPage, NextActionId = _middlePageId });
            _qnaApiClient.Setup(x => x.SkipPageBySectionNo(section.ApplicationId, section.SequenceId, section.SectionId, _middlePageId)).ReturnsAsync(new SkipPageResponse { NextAction = NextAction.NextPage, NextActionId = _lastPageId });
            _qnaApiClient.Setup(x => x.SkipPageBySectionNo(section.ApplicationId, section.SequenceId, section.SectionId, _lastPageId)).ReturnsAsync(new SkipPageResponse { NextAction = NextAction.ReturnToSection });
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
            Assert.That(actualPage.NextPageId, Is.EqualTo(_middlePageId));
        }

        [Test]
        public async Task GetAssessorPage_when_first_page_gets_expected_page()
        {
            var actualPage = await _assessorPageService.GetPage(_applicationId, _sequenceNumber, _sectionNumber, _firstPageId);

            Assert.That(actualPage, Is.Not.Null);
            Assert.That(actualPage.PageId, Is.EqualTo(_firstPageId));
            Assert.That(actualPage.NextPageId, Is.EqualTo(_middlePageId));
        }

        [Test]
        public async Task GetAssessorPage_when_middle_page_gets_expected_page()
        {
            var actualPage = await _assessorPageService.GetPage(_applicationId, _sequenceNumber, _sectionNumber, _middlePageId);

            Assert.That(actualPage, Is.Not.Null);
            Assert.That(actualPage.PageId, Is.EqualTo(_middlePageId));
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

        [Test]
        public async Task GetAssessorPage_when_management_hierarchy_financial_page_and_has_blindassessor_record_gets_expected_page()
        {
            const int managementHierarchySequenceNumber = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining;
            const int managementHierarchySectionNumber = RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy;
            const string managementHierarchyFinancialPage = RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ManagementHierarchy_Financial;

            _mediator.Setup(x => x.Send(It.Is<GetBlindAssessmentOutcomeRequest>(r => r.ApplicationId == _applicationId && r.SequenceNumber == managementHierarchySequenceNumber
                        && r.SectionNumber == managementHierarchySectionNumber && r.PageId == managementHierarchyFinancialPage), It.IsAny<CancellationToken>())).ReturnsAsync(new BlindAssessmentOutcome());


            var actualPage = await _assessorPageService.GetPage(_applicationId, managementHierarchySequenceNumber, managementHierarchySectionNumber, managementHierarchyFinancialPage);

            Assert.That(actualPage, Is.Not.Null);
            Assert.That(actualPage.PageId, Is.EqualTo(managementHierarchyFinancialPage));
            CollectionAssert.IsNotEmpty(actualPage.Questions);
        }

        [Test]
        public async Task GetAssessorPage_when_management_hierarchy_financial_page_and_does_not_have_blindassessor_record_returns_null()
        {
            const int managementHierarchySequenceNumber = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining;
            const int managementHierarchySectionNumber = RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy;
            const string managementHierarchyFinancialPage = RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ManagementHierarchy_Financial;

            _mediator.Setup(x => x.Send(It.Is<GetBlindAssessmentOutcomeRequest>(r => r.ApplicationId == _applicationId && r.SequenceNumber == managementHierarchySequenceNumber
                        && r.SectionNumber == managementHierarchySectionNumber && r.PageId == managementHierarchyFinancialPage), It.IsAny<CancellationToken>())).ReturnsAsync(default (BlindAssessmentOutcome));


            var actualPage = await _assessorPageService.GetPage(_applicationId, managementHierarchySequenceNumber, managementHierarchySectionNumber, managementHierarchyFinancialPage);

            Assert.That(actualPage, Is.Null);
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

        private static Page GenerateFinanicialQnAPage()
        {
            return new Page
            {
                PageId = RoatpWorkflowPageIds.YourOrganisationsFinancialEvidence.FinancialEvidence_CompanyOrCharity,
                Questions = new List<Question>
                {
                    new Question
                    {
                        QuestionId = RoatpWorkflowQuestionTags.Turnover,
                        QuestionTag = RoatpWorkflowQuestionTags.Turnover,
                        QuestionBodyText = "QuestionBodyText",
                        Input = new Input { Type = "TextArea" }
                    },
                    new Question
                    {
                        QuestionId = RoatpWorkflowQuestionTags.AverageNumberofFTEEmployees,
                        QuestionTag = RoatpWorkflowQuestionTags.AverageNumberofFTEEmployees,
                        QuestionBodyText = "QuestionBodyText",
                        Input = new Input { Type = "TextArea" }
                    }
                },
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Answers = new List<Answer>
                        {
                            new Answer { QuestionId = RoatpWorkflowQuestionTags.Turnover, Value = "Value" },
                            new Answer { QuestionId = RoatpWorkflowQuestionTags.AverageNumberofFTEEmployees, Value = "Value" }
                        }
                    }
                }
            };
        }
    }
}
