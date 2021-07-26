using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Clarification;
using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.DeleteClarificationFileHandlerTests
{
    [TestFixture]
    public class DeleteClarificationFileHandlerTests
    {
        private Mock<IClarificationRepository> _repository;
        private DeleteClarificationFileHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IClarificationRepository>();
            _handler = new DeleteClarificationFileHandler(_repository.Object, Mock.Of<ILogger<DeleteClarificationFileHandler>>());
        }

        [Test]
        public async Task SubmitModeratorPageOutcome_is_stored()
        {
            var applicationId = Guid.NewGuid();
            var sequenceNumber = 1;
            var sectionNumber = 2;
            var pageId = "30";
            var clarificaitonFile = "file.pdf";

            await _handler.Handle(new DeleteClarificationFileRequest(applicationId, sequenceNumber, sectionNumber, pageId, clarificaitonFile), new CancellationToken());

            _repository.Verify(x => x.DeleteClarificationFile(applicationId, sequenceNumber, sectionNumber, pageId, clarificaitonFile), Times.Once);
        }
    }
}
