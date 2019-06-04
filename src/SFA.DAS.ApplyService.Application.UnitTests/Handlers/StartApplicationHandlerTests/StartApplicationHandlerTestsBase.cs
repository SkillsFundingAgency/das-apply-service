using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Organisations;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.StartApplicationHandlerTests
{
    [TestFixture]
    public class StartApplicationHandlerTestsBase
    {
        protected static Guid UserId;
        protected Mock<IApplyRepository> ApplyRepository;
        protected Mock<IOrganisationRepository> OrganisationRepository;
        protected StartApplicationHandler Handler;
        protected Guid ApplyingOrganisationId;
        protected Guid ApplicationId;

        [SetUp]
        public void Setup()
        {
            UserId = Guid.NewGuid();
            var latestWorkflowId = Guid.NewGuid();
            ApplyingOrganisationId = Guid.NewGuid();
            ApplicationId = Guid.NewGuid();

            ApplyRepository = new Mock<IApplyRepository>();

            ApplyRepository.Setup(r => r.GetAssets()).ReturnsAsync(new List<Asset>());
            ApplyRepository.Setup(r => r.GetLatestWorkflow("EPAO")).ReturnsAsync(latestWorkflowId);
            ApplyRepository.Setup(r => r.CreateApplication("EPAO", ApplyingOrganisationId, UserId, latestWorkflowId)).ReturnsAsync(ApplicationId);
            ApplyRepository.Setup(r => r.CopyWorkflowToApplication(ApplicationId, latestWorkflowId, It.IsAny<string>())).ReturnsAsync(new List<ApplicationSection>
            {
                new ApplicationSection {SectionId = 1, QnAData = new QnAData {Pages = new List<Page>{new Page(){PageId = "1", Title = "REPLACEME"}, new Page() { PageId = "2", NotRequiredOrgTypes = new List<string> { "HEI" } } }}},
                new ApplicationSection {SectionId = 2, QnAData = new QnAData {Pages = new List<Page>()}},
                new ApplicationSection {SectionId = 3, QnAData = new QnAData {Pages = new List<Page>()}},
                new ApplicationSection {SectionId = 4, QnAData = new QnAData {Pages = new List<Page>()}}
            });
            ApplyRepository.Setup(r => r.GetSequences(ApplicationId)).ReturnsAsync(new List<ApplicationSequence>
            {
                new ApplicationSequence {SequenceId = SequenceId.Stage1},
                new ApplicationSequence {SequenceId = SequenceId.Stage2}
            });

            OrganisationRepository = new Mock<IOrganisationRepository>();

            Handler = new StartApplicationHandler(ApplyRepository.Object, OrganisationRepository.Object);
        }
    }
}