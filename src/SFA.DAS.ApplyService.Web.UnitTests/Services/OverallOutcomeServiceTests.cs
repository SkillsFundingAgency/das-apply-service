using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Polly.Caching;
using SFA.DAS.ApplyService.Application.Services.Assessor;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;
using SFA.DAS.ApplyService.InternalApi.Types.Responses.Oversight;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.UnitTests.Services
{
    [TestFixture]
    public class OverallOutcomeServiceTests
    {
        private Mock<IOutcomeApiClient> _apiClient;
        private Mock<IQnaApiClient> _qnaApiClient;
        private Mock<IAssessorLookupService> _assessorLookupService;
        private Guid _applicationId;
        private string _userId;
        private OverallOutcomeService _service;
        private string _page121;
        private string _question121Id;
        private string _page122BodyText;
        private string _page121QuestionBodyText;
        private string _page122;
        private string _clarificationResponse;
        private string _emailAddress;
        private string _clarificationFile;

        [SetUp]
        public void Before_each_test()
        {
            _emailAddress = "test@test.com";
            _applicationId = Guid.NewGuid();
            _qnaApiClient = new Mock<IQnaApiClient>();
            _apiClient = new Mock<IOutcomeApiClient>();
            _assessorLookupService = new Mock<IAssessorLookupService>();
            _userId = "test";
            _page121 = "page1.2.1";
            _page122 = "page1.2.2";
            _question121Id = "121Id";
            _page121QuestionBodyText = "page 1 2 1 Question Body text used in guidance";
            _page122BodyText = "page 1 2 2 Body text";
            _clarificationResponse = "clarification response";
            _clarificationFile = "test.pdf";

            var sequences = new List<AssessorSequence>();

            var clarificationOutcomes = new List<ClarificationPageReviewOutcome>();
            var sections = new List<ApplicationSection>();
            _apiClient.Setup(x => x.GetClarificationSequences(_applicationId)).ReturnsAsync(sequences);
            _apiClient.Setup(x => x.GetAllClarificationPageReviewOutcomes(_applicationId, _userId))
                .ReturnsAsync(clarificationOutcomes);
            _qnaApiClient.Setup(x => x.GetSections(_applicationId)).ReturnsAsync(sections);
            _service = new OverallOutcomeService(_apiClient.Object, _qnaApiClient.Object,
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
        public async Task page_titles_returned_against_sector_if_not_set_by_get_title_for_page_as_expected(string pageTitle)
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


        [Test]
        public async Task clarification_text_returned_as_expected()
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

            modelToBeUpdated.PagesWithClarifications.Count.Should().Be(2);
            modelToBeUpdated.PagesWithClarifications.First(x => x.PageId == _page121).ClarificationResponse.Should()
                .Be(_clarificationResponse);
            modelToBeUpdated.PagesWithClarifications.First(x => x.PageId == _page121).ClarificationFile.Should()
                .Be(null);
            modelToBeUpdated.PagesWithClarifications.First(x => x.PageId == _page122).ClarificationFile.Should()
                .Be(_clarificationFile);
            modelToBeUpdated.PagesWithClarifications.First(x => x.PageId == _page122).ClarificationResponse.Should()
                .Be(null);
        }

        [Test]
        public async Task BuildApplicationSummaryViewModel_builds_expected_viewModel()
        {
            var emailAddress = "test@test.com";
            var ukprn = "12345678";
            var organisationName = "org name 1";
            var tradingName = "trading name";
            var applicationRouteId = 1;
            var applicationReference = "ABC";
            var submittedDate = DateTime.Today.AddDays(-32);
            var externalComments = "external comments";
            var financialReviewStatus = Domain.Entities.FinancialReviewStatus.Pass;
          
            var financialExternalComments = "financial external comments";
            var financialReviewDetails = new FinancialReviewDetails
            {
                SelectedGrade = "Outstanding",
                ExternalComments = financialExternalComments,
                Status = financialReviewStatus
            };
            var gatewayReviewStatus = GatewayReviewStatus.Pass;
            var moderationStatus = Domain.Apply.ModerationStatus.Fail;

            var application = new Apply
            {
                ApplicationId = _applicationId, 
                ExternalComments = externalComments,
                GatewayReviewStatus = gatewayReviewStatus,
                ModerationStatus = moderationStatus,
                ApplyData = new ApplyData
                {
                    ApplyDetails = new ApplyDetails
                    {
                        UKPRN = ukprn,
                        OrganisationName = organisationName,
                        TradingName = tradingName,
                        ProviderRoute = applicationRouteId,
                        ReferenceNumber = applicationReference,
                        ApplicationSubmittedOn = submittedDate
                    }
                }
            };

            var expectedModel = new ApplicationSummaryViewModel
            {
                ApplicationId = _applicationId, 
                UKPRN = ukprn,
                OrganisationName = organisationName,
                EmailAddress = emailAddress,
                TradingName = tradingName,
                ApplicationRouteId = applicationRouteId.ToString(),
                ApplicationReference = applicationReference,
                SubmittedDate = submittedDate,
                GatewayExternalComments = externalComments,
                GatewayReviewStatus = gatewayReviewStatus,
                ModerationStatus = moderationStatus,
                FinancialGrade = financialReviewDetails.SelectedGrade,
                FinancialReviewStatus = financialReviewDetails.Status,
                FinancialExternalComments = financialExternalComments
            };

            var returnedModel = _service.BuildApplicationSummaryViewModel(application, financialReviewDetails,emailAddress);
            expectedModel.Should().BeEquivalentTo(returnedModel);
        }

        [Test]
        public async Task moderator_pass_with_failed_moderator_questions_returns_model_with_no_sequences_and_external_comments ()
        {
            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Unsuccessful,
                ApplicationId = _applicationId,
                GatewayReviewStatus = GatewayReviewStatus.Pass,
                ModerationStatus = ModerationStatus.Pass,
                ApplyData = new ApplyData
                {
                    ApplyDetails = new ApplyDetails
                    {
                        UKPRN = "11112222"
                    }
                }
            };

            var oversightExternalComments = "oversight external comments";

            _apiClient.Setup(x => x.GetOversightReview(_applicationId)).ReturnsAsync(new GetOversightReviewResponse {ModerationApproved = false, ExternalComments = oversightExternalComments});
          
            var result = await _service.BuildApplicationSummaryViewModelWithGatewayAndModerationDetails(submittedApp, null, _emailAddress);

            result.ModerationPassOverturnedToFail.Should().Be(true);
            result.OversightExternalComments.Should().Be(oversightExternalComments);
            result.Sequences.Should().BeNullOrEmpty();
        }

        [TestCase(GatewayReviewStatus.Pass, true, false)] 
        [TestCase(GatewayReviewStatus.Pass, false, true)]
        [TestCase(GatewayReviewStatus.Fail, true, false)]
        [TestCase(GatewayReviewStatus.Fail, false, false)]
        public async Task application_unsuccessful_gateway_settings_work_as_expected(string gatewayReviewStatus,bool gatewayApproved, bool gatewayPassOverturnedToFail) //, string moderationStatus, bool moderationApproved, bool applicationUnsuccessfulModerationFail, bool moderationFailedAndOverturned)
        {
            var moderationStatus = ModerationStatus.Pass;

            var gatewayExternalComments = "gatewayExternalComments";

            var moderationApproved = true;

            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Unsuccessful,
                ApplicationId = _applicationId,
                GatewayReviewStatus = gatewayReviewStatus,
                ModerationStatus = moderationStatus,
                ApplyData = new ApplyData
                {
                    ApplyDetails = new ApplyDetails
                    {
                        UKPRN = "11112222"
                    },
                    GatewayReviewDetails = new ApplyGatewayDetails
                    {
                        ExternalComments = gatewayExternalComments
                    }
                }
            };

            var oversightExternalComments = "oversight external comments";

            _apiClient.Setup(x => x.GetOversightReview(_applicationId)).ReturnsAsync(new GetOversightReviewResponse { GatewayApproved = gatewayApproved, ModerationApproved = moderationApproved, ExternalComments = oversightExternalComments });

            var result = await _service.BuildApplicationSummaryViewModelWithGatewayAndModerationDetails(submittedApp, null, _emailAddress);

            result.GatewayPassOverturnedToFail.Should().Be(gatewayPassOverturnedToFail);
            result.GatewayExternalComments.Should().Be(gatewayExternalComments);

        }

        [TestCase(ModerationStatus.Pass, false, true, false, false, false,false)]
        [TestCase(ModerationStatus.Pass, true, false, true, false,false,false)]
        [TestCase(ModerationStatus.Fail, false, false, false, true, false,false)]
        [TestCase(ModerationStatus.Fail, true, false, false, false, true,true)]
        public async Task application_unsuccessful_moderation_settings_work_as_expected(string moderationStatus, bool moderationApproved, bool moderationPassOverturnedToFail, bool moderationPassApproved, bool moderationFailAndOverturned, bool moderationFailApproved, bool sequencesInjected)
        {
            var gatewayReviewStatus = GatewayAnswerStatus.Pass;

            var submittedApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.Unsuccessful,
                ApplicationId = _applicationId,
                GatewayReviewStatus = gatewayReviewStatus,
                ModerationStatus = moderationStatus,
                ApplyData = new ApplyData
                {
                    ApplyDetails = new ApplyDetails
                    {
                        UKPRN = "11112222"
                    }
                }
            };

            const string oversightExternalComments = "oversight external comments";

            _apiClient.Setup(x => x.GetOversightReview(_applicationId)).ReturnsAsync(new GetOversightReviewResponse { GatewayApproved = true, ModerationApproved = moderationApproved, ExternalComments = oversightExternalComments });

            var sequences = SetUpAsessorSequences();
            _apiClient.Setup(x => x.GetClarificationSequences(_applicationId)).ReturnsAsync(sequences);

            var clarificationPages = SetUpClarificationOutcomes();
            _apiClient.Setup(x => x.GetAllClarificationPageReviewOutcomes(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(clarificationPages);
            var sections = SetUpApplicationSections(true, false, "answer", null);

            _qnaApiClient.Setup(x => x.GetSections(_applicationId)).ReturnsAsync(sections);

            var result = await _service.BuildApplicationSummaryViewModelWithGatewayAndModerationDetails(submittedApp,null, _emailAddress);

            result.ModerationStatus.Should().Be(moderationStatus);
            result.ModerationPassApproved.Should().Be(moderationPassApproved);
            result.ModerationFailApproved.Should().Be(moderationFailApproved);
            result.ModerationFailOverturnedToPass.Should().Be(moderationFailAndOverturned);
            result.ModerationPassOverturnedToFail.Should().Be(moderationPassOverturnedToFail);
            result.OversightExternalComments.Should().Be(oversightExternalComments);
            if (sequencesInjected)
                result.Sequences.Any().Should().Be(true);
            else
                result.Sequences.Should().BeNullOrEmpty();
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
                ModeratorReviewStatus = ModerationStatus.Fail,
                Status = ModerationStatus.Fail, // should be a clarification
                ClarificationResponse = _clarificationResponse
            },
            new ClarificationPageReviewOutcome
            {
                ApplicationId = _applicationId,
                PageId = _page122,
                SequenceNumber = 1,
                SectionNumber = 2,
                ModeratorReviewStatus = ModerationStatus.Fail,
                Status = ModerationStatus.Fail,
                ClarificationFile = _clarificationFile
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
            },
            new ClarificationPageReviewOutcome
            {
                ApplicationId = _applicationId,
                PageId = "Page2.4.3",
                SequenceNumber = 2,
                SectionNumber = 4,
                ModeratorReviewStatus = ModerationStatus.Fail,
                Status=ModerationStatus.Pass // it has passed from clarification, so should not show up
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
