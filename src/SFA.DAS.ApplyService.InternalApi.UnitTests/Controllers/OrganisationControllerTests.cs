using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Organisations.UpdateOrganisation;
using SFA.DAS.ApplyService.InternalApi.Controllers;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Controllers
{
    [TestFixture]
    public class OrganisationControllerTests
    {
        private OrganisationController _controller;
        private Mock<IMediator> _mediator;
        private Mock<ILogger<OrganisationController>> _logger;
        private static readonly Fixture AutoFixture = new Fixture();

        [SetUp]
        public void TestSetup()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<OrganisationController>>();
            _controller = new OrganisationController(_mediator.Object, _logger.Object);
        }

        [Test]
        public async Task Update_organisation_trustees_successful_returns_ok()
        {
            var request = AutoFixture.Create<UpdateOrganisationTrusteesRequest>();
            _mediator.Setup(x => x.Send(It.IsAny<UpdateOrganisationTrusteesRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            var result = await _controller.UpdateOrganisationTrustees(request);
            result.Should().BeOfType<ActionResult<bool>>();
            _mediator.Verify(x => x.Send(It.Is<UpdateOrganisationTrusteesRequest>(r=>r.Trustees == request.Trustees && r.Ukprn == request.Ukprn && r.UpdatedBy == request.UpdatedBy), It.IsAny<CancellationToken>()), Times.Once);

            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task Update_organisation_trustees_unsuccessful_returns_bad_request()
        {
            var request = AutoFixture.Create<UpdateOrganisationTrusteesRequest>();
            _mediator.Setup(x => x.Send(It.IsAny<UpdateOrganisationTrusteesRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            var result = await _controller.UpdateOrganisationTrustees(request);
            result.Should().BeOfType<ActionResult<bool>>();
            _mediator.Verify(x => x.Send(It.Is<UpdateOrganisationTrusteesRequest>(r => r.Trustees == request.Trustees && r.Ukprn == request.Ukprn && r.UpdatedBy == request.UpdatedBy), It.IsAny<CancellationToken>()), Times.Once);

            result.Result.Should().BeOfType<BadRequestResult>();
        }
    }
}
