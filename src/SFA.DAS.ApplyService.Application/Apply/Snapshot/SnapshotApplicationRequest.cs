using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.Snapshot
{
    public class SnapshotApplicationRequest : IRequest<Guid>
    {
        public Guid ApplicationId { get; set; }
        public Guid SnapshotApplicationId { get; set; }
        public List<ApplySequence> Sequences { get; set; }
    }
}
