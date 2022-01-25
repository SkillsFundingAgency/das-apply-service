using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Organisations.GetOrganisation;
using SFA.DAS.ApplyService.Application.Organisations.UpdateOrganisation;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Controllers;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Controllers.OrganisationControllerTests
{


    [TestFixture]
    public class GetOrganisationByApplicationIdTests
    {
        private OrganisationController _controller;
        private Mock<IMediator> _mediator;
        private Mock<ILogger<OrganisationController>> _logger;
        private Guid _applicationId = Guid.NewGuid();

        [SetUp]
        public void TestSetup()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<OrganisationController>>();
            _controller = new OrganisationController(_mediator.Object, _logger.Object);
        }

        [Test]
        public async Task GetOrganisationByAppicationId_OnSuccess_ReturnsOrganisationAndOk()
        {
            var organisationId = Guid.NewGuid();
            var organisation = new Organisation {Id = organisationId};
            _mediator.Setup(x => x.Send(It.IsAny<GetOrganisationByApplicationIdRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(organisation);
            var result = await _controller.GetOrganisationByApplicationId(_applicationId);
            result.Should().BeOfType<ActionResult<Organisation>>();
            result.Result.Should().BeOfType<OkObjectResult>();
            var expectedOrganisation =((ObjectResult)result.Result).Value as Organisation;
            expectedOrganisation.Id.Should().Be(organisationId);
    
        }

        [Test]
        public async Task GetOrganisationByAppicationId_NoOrganisationFound_ReturnsNotFound()
        {
            _mediator.Setup(x => x.Send(It.IsAny<GetOrganisationByApplicationIdRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Organisation)null);
            var result = await _controller.GetOrganisationByApplicationId(_applicationId);
            result.Result.Should().BeOfType<NotFoundResult>();
        }
    }
}
