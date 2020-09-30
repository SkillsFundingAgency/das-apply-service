using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Moderator;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.SubmitModeratorPageOutcomeHandlerTests
{
    [TestFixture]
    public class SubmitModeratorPageOutcomeHandlerTests
    {
        private Mock<IModeratorRepository> _repository;
        private SubmitModeratorPageOutcomeHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IModeratorRepository>();
            _handler = new SubmitModeratorPageOutcomeHandler(_repository.Object, Mock.Of<ILogger<SubmitModeratorPageOutcomeHandler>>());
        }

        [Test]
        public async Task SubmitModeratorPageOutcome_is_stored()
        {
            var applicationId = Guid.NewGuid();
            var sequenceNumber = 1;
            var sectionNumber = 2;
            var pageId = "30";
            var userId = "4fs7f-userId-7gfhh";
            var status = "Fail";
            var comment = "Very bad";

            await _handler.Handle(new SubmitModeratorPageOutcomeRequest(applicationId, sequenceNumber, sectionNumber, pageId, userId, status, comment), new CancellationToken());

            _repository.Verify(x => x.SubmitModeratorPageOutcome(applicationId, sequenceNumber, sectionNumber, pageId, userId, status, comment), Times.Once);
        }
    }
}
