using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Oversight;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.OversightHandlerTests
{
    [TestFixture]
    public class GetPendingAppealOutcomesHandlerTests
    {
        protected Mock<IOversightReviewQueries> OversightReviewQueries;
        protected GetPendingAppealOutcomesHandler PendingHandler;
        protected string FirstOrganisationName = "XXX Limited";
        protected string SecondOrganisationName = "ZZZ Limited";

        [SetUp]
        public void Setup()
        {
            OversightReviewQueries = new Mock<IOversightReviewQueries>();
            OversightReviewQueries.Setup(r => r.GetPendingAppealOutcomes(null, null, null)).ReturnsAsync(() => new PendingAppealOutcomes());
            PendingHandler = new GetPendingAppealOutcomesHandler(OversightReviewQueries.Object);
        }

        [Test]
        public void Check_pending_results_are_as_expected()
        {
            var appealApplications = new PendingAppealOutcomes
            {
                Reviews = new List<PendingAppealOutcome> {
                new PendingAppealOutcome
                {
                    ApplicationId = Guid.NewGuid(),
                    OrganisationName =FirstOrganisationName,
                    Ukprn = "12344321",
                    ProviderRoute = "Main",
                    ApplicationReferenceNumber = "APR000111",
                    ApplicationSubmittedDate = DateTime.Today.AddDays(-1)
                },
                new PendingAppealOutcome
                {
                    ApplicationId = Guid.NewGuid(),
                    OrganisationName = SecondOrganisationName,
                    Ukprn = "43211234",
                    ProviderRoute = "Employer",
                    ApplicationReferenceNumber = "APR000112",
                    ApplicationSubmittedDate = DateTime.Today.AddDays(-1)
                }
            }
            };

            OversightReviewQueries.Setup(r => r.GetPendingAppealOutcomes(null, null, null)).ReturnsAsync(appealApplications);

            var result = PendingHandler.Handle(new GetPendingAppealOutcomesRequest(null, null, null), new CancellationToken()).GetAwaiter().GetResult();

            Assert.AreEqual(2, result.Reviews.Count);
            Assert.AreEqual(1, result.Reviews.Count(x => x.ApplicationId == appealApplications.Reviews.First().ApplicationId));
            Assert.AreEqual(1, result.Reviews.Count(x => x.OrganisationName == FirstOrganisationName));
            Assert.AreEqual(1, result.Reviews.Count(x => x.OrganisationName == SecondOrganisationName));
        }
    }
}