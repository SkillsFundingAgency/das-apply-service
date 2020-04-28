using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.NewAssessorApplicationsHandlerTests
{
    class NewAssessorApplicationsHandlerTests
    {
        private Mock<IApplyRepository> _repository;
        private NewAssessorApplicationsHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IApplyRepository>();
            _handler = new NewAssessorApplicationsHandler(_repository.Object);
        }

        [Test]
        public async Task Get_new_applications_returns_new_applications_for_the_user()
        {
            var expectedUser = "sadjkffgdji";
            var expectedResult = new List<RoatpAssessorApplicationSummary>();
            _repository.Setup(x => x.GetNewAssessorApplications(expectedUser)).ReturnsAsync(expectedResult);

            var actualResult = await _handler.Handle(new NewAssessorApplicationsRequest(expectedUser), new CancellationToken());

            Assert.AreSame(expectedResult, actualResult);
        }
    }
}
