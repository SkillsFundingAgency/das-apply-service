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
using SFA.DAS.ApplyService.Application.Users.CreateAccount;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Types;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class WhenInviteUserIsCalledWithAValidUserRequest
    {
        [Test]
        public async Task ThenOkIsReturned()
        {
            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<CreateAccountRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var accountController = new AccountController(mediator.Object, new Mock<ILogger<AccountController>>().Object, new Mock<IContactRepository>().Object, new Mock<IApplyConfig>().Object);

            var result = await accountController.InviteUser(new NewContact()
                {Email = "email@email.com", FamilyName = "Jones", GivenName = "Fred"});

            result.Should().BeOfType<OkResult>();
        }
    }
}