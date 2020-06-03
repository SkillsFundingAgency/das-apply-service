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
    public class OversightPendingHandlerTests
    {
        protected Mock<IApplyRepository> ApplyRepository;
        protected GetOversightsPendingHandler PendingHandler;
        protected string FirstOrganisationName = "XXX Limited";
        protected string SecondOrganisationName = "ZZZ Limited";
        [SetUp]
        public void Setup()
        {
            ApplyRepository = new Mock<IApplyRepository>();
            ApplyRepository.Setup(r => r.GetOversightsPending()).ReturnsAsync(new List<ApplicationOversightDetails>());
            PendingHandler = new GetOversightsPendingHandler(ApplyRepository.Object);
          }

        [Test]
        public void Check_pending_results_are_as_expected()
        {
            var completedApplications = new List<ApplicationOversightDetails>
            {
                new ApplicationOversightDetails
                {
                    Id = Guid.NewGuid(),
                    ApplicationId = Guid.NewGuid(),
                    OrganisationName =FirstOrganisationName,
                    Ukprn = "12344321",
                    ProviderRoute = "Main",
                    ApplicationReferenceNumber = "APR000111",
                    OversightStatus = "New"
                },
                new ApplicationOversightDetails
                {
                    Id = Guid.NewGuid(),
                    ApplicationId = Guid.NewGuid(),
                    OrganisationName = SecondOrganisationName,
                    Ukprn = "43211234",
                    ProviderRoute = "Employer",
                    ApplicationReferenceNumber = "APR000112",
                    OversightStatus = "New"
                }
            };

            ApplyRepository.Setup(r => r.GetOversightsPending()).ReturnsAsync(completedApplications);

            var result = PendingHandler.Handle(new GetOversightsPendingRequest(), new CancellationToken()).GetAwaiter().GetResult();

            Assert.AreEqual(2,result.Count);
            Assert.AreEqual(1,result.Count(x=> x.Id == completedApplications.First().Id));
            Assert.AreEqual(1, result.Count(x => x.OrganisationName==FirstOrganisationName));
            Assert.AreEqual(1, result.Count(x => x.OrganisationName == SecondOrganisationName));
        }
    }
}
