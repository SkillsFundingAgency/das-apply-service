using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply
{
    public class StartApplicationRequest : IRequest<StartApplicationResponse>
    {
        public Guid UserId { get; set; }

        public string ApplicationType { get; set; }

        public StartApplicationRequest(Guid userId, string applicationType)
        {
            UserId = userId;
            ApplicationType = applicationType;
        }
    }
}