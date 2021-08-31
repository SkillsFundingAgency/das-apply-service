using MediatR;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.AllowedProviders
{
    public class AddAllowedProviderRequest : IRequest<bool>
    {
        public int Ukprn { get; }
        public DateTime StartDateTime { get; }
        public DateTime EndDateTime { get; }

        public AddAllowedProviderRequest(int ukprn, DateTime startDateTime, DateTime endDateTime)
        {
            Ukprn = ukprn;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
        }
    }
}
