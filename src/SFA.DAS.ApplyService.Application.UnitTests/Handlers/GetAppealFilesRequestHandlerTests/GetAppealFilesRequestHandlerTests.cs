using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Oversight;
using SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetStagedFiles;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetAppealFilesRequestHandlerTests
{
    [TestFixture]
    public class GetAppealFilesRequestHandlerTests
    {
        private GetStagedFilesRequestHandler _handler;
        private Mock<IAppealsQueries> _appealsQueries;
        private GetStagedFilesRequest _request;
        private AppealFiles _queryResult;

        [SetUp]
        public void Setup()
        {
            _request = new GetStagedFilesRequest
            {
                ApplicationId = Guid.NewGuid()
            };

            _queryResult = new AppealFiles();

            _appealsQueries = new Mock<IAppealsQueries>();
            _appealsQueries.Setup(x => x.GetStagedAppealFiles(_request.ApplicationId)).ReturnsAsync(() => _queryResult);
            _handler = new GetStagedFilesRequestHandler(_appealsQueries.Object);
        }

        [Test]
        public async Task Handle_Returns_Appeal_Files_For_Requested_ApplicationId()
        {
            var result = await _handler.Handle(_request, CancellationToken.None);
            Assert.AreSame(_queryResult, result);
        }
    }
}
