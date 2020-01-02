using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Submit;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Application.Users;
using SFA.DAS.ApplyService.Domain.Entities;
using System;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.SubmitApplicationHandlerTests
{
    [TestFixture]
    public class SubmitApplicationHandlerTestsBase
    {
        protected static Guid UserId;
        protected Mock<IApplyRepository> ApplyRepository;
        protected Mock<IContactRepository> ContactRepository;
        protected SubmitApplicationHandler Handler;

        [SetUp]
        public void Setup()
        {
            var application = new Domain.Entities.Apply() {  ApplyData = new ApplyData() };

            ApplyRepository = new Mock<IApplyRepository>();
            ApplyRepository.Setup(r => r.CanSubmitApplication(It.IsAny<Guid>())).ReturnsAsync(true);
            ApplyRepository.Setup(r => r.GetApplication(It.IsAny<Guid>())).ReturnsAsync(application);
            ApplyRepository.Setup(r => r.GetNextRoatpApplicationReference()).ReturnsAsync("APR123456");
            
            ContactRepository = new Mock<IContactRepository>();
            ContactRepository.Setup(r => r.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Contact());

            Handler = new SubmitApplicationHandler(ApplyRepository.Object, ContactRepository.Object);
        }
    }
}
