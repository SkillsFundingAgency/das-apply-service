using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.WindowsAzure.Storage.Core;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Services.Assessor;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.UnitTests.Services
{
    [TestFixture]
    public class OverallOutcomeAugmentationServiceTests
    {
        private Mock<IApplicationApiClient> _apiClient;
        private Mock<IQnaApiClient> _qnaApiClient;
        private Mock<IAssessorLookupService> _assessorLookupService;
        private Guid _applicationId;
        private ApplicationSummaryWithModeratorDetailsViewModel _model;
        private string _userId;
        private OverallOutcomeAugmentationService _service;
        private string _page121;
        private string _question121Id;
        private string _page122BodyText;
        private string _page121QuestionBodyText;
        private string _page122;

        [SetUp]
        public void Before_each_test()
        {
            _applicationId = Guid.NewGuid();
            _qnaApiClient = new Mock<IQnaApiClient>();
            _apiClient = new Mock<IApplicationApiClient>();
            _assessorLookupService = new Mock<IAssessorLookupService>();
            _userId = "test";
            _page121 = "page1.2.1";
            _page122 = "page1.2.2";
            _question121Id = "121Id";
            _page121QuestionBodyText = "page 1 2 1 Question Body text used in guidance";
            _page122BodyText = "page 1 2 2 Body text";
            var sequences = new List<AssessorSequence>();

            var clarificationOutcomes = new List<ClarificationPageReviewOutcome>();
            var sections = new List<ApplicationSection>();
            _apiClient.Setup(x => x.GetClarificationSequences(_applicationId)).ReturnsAsync(sequences);
            _apiClient.Setup(x => x.GetAllClarificationPageReviewOutcomes(_applicationId, _userId))
                .ReturnsAsync(clarificationOutcomes);
            _qnaApiClient.Setup(x => x.GetSections(_applicationId)).ReturnsAsync(sections);
            _service = new OverallOutcomeAugmentationService(_apiClient.Object, _qnaApiClient.Object,
                _assessorLookupService.Object);


        }


        [Test]
        public async Task no_failed_moderator_question_returns_model_unchanged()
        {
            var modelPreUpdate = GetCopyOfModel();
            var modelToBeUpdated = GetCopyOfModel();

            await _service.AugmentModelWithModerationFailDetails(modelToBeUpdated, _userId);

            modelPreUpdate.Should().BeEquivalentTo(modelToBeUpdated);
        }

        [TestCase(true, true, 2, 3, 7)]
        [TestCase(false, true, 2, 3, 4)]
        [TestCase(true, false, 1, 1, 3)]
        public async Task failed_moderator_questions_returns_model_with_inactive_sequences_sections_pages_removed(
            bool pageActive, bool sequence2PagesActive, int countOfSequences, int countOfSections, int countOfPages)
        {
            var modelPreUpdate = GetCopyOfModel();
            var modelToBeUpdated = GetCopyOfModel();

            var sequences = SetUpAsessorSequences();
            _apiClient.Setup(x => x.GetClarificationSequences(_applicationId)).ReturnsAsync(sequences);

            var clarificationPages = SetUpClarificationOutcomes();
            _apiClient.Setup(x => x.GetAllClarificationPageReviewOutcomes(_applicationId, _userId))
                .ReturnsAsync(clarificationPages);
            var sections = SetUpApplicationSections(pageActive, sequence2PagesActive, "answer",null);

            _qnaApiClient.Setup(x => x.GetSections(_applicationId)).ReturnsAsync(sections);

            await _service.AugmentModelWithModerationFailDetails(modelToBeUpdated, _userId);

            var numberOfSequences = modelToBeUpdated.Sequences.Count;
            var numberOfSections = 0;
            var numberOfPages = 0;
            foreach (var sequence in modelToBeUpdated.Sequences)
            {
                numberOfSections += sequence.Sections.Count;
                foreach (var section in sequence.Sections)
                {
                    numberOfPages += section.Pages.Count;
                }
            }

            numberOfSequences.Should().Be(countOfSequences);
            numberOfSections.Should().Be(countOfSections);
            numberOfPages.Should().Be(countOfPages);
        }


        [TestCase("page title")]
        [TestCase("page title 2")]
        public async Task page_titles_set_up_as_expected(string pageTitle)
        {
            var modelToBeUpdated = GetCopyOfModel();

            _assessorLookupService.Setup(x => x.GetTitleForPage(_page121)).Returns(pageTitle);

            var sequences = SetUpAsessorSequences();
            _apiClient.Setup(x => x.GetClarificationSequences(_applicationId)).ReturnsAsync(sequences);

            var clarificationPages = SetUpClarificationOutcomes();
            _apiClient.Setup(x => x.GetAllClarificationPageReviewOutcomes(_applicationId, _userId))
                .ReturnsAsync(clarificationPages);
            var sections = SetUpApplicationSections(true, true, "answer",null);

            _qnaApiClient.Setup(x => x.GetSections(_applicationId)).ReturnsAsync(sections);

            await _service.AugmentModelWithModerationFailDetails(modelToBeUpdated, _userId);


            var page121Title = modelToBeUpdated.Sequences.First(x => x.SequenceNumber == 1).Sections
                .FirstOrDefault(x => x.SectionNumber == 2).Pages.First(x => x.PageId == _page121).Title;

           pageTitle.Should().Be(page121Title);
        }


        [TestCase("page title 3")]
        [TestCase("page title 4")]
        public async Task page_titles_returned_against_sector_if_not_set_as_expected(string pageTitle)
        {
            var modelToBeUpdated = GetCopyOfModel();

            _assessorLookupService.Setup(x => x.GetTitleForPage(_page121)).Returns(string.Empty);
            _assessorLookupService.Setup(x => x.GetSectorNameForPage(_page121)).Returns(pageTitle);

            var sequences = SetUpAsessorSequences();
            _apiClient.Setup(x => x.GetClarificationSequences(_applicationId)).ReturnsAsync(sequences);

            var clarificationPages = SetUpClarificationOutcomes();
            _apiClient.Setup(x => x.GetAllClarificationPageReviewOutcomes(_applicationId, _userId))
                .ReturnsAsync(clarificationPages);
            var sections = SetUpApplicationSections(true, true, "answer",null);

            _qnaApiClient.Setup(x => x.GetSections(_applicationId)).ReturnsAsync(sections);

            await _service.AugmentModelWithModerationFailDetails(modelToBeUpdated, _userId);


            var page121Title = modelToBeUpdated.Sequences.First(x => x.SequenceNumber == 1).Sections
                .FirstOrDefault(x => x.SectionNumber == 2).Pages.First(x => x.PageId == _page121).Title;

            pageTitle.Should().Be(page121Title);
        }

        [TestCase("answer 1")]
        [TestCase("answer 1b")]
        [TestCase("answer 1c")]
        public async Task page_answers_returned_as_expected(string answer)
        {
            var modelToBeUpdated = GetCopyOfModel();


            var sequences = SetUpAsessorSequences();
            _apiClient.Setup(x => x.GetClarificationSequences(_applicationId)).ReturnsAsync(sequences);

            var clarificationPages = SetUpClarificationOutcomes();
            _apiClient.Setup(x => x.GetAllClarificationPageReviewOutcomes(_applicationId, _userId))
                .ReturnsAsync(clarificationPages);
            var sections = SetUpApplicationSections(true, true,answer,"question");

            _qnaApiClient.Setup(x => x.GetSections(_applicationId)).ReturnsAsync(sections);

            await _service.AugmentModelWithModerationFailDetails(modelToBeUpdated, _userId);

            var page121Answer = modelToBeUpdated.Sequences.First(x => x.SequenceNumber == 1).Sections
                .FirstOrDefault(x => x.SectionNumber == 2).Pages.First(x => x.PageId == _page121).PageOfAnswers.FirstOrDefault().Answers.FirstOrDefault().Value;

            answer.Should().Be(page121Answer);
        }


        [TestCase("question 1 text")]
        [TestCase("answer 1b text")]
        [TestCase("answer 1c text")]
        public async Task page_questions_returned_as_expected(string questionText)
        {
            var modelToBeUpdated = GetCopyOfModel();


            var sequences = SetUpAsessorSequences();
            _apiClient.Setup(x => x.GetClarificationSequences(_applicationId)).ReturnsAsync(sequences);

            var clarificationPages = SetUpClarificationOutcomes();
            _apiClient.Setup(x => x.GetAllClarificationPageReviewOutcomes(_applicationId, _userId))
                .ReturnsAsync(clarificationPages);
            var sections = SetUpApplicationSections(true, true, "answer", questionText);

            _qnaApiClient.Setup(x => x.GetSections(_applicationId)).ReturnsAsync(sections);

            await _service.AugmentModelWithModerationFailDetails(modelToBeUpdated, _userId);

            var page121Question = modelToBeUpdated.Sequences.First(x => x.SequenceNumber == 1).Sections
                .FirstOrDefault(x => x.SectionNumber == 2).Pages.First(x => x.PageId == _page121).Questions.FirstOrDefault(x=>x.QuestionId==_question121Id).Value;

            questionText.Should().Be(page121Question);
        }



        [TestCase("sequence 1 title", "sequence 2 title")]
        [TestCase("sequence 1 title alt", "sequence 2 title alt")]
        public async Task sequence_title_returned_as_expected(string sequence1Title, string  sequence2Title)
        {
            var modelToBeUpdated = GetCopyOfModel();


            var sequences = SetUpAsessorSequences();
            _apiClient.Setup(x => x.GetClarificationSequences(_applicationId)).ReturnsAsync(sequences);

            var clarificationPages = SetUpClarificationOutcomes();
            _apiClient.Setup(x => x.GetAllClarificationPageReviewOutcomes(_applicationId, _userId))
                .ReturnsAsync(clarificationPages);
            var sections = SetUpApplicationSections(true, true, "answer", "question");

            _qnaApiClient.Setup(x => x.GetSections(_applicationId)).ReturnsAsync(sections);

            var sequence1Number = 1;
            var sequence2Number = 2;
            _assessorLookupService.Setup(x=>x.GetTitleForSequence(sequence1Number)).Returns(sequence1Title);
            _assessorLookupService.Setup(x => x.GetTitleForSequence(sequence2Number)).Returns(sequence2Title);
            await _service.AugmentModelWithModerationFailDetails(modelToBeUpdated, _userId);

            sequence1Title.Should().Be(modelToBeUpdated.Sequences.FirstOrDefault(x=>x.SequenceNumber==sequence1Number).SequenceTitle);
            sequence2Title.Should().Be(modelToBeUpdated.Sequences.First(x=>x.SequenceNumber==sequence2Number).SequenceTitle);
        }


        [Test]
        public async Task guidance_text_returned_as_expected()
        {
            var modelToBeUpdated = GetCopyOfModel();

            var sequences = SetUpAsessorSequences();
            _apiClient.Setup(x => x.GetClarificationSequences(_applicationId)).ReturnsAsync(sequences);

            var clarificationPages = SetUpClarificationOutcomes();
            _apiClient.Setup(x => x.GetAllClarificationPageReviewOutcomes(_applicationId, _userId))
                .ReturnsAsync(clarificationPages);
            var sections = SetUpApplicationSections(true, true, null, null);

            _qnaApiClient.Setup(x => x.GetSections(_applicationId)).ReturnsAsync(sections);

            await _service.AugmentModelWithModerationFailDetails(modelToBeUpdated, _userId);

            _page121QuestionBodyText.Should().Be(modelToBeUpdated.PagesWithGuidance.First(x=>x.PageId==_page121).GuidanceInformation.FirstOrDefault());
            _page122BodyText.Should().Be(modelToBeUpdated.PagesWithGuidance.First(x => x.PageId == _page122).GuidanceInformation.FirstOrDefault());
        }

        private List<AssessorSequence> SetUpAsessorSequences()
    {

        var section2_3 = new AssessorSection
        {
            LinkTitle = "Section 2.3",
            SequenceNumber = 2,
            SectionNumber = 3,

            Pages = new List<Page>
            {
                new Page
                {
                    PageId = "Page2.3.1", DisplayType = SectionDisplayType.Questions,
                    LinkTitle = "2.3 Starting Page",
                    Active = true, Complete = true
                },
                new Page
                {
                    PageId = "Page2.3.2", DisplayType = SectionDisplayType.Questions, LinkTitle = "2.3 page 2",
                    Active = true, Complete = true
                }
            }
        };

        var section2_4 = new AssessorSection
        {
            LinkTitle = "Section 2.4",
            SequenceNumber = 2,
            SectionNumber = 4,
            Pages = new List<Page>
            {
                new Page {PageId = "Page2.4.1", Active = true, Complete = true},
                new Page {PageId = "Page2.4.2", Active = true, Complete = false}
            }
        };

        var section1_2 = new AssessorSection
        {
            LinkTitle = "Section 1.2",
            SequenceNumber = 1,
            SectionNumber = 2,

            Pages = new List<Page>
            {
                new Page
                {
                    PageId = _page121, DisplayType = SectionDisplayType.Questions,
                    LinkTitle = "1.2 Starting Page",
                    Active = true, Complete = true
                },
                new Page
                {
                    PageId = _page122, DisplayType = SectionDisplayType.Questions, LinkTitle = "1.2 page 2",
                    Active = true, Complete = true
                },
                new Page
                {
                    PageId = "Page1.2.3", DisplayType = SectionDisplayType.Questions, LinkTitle = "1.2 page 3",
                    Active = true, Complete = true
                }
            }
        };

        var sequences = new List<AssessorSequence>
        {
            new AssessorSequence
            {
                Id = Guid.NewGuid(),
                SequenceNumber = 1,
                SequenceTitle = "Sequence 1",
                Sections = new List<AssessorSection>
                {
                    section1_2
                }
            },
            new AssessorSequence
            {
                Id = Guid.NewGuid(),
                SequenceNumber = 2,
                SequenceTitle = "Sequence 2",
                Sections = new List<AssessorSection>
                {
                    section2_3,
                    section2_4
                }
            }
        };

        return sequences;

    }

    private List<ClarificationPageReviewOutcome> SetUpClarificationOutcomes()
    {
    var clarificationPages = new List<ClarificationPageReviewOutcome>
        {
            new ClarificationPageReviewOutcome
            {
                ApplicationId = _applicationId,
                PageId = _page121,
                SequenceNumber = 1,
                SectionNumber = 2,
                ModeratorReviewStatus = ModerationStatus.Fail
            },
            new ClarificationPageReviewOutcome
            {
                ApplicationId = _applicationId,
                PageId = _page122,
                SequenceNumber = 1,
                SectionNumber = 2,
                ModeratorReviewStatus = ModerationStatus.Fail
            },
            new ClarificationPageReviewOutcome
            {
                ApplicationId = _applicationId,
                PageId = "Page1.2.3",
                SequenceNumber = 1,
                SectionNumber = 2,
                ModeratorReviewStatus = ModerationStatus.Pass
            },
            new ClarificationPageReviewOutcome
            {
                ApplicationId = _applicationId,
                PageId = "Page2.3.1",
                SequenceNumber = 2,
                SectionNumber = 3,
                ModeratorReviewStatus = ModerationStatus.Fail
            },
            new ClarificationPageReviewOutcome
            {
                ApplicationId = _applicationId,
                PageId = "Page2.3.2",
                SequenceNumber = 2,
                SectionNumber = 3,
                ModeratorReviewStatus = ModerationStatus.Fail
            },
            new ClarificationPageReviewOutcome
            {
                ApplicationId = _applicationId,
                PageId = "Page2.4.1",
                SequenceNumber = 2,
                SectionNumber = 4,
                ModeratorReviewStatus = ModerationStatus.Fail
            },
            new ClarificationPageReviewOutcome
            {
                ApplicationId = _applicationId,
                PageId = "Page2.4.2",
                SequenceNumber = 2,
                SectionNumber = 4,
                ModeratorReviewStatus = ModerationStatus.Fail
            }
        };
    return clarificationPages;
    }

    private List<ApplicationSection> SetUpApplicationSections(bool pageActive, bool sequence2PagesActive, string answerToQuestion, string questionValue)
        {
            var sections = new List<ApplicationSection>();

            var applicationSection1_2 = new ApplicationSection
            {
                ApplicationId = _applicationId,
                SequenceId = 1,
                SectionId = 2,
                QnAData = new QnAData
                {
                    Pages = new List<Page>
                    {
                        new Page {PageId = _page121, Active = true, Questions = new List<Question>
                            {
                                new Question {QuestionId = _question121Id, Input = new Input{Type="Text"}, Value = questionValue, QuestionBodyText = _page121QuestionBodyText}
                            },
                            PageOfAnswers = new List<PageOfAnswers>
                            {
                                new PageOfAnswers
                                {
                                    Answers = new List<Answer>
                                    {
                                        new Answer {QuestionId = _question121Id, Value = answerToQuestion}
                                    }
                                }
                            }
                        },
                        new Page {PageId = _page122, Active = true, Questions = new List<Question> {new Question{ Input = new Input {Type="Text"}}},BodyText = _page122BodyText},
                        new Page
                        {
                            PageId = "Page1.2.3", Active = pageActive, Questions = new List<Question>()
                        }
                    },
                }
            };
            var applicationSection2_3 = new ApplicationSection
            {
                ApplicationId = _applicationId,
                SequenceId = 2,
                SectionId = 3,
                QnAData = new QnAData
                {
                    Pages = new List<Page>
                    {
                        new Page {PageId = "Page2.3.1", Active = sequence2PagesActive, Questions = new List<Question>()},
                        new Page
                        {
                            PageId = "Page2.3.2", Active = pageActive && sequence2PagesActive, Questions = new List<Question>()
                        }
                    }
                }
            };
   
            var applicationSection2_4 = new ApplicationSection
            {
                ApplicationId = _applicationId,
                SequenceId = 2,
                SectionId = 4,
                QnAData = new QnAData
                {
                    Pages = new List<Page>
                    {
                        new Page {PageId = "Page2.4.1", Active = sequence2PagesActive, Questions = new List<Question>()},
                        new Page
                        {
                            PageId = "Page2.4.2", Active = pageActive && sequence2PagesActive, Questions = new List<Question>()
                        }
                    }
                }
            };
            sections.Add(applicationSection1_2);
            sections.Add(applicationSection2_3);
            sections.Add(applicationSection2_4);
            return sections;
        }


        private ApplicationSummaryWithModeratorDetailsViewModel GetCopyOfModel()
        {
            var model = new ApplicationSummaryWithModeratorDetailsViewModel
            { ApplicationId = _applicationId };

            return model;
        }
    }
}
