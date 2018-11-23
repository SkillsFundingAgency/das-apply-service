using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.GetSection
{
    public class GetActiveSequenceRequest : IRequest<ApplicationSequence>
    {
        public GetActiveSequenceRequest(Guid applicationId)
        {
            ApplicationId = applicationId;
        }
        
        public Guid ApplicationId { get; set; }
    }
}