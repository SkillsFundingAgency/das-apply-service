using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Sectors;
using SFA.DAS.ApplyService.InternalApi.Services;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Services
{
    [TestFixture]
    public class SectorDetailsOrchestratorServiceTests
    {
        private SectorDetailsOrchestratorService _service;
        private Mock<IGetAssessorPageService> _getAssessorPageService;
        private Mock<IAssessorLookupService> _lookupService;
        private Mock<IExtractAnswerValueService> _extractAnswerValueService;
        private SectorQuestionIds _sectorQuestionIds;

        private readonly string _firstPageId = "57610";
        private readonly string _secondPageId = "57611";
        private readonly string _thirdPageId = "57612";
        private readonly string _fourthPageId = "57613";
        private readonly Guid _applicationId = Guid.NewGuid();
        private readonly int _sequenceId = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining;

        private readonly int _sectionId =
            RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees;

        [SetUp]
        public void TestSetup()
        {
            _getAssessorPageService = new Mock<IGetAssessorPageService>();
            _lookupService = new Mock<IAssessorLookupService>();
            _extractAnswerValueService = new Mock<IExtractAnswerValueService>();
            _sectorQuestionIds = new SectorQuestionIds();
            _getAssessorPageService.Setup(x => x.GetAssessorPage(_applicationId, _sequenceId, _sectionId, _firstPageId))
                .ReturnsAsync(new AssessorPage { PageId = _firstPageId, Answers = new List<AssessorAnswer> { new AssessorAnswer() }, NextPageId = _secondPageId});
            _getAssessorPageService.Setup(x => x.GetAssessorPage(_applicationId, _sequenceId, _sectionId, _secondPageId))
                .ReturnsAsync(new AssessorPage { PageId = _secondPageId, Answers = new List<AssessorAnswer> { new AssessorAnswer() }, NextPageId = _thirdPageId });

            _getAssessorPageService.Setup(x => x.GetAssessorPage(_applicationId, _sequenceId, _sectionId, _thirdPageId))
                .ReturnsAsync(new AssessorPage { PageId = _thirdPageId, Answers = new List<AssessorAnswer> { new AssessorAnswer() }, NextPageId = _fourthPageId });

            _getAssessorPageService.Setup(x => x.GetAssessorPage(_applicationId, _sequenceId, _sectionId, _fourthPageId))
                .ReturnsAsync(new AssessorPage { PageId = _fourthPageId, Answers = new List<AssessorAnswer> { new AssessorAnswer() } });
            _lookupService.Setup(x => x.GetSectorQuestionIdsForSectorPageId(_firstPageId)).Returns(_sectorQuestionIds);
            _extractAnswerValueService
                .Setup(a => a.ExtractAnswerValueFromQuestionId(It.IsAny<List<AssessorAnswer>>(), _sectorQuestionIds.FullName))
                .Returns(string.Empty);

            _service = new SectorDetailsOrchestratorService(_lookupService.Object,_getAssessorPageService.Object,_extractAnswerValueService.Object);
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

        
            _extractAnswerValueService
                .Setup(a => a.ExtractAnswerValueFromQuestionId(It.IsAny<List<AssessorAnswer>>(), _sectorQuestionIds.FullName))
                .Returns(fullName);
            _extractAnswerValueService
                .Setup(a => a.ExtractAnswerValueFromQuestionId(It.IsAny<List<AssessorAnswer>>(), _sectorQuestionIds.JobRole))
                .Returns(jobRole);
            _extractAnswerValueService
                .Setup(a => a.ExtractAnswerValueFromQuestionId(It.IsAny<List<AssessorAnswer>>(), _sectorQuestionIds.TimeInRole))
                .Returns(timeInRole);

            var actualSectorDetails = await _service.GetSectorDetails(_applicationId, _firstPageId);
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

            var actualSectorDetails = await _service.GetSectorDetails(_applicationId, _firstPageId);
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

            _lookupService.Setup(x => x.GetSectorQuestionIdsForSectorPageId(_firstPageId)).Returns(_sectorQuestionIds);

            _extractAnswerValueService
                .Setup(a => a.ExtractAnswerValueFromQuestionId(It.IsAny<List<AssessorAnswer>>(),
                    _sectorQuestionIds.WhatTypeOfTrainingDelivered))
                .Returns(typeOfTraining);

            var actualSectorDetails = await _service.GetSectorDetails(_applicationId, _firstPageId);
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
            _lookupService.Setup(x => x.GetSectorQuestionIdsForSectorPageId(_firstPageId)).Returns(_sectorQuestionIds);

            _extractAnswerValueService
                .Setup(a => a.ExtractAnswerValueFromQuestionId(It.IsAny<List<AssessorAnswer>>(), _sectorQuestionIds.HowHaveTheyDeliveredTraining))
                .Returns(howHaveTheyDelivered);

            _extractAnswerValueService
                .Setup(a => a.ExtractAnswerValueFromQuestionId(It.IsAny<List<AssessorAnswer>>(), _sectorQuestionIds.ExperienceOfDeliveringTraining))
                .Returns(experienceOfDelivering);

            _extractAnswerValueService
                .Setup(a => a.ExtractAnswerValueFromQuestionId(It.IsAny<List<AssessorAnswer>>(), _sectorQuestionIds.TypicalDurationOfTraining))
                .Returns(typicalDuration);

            var actualSectorDetails = await _service.GetSectorDetails(_applicationId, _firstPageId);
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

            _lookupService.Setup(x => x.GetSectorQuestionIdsForSectorPageId(_firstPageId)).Returns(_sectorQuestionIds);

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

            var actualSectorDetails = await _service.GetSectorDetails(_applicationId, _firstPageId);
            Assert.AreEqual(howHaveTheyDeliveredDescription, actualSectorDetails.HowHaveTheyDeliveredTraining);
            Assert.AreEqual(experienceOfDelivering, actualSectorDetails.ExperienceOfDeliveringTraining);
            Assert.AreEqual(typicalDuration, actualSectorDetails.TypicalDurationOfTraining);

        }
    }
}
