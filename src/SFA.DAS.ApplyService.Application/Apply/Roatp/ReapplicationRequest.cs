using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class ReapplicationRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; set; }
        public string UserId { get; set; }
    }
}