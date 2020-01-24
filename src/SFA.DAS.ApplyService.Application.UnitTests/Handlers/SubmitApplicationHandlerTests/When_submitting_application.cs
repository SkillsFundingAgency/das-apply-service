using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Submit;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.SubmitApplicationHandlerTests
{
    public class When_submitting_application : SubmitApplicationHandlerTestsBase
    {
        [Test]
        public async Task Then_Handler_Returns_False_If_Not_Allowed_To_Submit()
        {
            ApplyRepository.Setup(r => r.CanSubmitApplication(It.IsAny<Guid>())).ReturnsAsync(false);

            var request = new SubmitApplicationRequest { ApplicationId = Guid.NewGuid(), SubmittingContactId = Guid.NewGuid() };

            var result = await Handler.Handle(request, new CancellationToken());

            Assert.IsFalse(result);
            ApplyRepository.Verify(r => r.SubmitApplication(It.IsAny<Guid>(), It.IsAny<ApplyData>(), It.IsAny<Guid>()), Times.Never);
        }

        [Test]
        public async Task Then_Handler_Returns_True_If_Allowed_To_Submit()
        {
            ApplyRepository.Setup(r => r.CanSubmitApplication(It.IsAny<Guid>())).ReturnsAsync(true);

            var request = new SubmitApplicationRequest
            {
                ApplicationId = Guid.NewGuid(),
                SubmittingContactId = Guid.NewGuid(),
                ApplyData = new ApplyData
                {
                    ApplyDetails = new ApplyDetails(),
                    Sequences = new List<ApplySequence>()
                }
            };

            var result = await Handler.Handle(request, new CancellationToken());

            Assert.IsTrue(result);
            ApplyRepository.Verify(r => r.SubmitApplication(It.IsAny<Guid>(), It.IsAny<ApplyData>(), It.IsAny<Guid>()), Times.Once);
        }
    }
}
