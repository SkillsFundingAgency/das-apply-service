using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.AllowedProviders
{
    public class IsUkprnOnAllowedProvidersListRequest : IRequest<bool>
    {
        public int UKPRN { get; }

        public IsUkprnOnAllowedProvidersListRequest(int ukprn)
        {
            UKPRN = ukprn;
        }
    }
}
