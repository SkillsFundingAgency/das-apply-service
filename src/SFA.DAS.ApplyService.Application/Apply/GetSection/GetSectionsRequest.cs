using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.GetSection
{
    public class GetSectionsRequest : IRequest<List<ApplicationSection>>
    {
        public GetSectionsRequest(Guid applicationId, int sequenceId, Guid userId)
        {
            UserId = userId;
            ApplicationId = applicationId;
            SequenceId = sequenceId;
        }
        
        public Guid UserId { get; set; }
        public Guid ApplicationId { get; set; }
        public int SequenceId { get; }
    }
}