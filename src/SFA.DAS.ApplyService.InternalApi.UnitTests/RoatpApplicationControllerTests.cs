using AutoMapper;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using FluentAssertions;
    using Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Models.Roatp;
    using Moq;
    using NUnit.Framework;
    using SFA.DAS.ApplyService.Domain.Roatp;
    using SFA.DAS.ApplyService.InternalApi.Controllers;
    using SFA.DAS.ApplyService.InternalApi.Services;

    [TestFixture]
    public class RoatpApplicationControllerTests
    {
        private Mock<ILogger<RoatpApplicationController>> _logger;
        private Mock<IRoatpApiClient> _apiClient;
        private Mock<IOptions<List<RoatpSequences>>> _sequencesMock;
        private Mock<IRoatpService> _roatpServiceMock;

        private RoatpApplicationController _controller;
        private readonly RoatpSequences _sequence = new RoatpSequences { Id = 1 };

        [SetUp]
        public void Before_each_test()
        {
            _roatpServiceMock = new Mock<IRoatpService>();
            _logger = new Mock<ILogger<RoatpApplicationController>>();
            _apiClient = new Mock<IRoatpApiClient>();
            _sequencesMock = new Mock<IOptions<List<RoatpSequences>>>();
            _sequencesMock.Setup(s => s.Value).Returns(new List<RoatpSequences>() { _sequence });

            _controller = new RoatpApplicationController(_logger.Object, _apiClient.Object, _sequencesMock.Object, _roatpServiceMock.Object);

            Mapper.Reset();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<RoatpProfile>();
            });
        }

        [Test]
        public async Task Get_application_routes_returns_mapped_provider_types()
        {
            var providerTypes = new List<ProviderType>
            {
                new ProviderType { Id = 1, Type = "Main provider" },
                new ProviderType { Id = 2, Type = "Employer provider" },
                new ProviderType { Id = 3, Type = "Supporting provider" }
            };

            _apiClient.Setup(x => x.GetProviderTypes()).ReturnsAsync(providerTypes);

            var result = await _controller.GetApplicationRoutes();

            result.Should().BeOfType<OkObjectResult>();

            var applicationRoutes = ((OkObjectResult)result).Value as IEnumerable<ApplicationRoute>;

            applicationRoutes.Should().NotBeNull();

            applicationRoutes.Count().Should().Be(3);
            var applicationRoutesList = applicationRoutes.ToList();
            applicationRoutesList[0].Id.Should().Be(providerTypes[0].Id);
            applicationRoutesList[0].RouteName.Should().Be(providerTypes[0].Type);
            applicationRoutesList[1].Id.Should().Be(providerTypes[1].Id);
            applicationRoutesList[1].RouteName.Should().Be(providerTypes[1].Type);
            applicationRoutesList[2].Id.Should().Be(providerTypes[2].Id);
            applicationRoutesList[2].RouteName.Should().Be(providerTypes[2].Type);
        }

        [Test]
        public async Task Get_organisation_register_status_performs_roatp_lookup()
        {
            var registerStatus = new OrganisationRegisterStatus
            {
                UkprnOnRegister = true,
                StatusId = Domain.Roatp.OrganisationStatus.Active,
                ProviderTypeId = ProviderType.EmployerProvider
            };

            _roatpServiceMock.Setup(x => x.GetRegisterStatus(It.IsAny<int>())).ReturnsAsync(registerStatus).Verifiable();

            var result = await _controller.UkprnOnRegister(10002000);

            result.Should().BeOfType<OkObjectResult>();

            var registerStatusResult = ((OkObjectResult)result).Value as OrganisationRegisterStatus;

            registerStatusResult.Should().NotBeNull();

            registerStatusResult.UkprnOnRegister.Should().BeTrue();
            registerStatusResult.StatusId.Should().Be(registerStatus.StatusId);
            registerStatusResult.ProviderTypeId.Should().Be(registerStatus.ProviderTypeId);

            _roatpServiceMock.Verify(x => x.GetRegisterStatus(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void Get_RoatpSequences_ReturnsSequencesFromConfiguration()
        {
            var response = _controller.RoatpSequences();
            response.Should().NotBeNull();
            var result = (response as OkObjectResult).Value as List<RoatpSequences>;
            result.Should().NotBeNull();
            result.Count.Should().Be(1);
            result.FirstOrDefault().Id.Should().Be(_sequence.Id);
        }
    }
}
