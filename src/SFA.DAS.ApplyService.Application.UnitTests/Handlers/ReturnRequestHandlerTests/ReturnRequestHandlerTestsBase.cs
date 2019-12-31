using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Review.Return;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Application.Users;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Organisations;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.ReturnRequestHandlerTests
{
    [TestFixture]
    public class ReturnRequestHandlerTestsBase
    {
        protected static Guid UserId;
        protected Mock<IApplyRepository> ApplyRepository;
        protected Mock<IContactRepository> ContactRepository;
        protected Mock<IEmailService> EmailService;
        protected Mock<IConfigurationService> ConfigService;
        protected ReturnRequestHandler Handler;
        protected Mock<IOrganisationRepository> OrganisationRepository;

        [SetUp]
        public void Setup()
        {
            ///////////////////////////////////////////////////////////
            // TODO: THIS WILL NEED RE-WRITING FOR NEW RoATP PROCESS
            ///////////////////////////////////////////////////////////
            
            //var initSubmissions = new List<InitSubmission> { new InitSubmission() };
            //var standardSubmissions = new List<StandardSubmission> { new StandardSubmission() };
            //var applicationData = new ApplicationData { InitSubmissions = initSubmissions, StandardSubmissions = standardSubmissions };
            //var application = new Domain.Entities.Application() { ApplicationData = applicationData };

            //var sequence1 = new ApplicationSequence { ApplicationId = application.Id, SequenceId = SequenceId.Stage1, Sections = new List<ApplicationSection>() };
            //var sequence2 = new ApplicationSequence { ApplicationId = application.Id, SequenceId = SequenceId.Stage2, Sections = new List<ApplicationSection>() };

            //ApplyRepository = new Mock<IApplyRepository>();
            //ApplyRepository.Setup(r => r.GetSequences(It.IsAny<Guid>())).ReturnsAsync(new List<ApplicationSequence> { sequence1, sequence2 });
            //ApplyRepository.Setup(r => r.GetApplication(It.IsAny<Guid>())).ReturnsAsync(application);

            //ContactRepository = new Mock<IContactRepository>();
            //ContactRepository.Setup(r => r.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Contact());

            //ConfigService = new Mock<IConfigurationService>();
            //ConfigService.Setup(x => x.GetConfig()).ReturnsAsync(new ApplyConfig
            //{
            //    AssessorServiceBaseUrl = "https://host/signinpage"
            //});

            //EmailService = new Mock<IEmailService>();

            //OrganisationRepository = new Mock<IOrganisationRepository>();

            //OrganisationRepository.Setup(r => r.GetOrganisationByApplicationId(It.IsAny<Guid>())).ReturnsAsync(new Organisation() { RoEPAOApproved = true});

            //Handler = new ReturnRequestHandler(ApplyRepository.Object, ContactRepository.Object, EmailService.Object,ConfigService.Object, OrganisationRepository.Object, new Mock<ILogger<ReturnRequestHandler>>().Object);
        }
    }
}
