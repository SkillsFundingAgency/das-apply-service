﻿using System;
using System.Threading;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetOversightDetails;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.QueryResults;
using SFA.DAS.ApplyService.Types;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.OversightHandlerTests
{
    [TestFixture]
    public class OversightDetailsHandlerTests

    {
        protected Mock<IOversightReviewQueries> OversightReviewQueries;
        protected GetOversightApplicationDetailsHandler _handler;

        protected Guid applicationId = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            OversightReviewQueries = new Mock<IOversightReviewQueries>();
            OversightReviewQueries.Setup(r => r.GetOversightApplicationDetails(It.IsAny<Guid>())).ReturnsAsync(new ApplicationOversightDetails());
            _handler = new GetOversightApplicationDetailsHandler(OversightReviewQueries.Object);
        }

        [Test]
        public void Check_pending_results_are_as_expected()
        {
            var applicationDetails = new ApplicationOversightDetails
            {
                Id = Guid.NewGuid(),
                ApplicationId = applicationId,
                OrganisationName = "XXX Limited",
                Ukprn = "12344321",
                ProviderRoute = "Main",
                ApplicationReferenceNumber = "APR000111",
                ApplicationSubmittedDate = DateTime.Today.AddDays(-1),
                OversightStatus = OversightReviewStatus.Successful,
                ApplicationStatus = ApplicationStatus.Successful,    
                ApplicationDeterminedDate = DateTime.Today
            };

            OversightReviewQueries.Setup(r => r.GetOversightApplicationDetails(applicationId)).ReturnsAsync(applicationDetails);

            var result = _handler.Handle(new GetOversightApplicationDetailsRequest(applicationId), new CancellationToken()).GetAwaiter().GetResult();
            Assert.That(applicationDetails, Is.SameAs(result));
        }
    }
}
