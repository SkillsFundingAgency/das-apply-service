using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using System;
using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetApplicationsHandlerTests
{
    [TestFixture]
    public class GetApplicationsHandlerTestsBase
    {
        protected Mock<IApplyRepository> ApplyRepository;
        protected GetApplicationsHandler Handler;   

        [SetUp]
        public void Setup()
        {
            ApplyRepository = new Mock<IApplyRepository>();
            ApplyRepository.Setup(r => r.GetUserApplications(It.IsAny<Guid>())).ReturnsAsync(new List<Domain.Entities.Apply>());
            ApplyRepository.Setup(r => r.GetOrganisationApplications(It.IsAny<Guid>())).ReturnsAsync(new List<Domain.Entities.Apply>());
            Handler = new GetApplicationsHandler(ApplyRepository.Object);
        }
    }
}