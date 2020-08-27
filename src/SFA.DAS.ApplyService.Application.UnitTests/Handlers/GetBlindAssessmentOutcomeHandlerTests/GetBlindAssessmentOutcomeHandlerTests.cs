using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Application.Apply.Moderator;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types.Moderator;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetBlindAssessmentOutcomeHandlerTests
{
    [TestFixture]
    public class GetBlindAssessmentOutcomeHandlerTests
    {
        protected Mock<IAssessorRepository> _repository;
        protected GetBlindAssessmentOutcomeHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IAssessorRepository>();
            _handler = new GetBlindAssessmentOutcomeHandler(_repository.Object, Mock.Of<ILogger<GetBlindAssessmentOutcomeHandler>>());
        }

        [Test]
        public async Task GetBlindAssessmentOutcomeHandler_returns__BlindAssessmentOutcome()
        {
            var applicationId = Guid.NewGuid();
            var sequenceNumber = 1;
            var sectionNumber = 2;
            var pageId = "30";

            var expectedResult = new BlindAssessmentOutcome
            {
                Assessor1Name = "Assessor 1",
                Assessor1ReviewStatus = "Pass",
                Assessor1ReviewComment = "",
                Assessor2Name = "Assessor 2",
                Assessor2ReviewStatus = "Failed",
                Assessor2ReviewComment = "Very bad"
            };

            _repository.Setup(x => x.GetBlindAssessmentOutcome(applicationId, sequenceNumber, sectionNumber, pageId)).ReturnsAsync(expectedResult);

            var actualResult = await _handler.Handle(new GetBlindAssessmentOutcomeRequest(applicationId, sequenceNumber, sectionNumber, pageId), new CancellationToken());

            Assert.AreSame(expectedResult, actualResult);
        }
    }
}
