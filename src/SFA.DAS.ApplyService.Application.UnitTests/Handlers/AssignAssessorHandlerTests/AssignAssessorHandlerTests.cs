using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Assessor;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.AssignAssessorHandlerTests
{
    [TestFixture]
    public class AssignAssessorHandlerTests
    {
        private Mock<IApplyRepository> _repository;
        private AssignAssessorHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IApplyRepository>();
            _handler = new AssignAssessorHandler(_repository.Object, Mock.Of<ILogger<AssignAssessorHandler>>());
        }

        [Test]
        public async Task Assigning_application_to_assessor_1_stored_the_assessor_details()
        {
            var applicationId = Guid.NewGuid();
            var expectedUserId = "sadjkffgdji";
            var expectedUserName = "sadjkffgdji";
            var assessorNumber = 1;
            
            await _handler.Handle(new AssignAssessorRequest(applicationId, assessorNumber, expectedUserId, expectedUserName), new CancellationToken());

            _repository.Verify(x => x.UpdateAssessor1(applicationId, expectedUserId,expectedUserName), Times.Once);
        }

        [Test]
        public async Task Assigning_application_to_assessor_2_stored_the_assessor_details()
        {
            var applicationId = Guid.NewGuid();
            var expectedUserId = "sadjkffgdji";
            var expectedUserName = "sadjkffgdji";
            var assessorNumber = 2;

            await _handler.Handle(new AssignAssessorRequest(applicationId, assessorNumber, expectedUserId, expectedUserName), new CancellationToken());

            _repository.Verify(x => x.UpdateAssessor2(applicationId, expectedUserId, expectedUserName), Times.Once);
        }
    }
}
