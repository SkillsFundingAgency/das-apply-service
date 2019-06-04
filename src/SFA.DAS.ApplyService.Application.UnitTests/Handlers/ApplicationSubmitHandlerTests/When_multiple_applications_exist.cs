using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Submit;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Threading;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.ApplicationSubmitHandlerTests
{
    public class When_multiple_applications_exist : ApplicationSubmitHandlerTestsBase
    {
        private readonly Guid differentAppGuid = Guid.NewGuid();
        private readonly Guid sameAppGuid = Guid.NewGuid();

        [SetUp]
        public void AdditionalSetup()
        {
            ApplyRepository.Setup(r => r.CanSubmitApplication(It.Is<ApplicationSubmitRequest>(req => req.ApplicationId == differentAppGuid))).ReturnsAsync(false);
            ApplyRepository.Setup(r => r.CanSubmitApplication(It.Is<ApplicationSubmitRequest>(req => req.ApplicationId == sameAppGuid))).ReturnsAsync(true);  
        }

        [Test]
        public void Then_prevent_submission_if_another_user_has_already_submitted()
        {
            var request = new ApplicationSubmitRequest { SequenceId = 1, ApplicationId = differentAppGuid, UserId = differentAppGuid };

            Handler.Handle(request, new CancellationToken()).Wait();
            
            ApplyRepository.Verify(r => r.SubmitApplicationSequence(It.IsAny<ApplicationSubmitRequest>(), It.IsAny<ApplicationData>()), Times.Never);
        }

        [Test]
        public void Then_allow_submission_if_user_is_the_one_whom_already_submitted()
        {
            var request = new ApplicationSubmitRequest { SequenceId = 2, ApplicationId = sameAppGuid, UserId = sameAppGuid };

            Handler.Handle(request, new CancellationToken()).Wait();

            ApplyRepository.Verify(r => r.SubmitApplicationSequence(It.IsAny<ApplicationSubmitRequest>(), It.IsAny<ApplicationData>()), Times.Once);
        }
    }
}
