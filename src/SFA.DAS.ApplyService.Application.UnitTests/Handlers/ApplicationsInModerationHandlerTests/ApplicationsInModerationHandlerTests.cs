using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.ApplicationsInModerationHandlerTests
{
    [TestFixture]
    public class ApplicationsInModerationHandlerTests
    {
        private Mock<IAssessorRepository> _repository;
        private ApplicationsInModerationHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IAssessorRepository>();
            _handler = new ApplicationsInModerationHandler(_repository.Object);
        }

        [Test]
        public async Task Get_applications_in_moderation_returns_applications()
        {
            var expectedResult = new List<RoatpModerationApplicationSummary>();
            _repository.Setup(x => x.GetApplicationsInModeration()).ReturnsAsync(expectedResult);

            var actualResult = await _handler.Handle(new ApplicationsInModerationRequest(), new CancellationToken());

            Assert.AreSame(expectedResult, actualResult);
        }
    }
}
