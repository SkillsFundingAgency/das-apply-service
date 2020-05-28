using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Sectors;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class RoatpAssessorControllerGetSectorDetailsTests
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

        private Mock<ILogger<RoatpAssessorController>> _logger;
        private Mock<IMediator> _mediator;
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private Mock<IAssessorLookupService> _lookupService;
        private RoatpAssessorController _controller;
        private Mock<IExtractAnswerValueService> _extractAnswerValueService;
        private SectorQuestionIds _sectorQuestionIds;

        [SetUp]
        public void TestSetup()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<RoatpAssessorController>>();
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _lookupService = new Mock<IAssessorLookupService>();
            _extractAnswerValueService = new Mock<IExtractAnswerValueService>();
            _extractAnswerValueService
                .Setup(x => x.ExtractAnswerValueFromQuestionId(It.IsAny<IReadOnlyCollection<AssessorAnswer>>(),
                    It.IsAny<string>())).Returns(string.Empty);
            _extractAnswerValueService
                .Setup(x => x.ExtractFurtherQuestionAnswerValueFromQuestionId(It.IsAny<AssessorPage>(),
                    It.IsAny<string>())).Returns(string.Empty);

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
            _controller = new RoatpAssessorController(_logger.Object, _mediator.Object, 
                _qnaApiClient.Object, _lookupService.Object, _extractAnswerValueService.Object);
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
        public async Task Get_sector_details_for_page1_answers()
        {
            _sectorQuestionIds.FullName = "FullNameQuestionId";
            _sectorQuestionIds.JobRole = "JobRoleId";
            _sectorQuestionIds.TimeInRole = "TimeInRoleId";
            var fullName = "FullName Answer";
            var jobRole = "Job Role Description";
            var timeInRole = "Time in role";

            _lookupService.Setup(x => x.GetSectorQuestionIdsForSectorPageId(_pageId)).Returns(_sectorQuestionIds);

            _extractAnswerValueService
                .Setup(a => a.ExtractAnswerValueFromQuestionId(It.IsAny<List<AssessorAnswer>>(), _sectorQuestionIds.FullName))
                .Returns(fullName);
            _extractAnswerValueService
                .Setup(a => a.ExtractAnswerValueFromQuestionId(It.IsAny<List<AssessorAnswer>>(), _sectorQuestionIds.JobRole))
                .Returns(jobRole);
            _extractAnswerValueService
                .Setup(a => a.ExtractAnswerValueFromQuestionId(It.IsAny<List<AssessorAnswer>>(), _sectorQuestionIds.TimeInRole))
                .Returns(timeInRole);

            var actualSectorDetails = await _controller.GetSectorDetails(_applicationId, _firstPageId);
            Assert.AreEqual(fullName, actualSectorDetails.FullName);
            Assert.AreEqual(jobRole, actualSectorDetails.JobRole);
            Assert.AreEqual(timeInRole, actualSectorDetails.TimeInRole);
        }

        [Test]
        public async Task Get_sector_details_for_page2_answers()
        {
            _sectorQuestionIds.ExperienceOfDelivering = "ExperienceOfDeliveringId";
            _sectorQuestionIds.DoTheyHaveQualifications = "qualificationsId";
            _sectorQuestionIds.AwardingBodies = "awardingId";
            _sectorQuestionIds.TradeMemberships = "TradeId";

            var experienceOfDelivering = "experience of delivering";
            var experienceOfDeliveringFurther = "experience of delivering further";
            var doTheyHaveQualifications = "Do they have qualifications";
            var whatQualificationsDoTheyHave = "What qualifications";
            var approvedByAwardingBodies = "awarding bodies";
            var namesOfAwardingBodies = "names of awarding bodies";
            var doTheyHaveTradeBodyMemberships = "trade memberships";
            var namesOfTradeBodyMemberships = "names of trade memberships";

            _lookupService.Setup(x => x.GetSectorQuestionIdsForSectorPageId(_pageId)).Returns(_sectorQuestionIds);

            _extractAnswerValueService
                .Setup(a => a.ExtractAnswerValueFromQuestionId(It.IsAny<List<AssessorAnswer>>(), _sectorQuestionIds.ExperienceOfDelivering))
                .Returns(experienceOfDelivering);
            _extractAnswerValueService
                .Setup(a => a.ExtractFurtherQuestionAnswerValueFromQuestionId(It.IsAny<AssessorPage>(), _sectorQuestionIds.ExperienceOfDelivering))
                .Returns(experienceOfDeliveringFurther);

            _extractAnswerValueService
                .Setup(a => a.ExtractAnswerValueFromQuestionId(It.IsAny<List<AssessorAnswer>>(), _sectorQuestionIds.DoTheyHaveQualifications))
                .Returns(doTheyHaveQualifications);
            _extractAnswerValueService
                .Setup(a => a.ExtractFurtherQuestionAnswerValueFromQuestionId(It.IsAny<AssessorPage>(), _sectorQuestionIds.DoTheyHaveQualifications))
                .Returns(whatQualificationsDoTheyHave);


            _extractAnswerValueService
                .Setup(a => a.ExtractAnswerValueFromQuestionId(It.IsAny<List<AssessorAnswer>>(), _sectorQuestionIds.AwardingBodies))
                .Returns(approvedByAwardingBodies);
            _extractAnswerValueService
                .Setup(a => a.ExtractFurtherQuestionAnswerValueFromQuestionId(It.IsAny<AssessorPage>(), _sectorQuestionIds.AwardingBodies))
                .Returns(namesOfAwardingBodies);


            _extractAnswerValueService
                .Setup(a => a.ExtractAnswerValueFromQuestionId(It.IsAny<List<AssessorAnswer>>(), _sectorQuestionIds.TradeMemberships))
                .Returns(doTheyHaveTradeBodyMemberships);
            _extractAnswerValueService
                .Setup(a => a.ExtractFurtherQuestionAnswerValueFromQuestionId(It.IsAny<AssessorPage>(), _sectorQuestionIds.TradeMemberships))
                .Returns(namesOfTradeBodyMemberships);

            var actualSectorDetails = await _controller.GetSectorDetails(_applicationId, _firstPageId);
            Assert.AreEqual(experienceOfDelivering, actualSectorDetails.ExperienceOfDelivering);
            Assert.AreEqual(experienceOfDeliveringFurther, actualSectorDetails.WhereDidTheyGainThisExperience);
            Assert.AreEqual(doTheyHaveQualifications, actualSectorDetails.DoTheyHaveQualifications);
            Assert.AreEqual(whatQualificationsDoTheyHave, actualSectorDetails.WhatQualificationsDoTheyHave);
            Assert.AreEqual(approvedByAwardingBodies, actualSectorDetails.ApprovedByAwardingBodies);
            Assert.AreEqual(namesOfAwardingBodies, actualSectorDetails.NamesOfAwardingBodies);
            Assert.AreEqual(doTheyHaveTradeBodyMemberships, actualSectorDetails.DoTheyHaveTradeBodyMemberships);
            Assert.AreEqual(namesOfTradeBodyMemberships, actualSectorDetails.NamesOfTradeBodyMemberships);
        }

        [Test]
        public async Task Get_sector_details_for_page3_answers()
        {
            _sectorQuestionIds.WhatTypeOfTrainingDelivered = "WhatTypeOfTrainingId";


            var typeOfTraining = "type of training";
            
            _lookupService.Setup(x => x.GetSectorQuestionIdsForSectorPageId(_pageId)).Returns(_sectorQuestionIds);

            _extractAnswerValueService
                .Setup(a => a.ExtractAnswerValueFromQuestionId(It.IsAny<List<AssessorAnswer>>(),
                    _sectorQuestionIds.WhatTypeOfTrainingDelivered))
                .Returns(typeOfTraining);
            
            var actualSectorDetails = await _controller.GetSectorDetails(_applicationId, _firstPageId);
            Assert.AreEqual(typeOfTraining, actualSectorDetails.WhatTypeOfTrainingDelivered);
        }


        [Test]
        public async Task Get_sector_details_for_page4_answers()
        {
            _sectorQuestionIds.HowHaveTheyDeliveredTraining = "HowHaveTheyDeliveredId";
            _sectorQuestionIds.ExperienceOfDeliveringTraining = "ExperienceId";
            _sectorQuestionIds.TypicalDurationOfTraining = "DurationId";

            var howHaveTheyDelivered = "delivery mechanism";
            var experienceOfDelivering = "experience of delivering";
            var typicalDuration = "typical duration";
               _lookupService.Setup(x => x.GetSectorQuestionIdsForSectorPageId(_pageId)).Returns(_sectorQuestionIds);

            _extractAnswerValueService
                .Setup(a => a.ExtractAnswerValueFromQuestionId(It.IsAny<List<AssessorAnswer>>(), _sectorQuestionIds.HowHaveTheyDeliveredTraining))
                .Returns(howHaveTheyDelivered);

            _extractAnswerValueService
                .Setup(a => a.ExtractAnswerValueFromQuestionId(It.IsAny<List<AssessorAnswer>>(), _sectorQuestionIds.ExperienceOfDeliveringTraining))
                .Returns(experienceOfDelivering);

            _extractAnswerValueService
                .Setup(a => a.ExtractAnswerValueFromQuestionId(It.IsAny<List<AssessorAnswer>>(), _sectorQuestionIds.TypicalDurationOfTraining))
                .Returns(typicalDuration);

            var actualSectorDetails = await _controller.GetSectorDetails(_applicationId, _firstPageId);
            Assert.AreEqual(howHaveTheyDelivered, actualSectorDetails.HowHaveTheyDeliveredTraining);
            Assert.AreEqual(experienceOfDelivering, actualSectorDetails.ExperienceOfDeliveringTraining);
            Assert.AreEqual(typicalDuration, actualSectorDetails.TypicalDurationOfTraining);

        }


        [Test]
        public async Task Get_sector_details_for_page4_answers_other()
        {
            _sectorQuestionIds.HowHaveTheyDeliveredTraining = "HowHaveTheyDeliveredId";
            _sectorQuestionIds.ExperienceOfDeliveringTraining = "ExperienceId";
            _sectorQuestionIds.TypicalDurationOfTraining = "DurationId";

            var howHaveTheyDeliveredDescription = "other delivery mechanism";
            var experienceOfDelivering = "experience of delivering";
            var typicalDuration = "typical duration";
            var otherHowHaveTheyDelivered = "delivery mechanism other";

            _lookupService.Setup(x => x.GetSectorQuestionIdsForSectorPageId(_pageId)).Returns(_sectorQuestionIds);

            _extractAnswerValueService
                .Setup(a => a.ExtractAnswerValueFromQuestionId(It.IsAny<List<AssessorAnswer>>(), _sectorQuestionIds.HowHaveTheyDeliveredTraining))
                .Returns(RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.DeliveringTrainingOther);

            _extractAnswerValueService
                .Setup(a => a.ExtractFurtherQuestionAnswerValueFromQuestionId(It.IsAny<AssessorPage>(), _sectorQuestionIds.HowHaveTheyDeliveredTraining))
                .Returns(howHaveTheyDeliveredDescription);

            _extractAnswerValueService
                .Setup(a => a.ExtractAnswerValueFromQuestionId(It.IsAny<List<AssessorAnswer>>(), _sectorQuestionIds.ExperienceOfDeliveringTraining))
                .Returns(experienceOfDelivering);

            _extractAnswerValueService
                .Setup(a => a.ExtractAnswerValueFromQuestionId(It.IsAny<List<AssessorAnswer>>(), _sectorQuestionIds.TypicalDurationOfTraining))
                .Returns(typicalDuration);

            var actualSectorDetails = await _controller.GetSectorDetails(_applicationId, _firstPageId);
            Assert.AreEqual(howHaveTheyDeliveredDescription, actualSectorDetails.HowHaveTheyDeliveredTraining);
            Assert.AreEqual(experienceOfDelivering, actualSectorDetails.ExperienceOfDeliveringTraining);
            Assert.AreEqual(typicalDuration, actualSectorDetails.TypicalDurationOfTraining);

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
