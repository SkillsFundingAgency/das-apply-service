using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Organisations.UpdateOrganisation;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Controllers;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class WhenRoEpaoApprovedAndEporgIdIsUpdated
    {
        [Test]
        public async Task ThenRoEpaoApprovedFlagIsSet()
        {
            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<UpdateRoEpaoApprovedFlagRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Organisation
            {
                Id = Guid.NewGuid(),

            });

            var organisationController = new OrganisationController(mediator.Object, new Mock<ILogger<OrganisationController>>().Object);

            await organisationController.UpdateRoEpaoApprovedFlag(Guid.NewGuid(),Guid.NewGuid(),It.IsAny<string>(),true);
            
            mediator.VerifyAll();
        }
    }
}
