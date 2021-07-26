using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.AllowedProviders;

namespace SFA.DAS.ApplyService.Application.Apply.AllowedProviders
{
    public class GetAllowedProviderDetailsRequest : IRequest<AllowedProvider>
    {
        public int Ukprn { get; }

        public GetAllowedProviderDetailsRequest(int ukprn)
        {
            Ukprn = ukprn;
        }
    }
}
