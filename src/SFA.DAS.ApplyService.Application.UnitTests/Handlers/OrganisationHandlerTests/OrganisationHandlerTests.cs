using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Organisations.GetOrganisation;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.OrganisationHandlerTests
{
    [TestFixture]
    public class OrganisationHandlerTests
    {
        private GetOrganisationByApplicationIdHandler _handler;
        private Mock<IOrganisationRepository> _repository;
        private Guid _applicationId;

        [SetUp]
        public void TestSetup()
        {
            _applicationId = Guid.NewGuid();
            _repository = new Mock<IOrganisationRepository>();
            _handler = new GetOrganisationByApplicationIdHandler(_repository.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_ReturnsOrganisation()
        {
            var ukprn = 12334455;
            var organisation = new Organisation {OrganisationUkprn = ukprn};

            _repository.Setup(x => x.GetOrganisationByApplicationId(_applicationId)).ReturnsAsync(organisation);
            var result = await _handler.Handle(new GetOrganisationByApplicationIdRequest(_applicationId), new CancellationToken());

            Assert.IsNotNull(result);
            Assert.AreEqual(result.OrganisationUkprn, ukprn);
        }
    }
}
