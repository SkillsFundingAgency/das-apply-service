using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Assessor;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.AssessorSummaryHandlerTests
{
    [TestFixture]
    public class AssessorSummaryHandlerTests
    {
        private Mock<IAssessorRepository> _repository;
        private AssessorSummaryHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IAssessorRepository>();
            _handler = new AssessorSummaryHandler(_repository.Object);
        }

        [Test]
        public async Task Get_assessor_summary_returns_number_of_new_applications()
        {
            var expectedUser = "sadjkffgdji";
            var expectedResult = 7;
            _repository.Setup(x => x.GetNewAssessorApplicationsCount(expectedUser)).ReturnsAsync(expectedResult);

            var actualResult = await _handler.Handle(new AssessorSummaryRequest(expectedUser), new CancellationToken());

            Assert.AreEqual(expectedResult, actualResult.NewApplications);
        }

        [Test]
        public async Task Get_assessor_summary_returns_number_of_in_progress_applications()
        {
            var expectedUser = "sadjkffgdji";
            var expectedResult = 5;
            _repository.Setup(x => x.GetInProgressAssessorApplicationsCount(expectedUser)).ReturnsAsync(expectedResult);

            var actualResult = await _handler.Handle(new AssessorSummaryRequest(expectedUser), new CancellationToken());

            Assert.AreEqual(expectedResult, actualResult.InProgressApplications);
        }

        [Test]
        public async Task Get_assessor_summary_returns_number_of_applications_in_moderation()
        {
            var expectedUser = "sadjkffgdji";
            var expectedResult = 4;
            _repository.Setup(x => x.GetApplicationsInModerationCount()).ReturnsAsync(expectedResult);

            var actualResult = await _handler.Handle(new AssessorSummaryRequest(expectedUser), new CancellationToken());

            Assert.AreEqual(expectedResult, actualResult.ModerationApplications);
        }
    }
}
