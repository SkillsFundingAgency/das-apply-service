using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Submit;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.ApplicationSubmitHandlerTests
{
    public class When_submitting : ApplicationSubmitHandlerTestsBase
    {
        [Test]
        public async Task Then_Handler_Returns_False_If_Not_Allowed_To_Submit()
        {
            ApplyRepository.Setup(r => r.CanSubmitApplication(It.IsAny<ApplicationSubmitRequest>())).ReturnsAsync(false);

            var request = new ApplicationSubmitRequest { SequenceId = 1, ApplicationId = Guid.NewGuid(), UserId = Guid.NewGuid(), UserEmail = "Email" };

            var result = await Handler.Handle(request, new CancellationToken());

            Assert.IsFalse(result);
            ApplyRepository.Verify(r => r.SubmitApplicationSequence(It.IsAny<ApplicationSubmitRequest>(), It.IsAny<ApplicationData>()), Times.Never);
        }

        [Test]
        public async Task Then_Handler_Returns_True_If_Allowed_To_Submit()
        {
            ApplyRepository.Setup(r => r.CanSubmitApplication(It.IsAny<ApplicationSubmitRequest>())).ReturnsAsync(true);

            var request = new ApplicationSubmitRequest { SequenceId = 1, ApplicationId = Guid.NewGuid(), UserId = Guid.NewGuid(), UserEmail = "Email" };

            var result = await Handler.Handle(request, new CancellationToken());

            Assert.IsTrue(result);
            ApplyRepository.Verify(r => r.SubmitApplicationSequence(It.IsAny<ApplicationSubmitRequest>(), It.IsAny<ApplicationData>()), Times.Once);
        }
    }
}
