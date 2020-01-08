using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Start;
using SFA.DAS.ApplyService.Application.Organisations;
using SFA.DAS.ApplyService.Application.Users;
using SFA.DAS.ApplyService.Domain.Entities;
using System;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.StartApplicationHandlerTests
{
    [TestFixture]
    public class StartApplicationHandlerTestsBase
    {
        protected static Guid UserId;
        protected Guid ApplyingOrganisationId;
        protected Guid ApplicationId;

        protected Mock<IApplyRepository> ApplyRepository;
        protected Mock<IOrganisationRepository> OrganisationRepository;
        protected Mock<IContactRepository> ContactRepository;

        protected StartApplicationHandler Handler;


        [SetUp]
        public void Setup()
        {
            UserId = Guid.NewGuid();
            ApplyingOrganisationId = Guid.NewGuid();
            ApplicationId = Guid.NewGuid();

            ApplyRepository = new Mock<IApplyRepository>();
            ApplyRepository.Setup(r => r.GetNextRoatpApplicationReference()).ReturnsAsync("APR123456");
            ApplyRepository.Setup(r => r.StartApplication(ApplicationId, It.IsAny<ApplyData>(), ApplyingOrganisationId, UserId)).ReturnsAsync(ApplicationId);

            OrganisationRepository = new Mock<IOrganisationRepository>();
            OrganisationRepository.Setup(r => r.GetOrganisationByUserId(UserId)).ReturnsAsync(new Organisation { Id = ApplyingOrganisationId });

            ContactRepository = new Mock<IContactRepository>();
            ContactRepository.Setup(r => r.GetContact(UserId)).ReturnsAsync(new Contact { Id = UserId, ApplyOrganisationId = ApplyingOrganisationId });

            Handler = new StartApplicationHandler(ApplyRepository.Object, OrganisationRepository.Object, ContactRepository.Object);
        }
    }
}