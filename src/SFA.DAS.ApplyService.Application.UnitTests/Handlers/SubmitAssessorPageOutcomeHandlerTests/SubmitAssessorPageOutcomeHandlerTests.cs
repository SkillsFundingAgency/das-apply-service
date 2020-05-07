using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.SubmitAssessorPageOutcomeHandlerTests
{
    [TestFixture]
    public class SubmitAssessorPageOutcomeHandlerTests
    {
        private Mock<IApplyRepository> _repository;
        private SubmitAssessorPageOutcomeHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IApplyRepository>();
            _handler = new SubmitAssessorPageOutcomeHandler(_repository.Object, Mock.Of<ILogger<SubmitAssessorPageOutcomeHandler>>());
        }

        [Test]
        public async Task SubmitAssessorPageOutcome_is_stored()
        {
            var applicationId = Guid.NewGuid();
            var sequenceNumber = 1;
            var sectionNumber = 2;
            var pageId = "30";
            var assessorType = 1;
            var userId = "4fs7f-userId-7gfhh";
            var status = "Fail";
            var comment = "Very bad";

            await _handler.Handle(new SubmitAssessorPageOutcomeRequest(applicationId, sequenceNumber, sectionNumber, pageId, assessorType, userId, status, comment), new CancellationToken());

            _repository.Verify(x => x.SubmitAssessorPageOutcome(applicationId, sequenceNumber, sectionNumber, pageId, assessorType, userId, status, comment), Times.Once);
        }
    }
}
