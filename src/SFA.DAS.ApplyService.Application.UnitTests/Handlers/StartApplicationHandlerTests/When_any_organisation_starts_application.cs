using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.StartApplicationHandlerTests
{
    public class When_any_organisation_starts_application : StartApplicationHandlerTestsBase
    {
        private void Init()
        {
            OrganisationRepository.Setup(r => r.GetUserOrganisation(UserId)).ReturnsAsync(new Organisation
            {
                Id = ApplyingOrganisationId,
                OrganisationType = "",
                RoEPAOApproved = false
            });

            ApplyRepository.Setup(r => r.GetAssets()).ReturnsAsync(new List<Asset> {new Asset {Reference = "REPLACEME", Text = "REPLACEDWITH"}});
        }
        
        [Test]
        public async Task Then_ApplicationId_is_returned()
        {
            Init();

            var result = await Handler.Handle(new StartApplicationRequest(ApplicationId, UserId, ApplicationType), CancellationToken.None);

            result.Should().BeOfType<StartApplicationResponse>();
            result.As<StartApplicationResponse>().ApplicationId.Should().Be(ApplicationId);
        }
    }
}