using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class ChangeProviderRouteRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; set; }
        public int ProviderRoute { get; set; }
    }
}
