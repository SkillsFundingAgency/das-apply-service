using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Organisations;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.UnitTests.StartApplicationHandlerTests
{
    [TestFixture]
    public class When_non_epao_organisation_starts_application
    {
        [Test]
        public void Then_all_sequences_and_sections_should_be_required()
        {
            var userId = Guid.NewGuid();
            var latestWorkflowId = Guid.NewGuid();
            var applyingOrganisationId = Guid.NewGuid();
            var applicationId = Guid.NewGuid();
            
            List<ApplicationSection> passedSections = null;
            var applyRepository = new Mock<IApplyRepository>();
            //applyRepository.Setup(r => r.UpdateSections(It.IsAny<List<ApplicationSection>>()));
                //.Callback<List<ApplicationSection>>(sections => passedSections = sections);

            applyRepository.Setup(r => r.GetAssets()).ReturnsAsync(new List<Asset>());
            applyRepository.Setup(r => r.GetLatestWorkflow("EPAO")).ReturnsAsync(latestWorkflowId);
            applyRepository.Setup(r => r.CreateApplication("EPAO", applyingOrganisationId, userId, latestWorkflowId)).ReturnsAsync(applicationId);
            applyRepository.Setup(r => r.CopyWorkflowToApplication(applicationId, latestWorkflowId, It.IsAny<string>())).ReturnsAsync(new List<ApplicationSection>
            {
                new ApplicationSection{SectionId = 1, QnAData = new QnAData{Pages = new List<Page>()}},
                new ApplicationSection{SectionId = 2, QnAData = new QnAData{Pages = new List<Page>()}},
                new ApplicationSection{SectionId = 3, QnAData = new QnAData{Pages = new List<Page>()}},
                new ApplicationSection{SectionId = 4, QnAData = new QnAData{Pages = new List<Page>()}}
            });
            applyRepository.Setup(r => r.GetSequences(applicationId)).ReturnsAsync(new List<ApplicationSequence>
            {
                new ApplicationSequence{SequenceId = SequenceId.Stage1},
                new ApplicationSequence{SequenceId = SequenceId.Stage2}
            });
            //applyRepository.Setup(r => r.UpdateSequences(It.IsAny<List<ApplicationSequence>>()));
            
            
            var organisationRepository = new Mock<IOrganisationRepository>();
            organisationRepository.Setup(r => r.GetUserOrganisation(userId)).ReturnsAsync(new Organisation
            {
                Id = applyingOrganisationId,
                OrganisationType = "",
                RoEPAOApproved = false
            });
            
            
            var handler = new StartApplicationHandler(applyRepository.Object, organisationRepository.Object);

            handler.Handle(new StartApplicationRequest(userId), new CancellationToken()).Wait();

            applyRepository.Verify(r => r.UpdateSections(It.Is<List<ApplicationSection>>(response => response.All(section => section.NotRequired == false))));

        }
    }
}