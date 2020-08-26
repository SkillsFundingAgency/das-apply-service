using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Assessor;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.AssessorApplicationCountsHandlerTests
{
    [TestFixture]
    public class AssessorApplicationCountsHandlerTests
    {
        private Mock<IAssessorRepository> _repository;
        private AssessorApplicationCountsHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IAssessorRepository>();
            _handler = new AssessorApplicationCountsHandler(_repository.Object);
        }

        [Test]
        public async Task Get_assessor_application_counts_returns_number_of_new_applications()
        {
            var expectedUser = "sadjkffgdji";
            var expectedResult = 7;
            _repository.Setup(x => x.GetNewAssessorApplicationsCount(expectedUser)).ReturnsAsync(expectedResult);

            var actualResult = await _handler.Handle(new AssessorApplicationCountsRequest(expectedUser), new CancellationToken());

            Assert.AreEqual(expectedResult, actualResult.NewApplications);
        }

        [Test]
        public async Task Get_assessor_application_counts_returns_number_of_in_progress_applications()
        {
            var expectedUser = "sadjkffgdji";
            var expectedResult = 5;
            _repository.Setup(x => x.GetInProgressAssessorApplicationsCount(expectedUser)).ReturnsAsync(expectedResult);

            var actualResult = await _handler.Handle(new AssessorApplicationCountsRequest(expectedUser), new CancellationToken());

            Assert.AreEqual(expectedResult, actualResult.InProgressApplications);
        }

        [Test]
        public async Task Get_assessor_application_counts_returns_number_of_applications_in_moderation()
        {
            var expectedUser = "sadjkffgdji";
            var expectedResult = 4;
            _repository.Setup(x => x.GetApplicationsInModerationCount()).ReturnsAsync(expectedResult);

            var actualResult = await _handler.Handle(new AssessorApplicationCountsRequest(expectedUser), new CancellationToken());

            Assert.AreEqual(expectedResult, actualResult.ModerationApplications);
        }
    }
}
