using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.AllowedProviders
{
    public class IsUkprnOnAllowedProvidersListRequest : IRequest<bool>
    {
        public int UKPRN { get; set; }

        public IsUkprnOnAllowedProvidersListRequest(int ukprn)
        {
            UKPRN = ukprn;
        }
    }
}
