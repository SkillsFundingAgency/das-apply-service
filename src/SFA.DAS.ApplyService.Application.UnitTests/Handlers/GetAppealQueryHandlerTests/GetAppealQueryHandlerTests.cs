using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using KellermanSoftware.CompareNetObjects;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetAppeal;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetAppealQueryHandlerTests
{
    [TestFixture]
    public class GetAppealQueryHandlerTests
    {
        private GetAppealQueryHandler _handler;
        private Mock<IAppealsQueries> _appealsQueries;
        private GetAppealQuery _query;
        private Appeal _queryResult;

        [SetUp]
        public void Setup()
        {
            var autoFixture = new Fixture();

            _query = new GetAppealQuery
            {
                ApplicationId = Guid.NewGuid(),
                OversightReviewId = Guid.NewGuid()
            };

            _queryResult = new Appeal
            {
                Message = autoFixture.Create<string>(),
                CreatedOn = autoFixture.Create<DateTime>(),
                UserId = autoFixture.Create<string>(),
                UserName = autoFixture.Create<string>()
            };

            _appealsQueries = new Mock<IAppealsQueries>();
            _appealsQueries.Setup(x => x.GetAppeal(_query.ApplicationId, _query.OversightReviewId)).ReturnsAsync(() => _queryResult);
            _handler = new GetAppealQueryHandler(_appealsQueries.Object);
        }

        [Test]
        public async Task Handle_Returns_Appeal_For_Requested_Application_And_OversightReview()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);

            var compareLogic = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });
            var comparisonResult = compareLogic.Compare(_queryResult, result);
            Assert.IsTrue(comparisonResult.AreEqual);
        }

    }
}
