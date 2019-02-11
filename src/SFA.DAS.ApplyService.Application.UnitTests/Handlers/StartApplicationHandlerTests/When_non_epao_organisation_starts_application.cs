using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.StartApplicationHandlerTests
{
    public class When_non_epao_organisation_starts_application : StartApplicationHandlerTestsBase
    {
        private void Init()
        {
            OrganisationRepository.Setup(r => r.GetUserOrganisation(UserId)).ReturnsAsync(new Organisation
            {
                Id = ApplyingOrganisationId,
                OrganisationType = "",
                RoEPAOApproved = false
            });
        }
        
        [Test]
        public void Then_all_sections_should_be_required()
        {
            Init();
            
            Handler.Handle(new StartApplicationRequest(UserId), new CancellationToken()).Wait();

            ApplyRepository.Verify(r => r.UpdateSections(It.Is<List<ApplicationSection>>(response => response.All(section => section.NotRequired == false))));
        }

        [Test]
        public void Then_both_sequences_should_be_required()
        {
            Init();
            
            Handler.Handle(new StartApplicationRequest(UserId), new CancellationToken()).Wait();

            ApplyRepository.Verify(r => r.UpdateSequences(It.Is<List<ApplicationSequence>>(response => response.All(sequence => sequence.NotRequired == false))));
        }
    }
}