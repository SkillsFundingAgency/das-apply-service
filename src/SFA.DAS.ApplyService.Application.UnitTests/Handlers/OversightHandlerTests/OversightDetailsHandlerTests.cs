using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Oversight;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.OversightHandlerTests
{
    [TestFixture]
    public class OversightDetailsHandlerTests

    {
        protected Mock<IApplyRepository> ApplyRepository;
        protected GetOversightDetailsHandler _handler;

        protected Guid applicationId = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            ApplyRepository = new Mock<IApplyRepository>();
            ApplyRepository.Setup(r => r.GetOversightDetails(It.IsAny<Guid>())).ReturnsAsync(new ApplicationOversightDetails());
            _handler = new GetOversightDetailsHandler(ApplyRepository.Object);
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
                OversightStatus = "Successful",
                ApplicationDeterminedDate = DateTime.Today
            };

            ApplyRepository.Setup(r => r.GetOversightDetails(applicationId)).ReturnsAsync(applicationDetails);

            var result = _handler.Handle(new GetOversightDetailsRequest(applicationId), new CancellationToken()).GetAwaiter().GetResult();

            Assert.AreEqual(applicationId, result.ApplicationId);
           
        }
    }
}
