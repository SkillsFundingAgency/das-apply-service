using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetApplicationByUserIdHandlerTests
{
    public class When_application_by_userid_is_requested 
    {
        protected Mock<IApplyRepository> ApplyRepository;
        protected GetApplicationByUserIdHandler IdHandler;

        [SetUp]
        public void Setup()
        {
            ApplyRepository = new Mock<IApplyRepository>();
            ApplyRepository.Setup(r => r.GetApplicationByUserId(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new Domain.Entities.Apply());
            IdHandler = new GetApplicationByUserIdHandler(ApplyRepository.Object);
        }

        [Test]
        public async Task Then_application_for_user_are_returned()
        {
            var applicationId = Guid.NewGuid();
            var signinId = Guid.NewGuid();
            await IdHandler.Handle(new GetApplicationByUserIdRequest(applicationId, signinId), new CancellationToken());
            ApplyRepository.Verify(r => r.GetApplicationByUserId(applicationId, signinId), Times.Once);
        }
    }
}
