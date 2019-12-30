using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply
{
    public class StartApplicationRequest : IRequest<StartApplicationResponse>
    {
        public Guid ApplicationId { get; set; }
        public List<ApplySequence> ApplySequences { get; set; }
        public Guid CreatingContactId { get; set; }
        public int ProviderRoute { get; set; }
    }
}