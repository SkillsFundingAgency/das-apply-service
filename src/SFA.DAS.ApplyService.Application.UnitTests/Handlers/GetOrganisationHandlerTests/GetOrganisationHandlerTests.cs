using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Application.Organisations.GetOrganisation;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetOrganisationHandlerTests
{

    [TestFixture]
    public class GetOrganisationHandlerTests
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
        public async Task GetOrganisationDetailsByApplicationIdHandler_returns_organisation_details()
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
