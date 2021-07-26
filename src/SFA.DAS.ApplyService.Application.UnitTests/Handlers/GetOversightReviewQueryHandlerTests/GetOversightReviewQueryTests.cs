using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using KellermanSoftware.CompareNetObjects;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetOversightReview;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetOversightReviewQueryHandlerTests
{
    [TestFixture]
    public class GetOversightReviewQueryTests
    {
        private GetOversightReviewQueryHandler _handler;
        private Mock<IOversightReviewQueries> _appealsQueries;
        private GetOversightReviewQuery _query;
        private OversightReview _queryResult;

        [SetUp]
        public void Setup()
        {
            var autoFixture = new Fixture();

            _query = autoFixture.Create<GetOversightReviewQuery>();

            _queryResult = autoFixture.Create<OversightReview>();

            _appealsQueries = new Mock<IOversightReviewQueries>();
            _appealsQueries.Setup(x => x.GetOversightReview(_query.ApplicationId)).ReturnsAsync(() => _queryResult);
            _handler = new GetOversightReviewQueryHandler(_appealsQueries.Object);
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
