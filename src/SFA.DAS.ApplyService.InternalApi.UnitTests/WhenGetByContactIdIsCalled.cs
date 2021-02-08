using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Users;
using SFA.DAS.ApplyService.Application.Users.GetContact;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.InternalApi.Controllers;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class WhenGetByContactIdIsCalled
    {
        [Test]
        public async Task ThenContactIsReturned()
        {
            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<GetContactByIdRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Contact
            {
                Id = Guid.NewGuid(),
                
            });

            var accountController = new AccountController(mediator.Object, new Mock<ILogger<AccountController>>().Object, new Mock<IContactRepository>().Object, new Mock<IConfigurationService>().Object);

            var result = await accountController.GetByContactId(Guid.NewGuid());

            result.Should().BeOfType<ActionResult<Contact>>();
        }
    }
}
