using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Appeals.Commands.CancelAppeal
{
    public class CancelAppealCommand : IRequest
    {
        public Guid ApplicationId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
