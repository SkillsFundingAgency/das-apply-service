using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetApplicationByUserHandlerTests
{
    public class When_applications_created_by_organisWhen_application_by_user_is_requested 
    {
        protected Mock<IApplyRepository> ApplyRepository;
        protected GetApplicationByUserHandler Handler;

        [SetUp]
        public void Setup()
        {
            ApplyRepository = new Mock<IApplyRepository>();
            ApplyRepository.Setup(r => r.GetApplicationByUser(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new Domain.Entities.Apply());
            Handler = new GetApplicationByUserHandler(ApplyRepository.Object);
        }

        [Test]
        public async Task Then_application_for_user_are_returned()
        {
            var applicationId = Guid.NewGuid();
            var signinId = Guid.NewGuid();
            await Handler.Handle(new GetApplicationByUserRequest(applicationId, signinId), new CancellationToken());
            ApplyRepository.Verify(r => r.GetApplicationByUser(applicationId, signinId), Times.Once);
        }
    }
}
