using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Start;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.StartApplicationHandlerTests
{
    public class When_any_organisation_starts_application : StartApplicationHandlerTestsBase
    {
        [Test]
        public async Task Then_ApplicationId_is_returned()
        {
            var request = new StartApplicationRequest { ApplicationId = ApplicationId, CreatingContactId = UserId };

            var result = await Handler.Handle(request, CancellationToken.None);

            result.Should().Be(ApplicationId);
        }
    }
}