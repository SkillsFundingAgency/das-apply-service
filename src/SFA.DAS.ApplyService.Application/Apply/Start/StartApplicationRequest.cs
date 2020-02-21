using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.Start
{
    public class StartApplicationRequest : IRequest<Guid>
    {
        public Guid ApplicationId { get; set; }
        public List<ApplySequence> ApplySequences { get; set; }
        public Guid CreatingContactId { get; set; }
        public int ProviderRoute { get; set; }
        public string ProviderRouteName { get;set }
    }
}