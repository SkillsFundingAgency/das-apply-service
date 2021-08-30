using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Appeals.Queries.GetAppealFileList;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.Appeals.GetAppealFileListQueryHandlerTests
{
    [TestFixture]
    public class GetAppealFileListQueryHandlerTests
    {
        private GetAppealFileListQueryHandler _handler;
        private Mock<IAppealsQueries> _appealsQueries;
        private GetAppealFileListQuery _query;
        private List<AppealFile> _queryResult;

        [SetUp]
        public void Setup()
        {
            _query = new GetAppealFileListQuery
            {
                ApplicationId = Guid.NewGuid()
            };

            _queryResult = new List<AppealFile>
            {
                new AppealFile{ Id = Guid.NewGuid(), Filename = "test.pdf"}
            };

            _appealsQueries = new Mock<IAppealsQueries>();
            _appealsQueries.Setup(x => x.GetAppealFilesForApplication(_query.ApplicationId)).ReturnsAsync(() => _queryResult);
            _handler = new GetAppealFileListQueryHandler(_appealsQueries.Object);
        }

        [Test]
        public async Task Handle_Returns_Appeal_Files_For_Requested_ApplicationId()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);

            var compareLogic = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });
            var comparisonResult = compareLogic.Compare(_queryResult, result);
            Assert.IsTrue(comparisonResult.AreEqual);
        }
    }
}
