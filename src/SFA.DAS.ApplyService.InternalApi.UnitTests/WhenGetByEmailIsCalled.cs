using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Users.GetContact;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Controllers;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class WhenGetByEmailIsCalled
    {
        [Test]
        [TestCase("someone@email.com")]
        public async Task ThenContactIsReturned(string email)
        {
            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<GetContactByEmailRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Contact
            {
                Id = Guid.NewGuid(),
                Email = email
            });

            var accountController = new AccountController(mediator.Object, new Mock<ILogger<AccountController>>().Object);

            var result = await accountController.GetByContactEmail(email);

            result.Should().BeOfType<ActionResult<Contact>>();
            result.Value.Should().NotBeNull();
            result.Value.Email.Should().Be(email);
        }
    }
}
