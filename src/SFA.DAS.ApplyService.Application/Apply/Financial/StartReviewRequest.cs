using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Financial
{
    public class StartReviewRequest : IRequest
    {
        public Guid ApplicationId { get; }

        public StartReviewRequest(Guid applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}