using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Submit;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Application.Users;
using SFA.DAS.ApplyService.Domain.Entities;
using System;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.ApplicationSubmitHandlerTests
{
    [TestFixture]
    public class ApplicationSubmitHandlerTestsBase
    {
        protected static Guid UserId;
        protected Mock<IApplyRepository> ApplyRepository;
        protected Mock<IContactRepository> ContactRepository;
        protected Mock<IEmailService> EmailService;
        protected ApplicationSubmitHandler Handler;

        [SetUp]
        public void Setup()
        {
            var application = new Domain.Entities.Application() { ApplicationData = new ApplicationData() };

            ApplyRepository = new Mock<IApplyRepository>();
            ApplyRepository.Setup(r => r.GetNextAppReferenceSequence()).ReturnsAsync(1);
            ApplyRepository.Setup(r => r.GetWorkflowReferenceFormat(It.IsAny<Guid>())).ReturnsAsync("AAD");
            ApplyRepository.Setup(r => r.GetApplication(It.IsAny<Guid>())).ReturnsAsync(application);
            ApplyRepository.Setup(r => r.CanSubmitApplication(It.IsAny<ApplicationSubmitRequest>())).ReturnsAsync(true);

            ContactRepository = new Mock<IContactRepository>();
            ContactRepository.Setup(r => r.GetContact(It.IsAny<string>())).ReturnsAsync(new Contact());

            EmailService = new Mock<IEmailService>();

            Handler = new ApplicationSubmitHandler(ApplyRepository.Object, EmailService.Object, ContactRepository.Object);
        }
    }
}
