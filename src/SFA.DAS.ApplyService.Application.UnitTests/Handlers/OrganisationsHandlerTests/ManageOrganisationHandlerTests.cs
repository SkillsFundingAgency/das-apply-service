using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Organisations.ManageOrganisation;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.OrganisationsHandlerTests
{
    [TestFixture]
    public class ManageOrganisationHandlerTests
    {
        private Mock<IOrganisationRepository> _organisationRepository;
        private Mock<IOrganisationAddressesRepository> _organisationAddressesRepository;
        private ManageOrganisationHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _organisationRepository = new Mock<IOrganisationRepository>();
            _organisationAddressesRepository = new Mock<IOrganisationAddressesRepository>();
            _handler = new ManageOrganisationHandler(_organisationRepository.Object, _organisationAddressesRepository.Object);
        }

        [Test]
        public async Task ManageOrganisationHandler_creates_new_Organization_if_it_not_exists()
        {
            var userId = Guid.NewGuid();
            var request = new ManageOrganisationRequest
            {
                Name = "Test",
                OrganisationType = "Test",
                OrganisationUkprn = 10003000,
                RoATPApproved = true,
                CreatedBy = Guid.NewGuid(),
                PrimaryContactEmail = "t@t.com"
            };
            var organisationDetails = new OrganisationDetails
            {
                TradingName = "Trading name",
                CompanyNumber = "12322322",
                CharityNumber = "2232332",
                Address1 = "T1",
                Address2 = "T2",
                Address3 = "T3",
                City = "Test",
                Postcode = "DD12TT"
            };
            request.OrganisationDetails = organisationDetails;
            _organisationRepository.Setup(x => x.GetOrganisationByName(It.IsAny<string>())).ReturnsAsync(() => null);
            _organisationRepository.Setup(x => x.CreateOrganisation(It.IsAny<Organisation>())).ReturnsAsync(() => Guid.NewGuid());

            var result = await _handler.Handle(request, new CancellationToken());
            result.Should().NotBeNull();
        }

        [Test]
        public async Task ManageOrganisationHandler_Updates_Organization_if_it_already_exists()
        {
            var userId = Guid.NewGuid();
            var request = new ManageOrganisationRequest
            {
                Name = "Test",
                OrganisationType = "Test",
                OrganisationUkprn = 10003000,
                RoATPApproved = true,
                CreatedBy = Guid.NewGuid(),
                PrimaryContactEmail = "t@t.com"
            };
            var organisationDetails = new OrganisationDetails
            {
                TradingName = "Trading name",
                CompanyNumber = "12322322",
                CharityNumber = "2232332",
                Address1 = "T1",
                Address2 = "T2",
                Address3 = "T3",
                City = "Test",
                Postcode = "DD12TT"
            };
            request.OrganisationDetails = organisationDetails;
            var org = new Organisation
            {
                OrganisationDetails = organisationDetails
            };
            org.OrganisationDetails = request.OrganisationDetails;
            org.OrganisationType = request.OrganisationType;
            org.OrganisationUkprn = request.OrganisationUkprn;
            _organisationRepository.Setup(x => x.GetOrganisationByName(It.IsAny<string>())).ReturnsAsync(new Organisation());
            var result = await _handler.Handle(request, new CancellationToken());
            result.Should().NotBeNull();
        }
    }
}
