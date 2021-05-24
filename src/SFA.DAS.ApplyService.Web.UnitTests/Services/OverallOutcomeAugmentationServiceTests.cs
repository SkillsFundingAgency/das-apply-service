using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
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
        
        [SetUp]
        public void Before_each_test()
        {
            _applicationId = Guid.NewGuid();
            _qnaApiClient = new Mock<IQnaApiClient>();
            _apiClient = new Mock<IApplicationApiClient>();
            _assessorLookupService = new Mock<IAssessorLookupService>();
            _userId = "test";
            _page121 = "page1.2.1";
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

            var sequences = SetUpAsessorSequences("page title");
            _apiClient.Setup(x => x.GetClarificationSequences(_applicationId)).ReturnsAsync(sequences);

            var clarificationPages = SetUpClarificationOutcomes();
            _apiClient.Setup(x => x.GetAllClarificationPageReviewOutcomes(_applicationId, _userId))
                .ReturnsAsync(clarificationPages);
            var sections = SetUpApplicationSections(pageActive, sequence2PagesActive);

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

            var sequences = SetUpAsessorSequences(pageTitle);
            _apiClient.Setup(x => x.GetClarificationSequences(_applicationId)).ReturnsAsync(sequences);

            var clarificationPages = SetUpClarificationOutcomes();
            _apiClient.Setup(x => x.GetAllClarificationPageReviewOutcomes(_applicationId, _userId))
                .ReturnsAsync(clarificationPages);
            var sections = SetUpApplicationSections(true, true);

            _qnaApiClient.Setup(x => x.GetSections(_applicationId)).ReturnsAsync(sections);

            await _service.AugmentModelWithModerationFailDetails(modelToBeUpdated, _userId);


            var page121Title = modelToBeUpdated.Sequences.First(x => x.SequenceNumber == 1).Sections
                .FirstOrDefault(x => x.SectionNumber == 2).Pages.First(x => x.PageId == _page121).Title;

           pageTitle.Should().Be(page121Title);
        }


        [TestCase("page title")]
        [TestCase("page title 2")]
        public async Task page_titles_returned_against_sector_if_not_set_as_expected(string pageTitle)
        {
            var modelToBeUpdated = GetCopyOfModel();

            _assessorLookupService.Setup(x => x.GetTitleForPage(_page121)).Returns(string.Empty);
            _assessorLookupService.Setup(x => x.GetSectorNameForPage(_page121)).Returns(pageTitle);

            var sequences = SetUpAsessorSequences(pageTitle);
            _apiClient.Setup(x => x.GetClarificationSequences(_applicationId)).ReturnsAsync(sequences);

            var clarificationPages = SetUpClarificationOutcomes();
            _apiClient.Setup(x => x.GetAllClarificationPageReviewOutcomes(_applicationId, _userId))
                .ReturnsAsync(clarificationPages);
            var sections = SetUpApplicationSections(true, true);

            _qnaApiClient.Setup(x => x.GetSections(_applicationId)).ReturnsAsync(sections);

            await _service.AugmentModelWithModerationFailDetails(modelToBeUpdated, _userId);


            var page121Title = modelToBeUpdated.Sequences.First(x => x.SequenceNumber == 1).Sections
                .FirstOrDefault(x => x.SectionNumber == 2).Pages.First(x => x.PageId == _page121).Title;

            pageTitle.Should().Be(page121Title);
        }



        private List<AssessorSequence> SetUpAsessorSequences(string pageTitle)
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
                    PageId = "Page1.2.2", DisplayType = SectionDisplayType.Questions, LinkTitle = "1.2 page 2",
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
                PageId = "Page1.2.2",
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

    private List<ApplicationSection> SetUpApplicationSections(bool pageActive, bool sequence2PagesActive)
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
                        new Page {PageId = _page121, Active = true, Questions = new List<Question>()},
                        new Page {PageId = "Page1.2.2", Active = true, Questions = new List<Question>()},
                        new Page
                        {
                            PageId = "Page1.2.3", Active = pageActive, Questions = new List<Question>()
                        }
                    }
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
