using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetApplicationsHandlerTests
{
    public class When_applications_created_by_organisation_are_requested : GetApplicationsHandlerTestsBase
    { 
        [Test]
        public async Task Then_application_created_by_orgainisation_are_returned()
        {
            await Handler.Handle(new GetApplicationsRequest(Guid.NewGuid(), false), new CancellationToken());
            ApplyRepository.Verify(r => r.GetOrganisationApplications(It.IsAny<Guid>()), Times.Once);
        }
    }
}