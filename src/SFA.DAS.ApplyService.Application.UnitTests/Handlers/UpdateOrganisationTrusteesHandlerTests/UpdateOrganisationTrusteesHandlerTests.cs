
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Organisations.UpdateOrganisation;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.InternalApi.Types.CharityCommission;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.UpdateOrganisationTrusteesHandlerTests
{
    [TestFixture]
    public class UpdateOrganisationTrusteesHandlerTests
    {
        private Mock<IOrganisationRepository> _repository;
        private UpdateOrganisationTrusteesHandler _handler;
        private string _ukprn;
        private Guid _updatedBy;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IOrganisationRepository>();
            _handler = new UpdateOrganisationTrusteesHandler(_repository.Object);
            _ukprn = "12345678";
            _updatedBy = Guid.NewGuid();
        }

        [Test]
        public async Task Update_organisation_trustees_and_return_success()
        {
            var request = new UpdateOrganisationTrusteesRequest
            {
                Ukprn = _ukprn,
                UpdatedBy = _updatedBy,
                Trustees = new List<Trustee> { new Trustee { Id = 1, Name = "Cody McCodeface" } }
            };

            var organisation = new Organisation
                {
                    OrganisationDetails = new OrganisationDetails
                    {
                        CharityCommissionDetails = new CharityCommissionDetails()
                    }
            };
            
            _repository.Setup(x => x.GetOrganisationByUkprn(_ukprn)).ReturnsAsync(organisation);

            var result = await _handler.Handle(request, new CancellationToken());

            Assert.IsTrue(result);
            _repository.Verify(x => x.UpdateOrganisation(It.IsAny<Organisation>(),_updatedBy), Times.Once);
        }

        [Test]
        public async Task Update_organisation_trustees_failed_when_on_matching_organisation()
        {
            var request = new UpdateOrganisationTrusteesRequest
            {
                Ukprn = _ukprn,
                UpdatedBy = _updatedBy,
                Trustees = new List<Trustee> { new Trustee { Id = 1, Name = "Cody McCodeface" } }
            };

            Organisation organisation = null;

            _repository.Setup(x => x.GetOrganisationByUkprn(_ukprn)).ReturnsAsync(organisation);

            var result = await _handler.Handle(request, new CancellationToken());

            Assert.IsFalse(result);
            _repository.Verify(x => x.UpdateOrganisation(It.IsAny<Organisation>(), It.IsAny<Guid>()), Times.Never);
        }
    }
}
