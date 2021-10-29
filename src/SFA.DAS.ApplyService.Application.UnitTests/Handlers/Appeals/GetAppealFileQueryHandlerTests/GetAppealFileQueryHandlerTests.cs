using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Appeals.Queries.GetAppealFile;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.Appeals.GetAppealFileQueryHandlerTests
{
    [TestFixture]
    public class GetAppealFileQueryHandlerTests
    {
        private GetAppealFileQueryHandler _handler;
        private Mock<IAppealsQueries> _appealsQueries;
        private AppealFile _appealFile;

        private readonly Guid _applicationId = Guid.NewGuid();
        private readonly Guid _appealFileId = Guid.NewGuid();
        private readonly string _fileName = "file.pdf";
        private readonly string _contentType = "application/pdf";

        [SetUp]
        public void Setup()
        {
            _appealsQueries = new Mock<IAppealsQueries>();

            _appealFile = new AppealFile
            {
                FileName = _fileName,
                ContentType = _contentType,
                ApplicationId = _applicationId,
                Id = _appealFileId
            };

            _appealsQueries.Setup(x => x.GetAppealFile(_applicationId, _fileName)).ReturnsAsync(_appealFile);

            _handler = new GetAppealFileQueryHandler(_appealsQueries.Object);
        }

        [Test]
        public async Task Handle_Returns_Requested_Upload()
        {
            var request = new GetAppealFileQuery
            {
                ApplicationId = _applicationId,
                FileName = _fileName
            };

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.AreEqual(_appealFileId, result.Id);
            Assert.AreEqual(_applicationId, result.ApplicationId);
            Assert.AreEqual(_fileName, result.FileName);
            Assert.AreEqual(_contentType, result.ContentType);
        }

        [Test]
        public async Task Handle_Returns_Null_If_Upload_Does_Not_Belong_To_Application()
        {
            var request = new GetAppealFileQuery
            {
                ApplicationId = Guid.NewGuid(),
                FileName = _fileName
            };

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.IsNull(result);
        }
    }
}
