using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetStagedFiles;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetAppealFilesRequestHandlerTests
{
    [TestFixture]
    public class GetAppealFilesRequestHandlerTests
    {
        private GetStagedFilesQueryHandler _handler;
        private Mock<IAppealsQueries> _appealsQueries;
        private GetStagedFilesQuery _query;
        private AppealFiles _queryResult;

        [SetUp]
        public void Setup()
        {
            _query = new GetStagedFilesQuery
            {
                ApplicationId = Guid.NewGuid()
            };

            _queryResult = new AppealFiles
            {
                Files = new List<AppealFile>
                {
                    new AppealFile{ Id = Guid.NewGuid(), Filename = "test.pdf"}
                }
            };

            _appealsQueries = new Mock<IAppealsQueries>();
            _appealsQueries.Setup(x => x.GetStagedAppealFiles(_query.ApplicationId)).ReturnsAsync(() => _queryResult);
            _handler = new GetStagedFilesQueryHandler(_appealsQueries.Object);
        }

        [Test]
        public async Task Handle_Returns_Appeal_Files_For_Requested_ApplicationId()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);

            var compareLogic = new CompareLogic(new ComparisonConfig{ IgnoreObjectTypes = true});
            var comparisonResult = compareLogic.Compare(_queryResult, result);
            Assert.IsTrue(comparisonResult.AreEqual);
        }
    }
}
