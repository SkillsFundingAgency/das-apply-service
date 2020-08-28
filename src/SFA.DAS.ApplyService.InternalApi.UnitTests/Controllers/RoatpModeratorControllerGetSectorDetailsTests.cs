using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Sectors;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;
using SFA.DAS.ApplyService.InternalApi.Services.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Controllers
{
    [TestFixture]
    public class RoatpModeratorControllerGetSectorDetailsTests
    {
        private readonly Guid _applicationId = Guid.NewGuid();
        private readonly int _sequenceId = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining;

        private readonly int _sectionId =
            RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees;

        private readonly string _firstPageId = "57610";
        private readonly string _secondPage = "57611";
        private readonly string _thirdPage = "57612";
        private readonly string _fourthPage = "57613";

        private readonly string _pageId = "57610";

        private Mock<ILogger<RoatpModeratorController>> _logger;
        private Mock<IMediator> _mediator;
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private Mock<IAssessorLookupService> _lookupService;
        private RoatpModeratorController _controller;
        private Mock<IAssessorSectorDetailsService> _sectorDetailsOrchestratorService;
        private SectorQuestionIds _sectorQuestionIds;

        [SetUp]
        public void TestSetup()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<RoatpModeratorController>>();
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _lookupService = new Mock<IAssessorLookupService>();
            _sectorDetailsOrchestratorService = new Mock<IAssessorSectorDetailsService>();

              var section = new ApplicationSection
            {
                ApplicationId = _applicationId,
                SequenceId = _sequenceId,
                SectionId = _sectionId,
                QnAData = new QnAData
                {
                    Pages = new List<Page> { GenerateQnAPage(_firstPageId), GenerateQnAPage(_secondPage), GenerateQnAPage(_thirdPage), GenerateQnAPage(_fourthPage) }
                }
            };

            _sectorQuestionIds =new SectorQuestionIds();
          

            _qnaApiClient.Setup(x => x.GetSectionBySectionNo(section.ApplicationId, section.SequenceId, section.SectionId)).ReturnsAsync(section);

            _qnaApiClient.Setup(x => x.SkipPageBySectionNo(section.ApplicationId, section.SequenceId, section.SectionId, _firstPageId)).ReturnsAsync(new SkipPageResponse { NextAction = "NextPage", NextActionId = _secondPage });
            _qnaApiClient.Setup(x => x.SkipPageBySectionNo(section.ApplicationId, section.SequenceId, section.SectionId, _secondPage)).ReturnsAsync(new SkipPageResponse { NextAction = "NextPage", NextActionId = _thirdPage });
            _qnaApiClient.Setup(x => x.SkipPageBySectionNo(section.ApplicationId, section.SequenceId, section.SectionId, _thirdPage)).ReturnsAsync(new SkipPageResponse { NextAction = "NextPage", NextActionId = _fourthPage });
            _controller = new RoatpModeratorController(_logger.Object, _mediator.Object, 
                _qnaApiClient.Object, _lookupService.Object, Mock.Of<IAssessorPageService>(), _sectorDetailsOrchestratorService.Object);
        }

        [Test]
        public async Task Get_sector_details_no_details_present()
        {
            _lookupService.Setup(x => x.GetSectorQuestionIdsForSectorPageId(_pageId)).Returns((SectorQuestionIds) null);
            var actualSectorDetails = await _controller.GetSectorDetails(_applicationId, _pageId);
            Assert.IsNull(actualSectorDetails);
        }


        [Test]
        public async Task Get_sector_details_for_non_matching_pageId()
        {
            _lookupService.Setup(x => x.GetSectorQuestionIdsForSectorPageId(_pageId)).Returns(_sectorQuestionIds);
            var actualSectorDetails = await _controller.GetSectorDetails(_applicationId, "randomPageId");
            Assert.IsNull(actualSectorDetails);
        }

        [Test]
        public async Task Get_sector_details_for_matching_pageId()
        {
            var sectorDetails = new AssessorSectorDetails {ApprovedByAwardingBodies = "yes"};

            _lookupService.Setup(x => x.GetSectorQuestionIdsForSectorPageId(_pageId)).Returns(_sectorQuestionIds);
            _sectorDetailsOrchestratorService.Setup(x => x.GetSectorDetails(_applicationId, _pageId)).ReturnsAsync(sectorDetails);

            var actualSectorDetails = await _controller.GetSectorDetails(_applicationId, _pageId);
            Assert.That(sectorDetails, Is.SameAs(actualSectorDetails));
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
                PageOfAnswers = new List<PageOfAnswers> { new PageOfAnswers { Answers = new List<Answer> { new Answer { QuestionId = $"Q{pageId}", Value = $"{pageId}Value" } } } }
            };
        }
    }
}
