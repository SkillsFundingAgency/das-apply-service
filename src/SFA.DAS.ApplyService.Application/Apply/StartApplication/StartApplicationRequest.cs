using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.StartApplication
{
    public class StartApplicationRequest : IRequest<StartApplicationResponse>
    {
        public Guid UserId { get; set; }

        public StartApplicationRequest(Guid userId)
        {     
            UserId = userId;
        }
    }
}