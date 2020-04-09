using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Start;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.StartApplicationHandlerTests
{
    public class When_any_organisation_starts_application : StartApplicationHandlerTestsBase
    {
        [Test]
        public async Task Then_ApplicationId_is_returned()
        {
            var request = new StartApplicationRequest 
            { 
                ApplicationId = ApplicationId, 
                CreatingContactId = UserId,
                ApplySequences = new List<ApplySequence>
                {
                    new ApplySequence
                    {
                        SequenceId = Guid.NewGuid(),
                        SequenceNo = 1,
                        Sections = new List<ApplySection>
                        {
                            new ApplySection
                            {
                                SectionId = Guid.NewGuid(),
                                SectionNo = 1
                            }
                        }
                    }
                }
            };

            var result = await Handler.Handle(request, CancellationToken.None);

            result.Should().Be(ApplicationId);
        }
    }
}