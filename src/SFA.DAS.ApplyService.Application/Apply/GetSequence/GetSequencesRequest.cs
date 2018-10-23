using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.GetSequence
{
    public class GetSequencesRequest : IRequest<List<Sequence>>
    {

        public GetSequencesRequest(Guid applicationId, Guid userId)
        {
            UserId = userId;
            ApplicationId = applicationId;
        }
        
        public Guid UserId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}