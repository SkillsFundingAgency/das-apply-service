using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.AllowedProviders
{
    public class RemoveAllowedProviderRequest : IRequest<bool>
    {
        public int Ukprn { get; }

        public RemoveAllowedProviderRequest(int ukprn)
        {
            Ukprn = ukprn;
        }
    }
}
