using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.AllowedProviders;
using SFA.DAS.ApplyService.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.AllowedProviders
{
    public class GetAllowedProviderDetailsHandler : IRequestHandler<GetAllowedProviderDetailsRequest, AllowedProvider>
    {
        private readonly IAllowedProvidersRepository _repository;

        public GetAllowedProviderDetailsHandler(IAllowedProvidersRepository repository)
        {
            _repository = repository;
        }

        public async Task<AllowedProvider> Handle(GetAllowedProviderDetailsRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllowedProviderDetails(request.Ukprn);
        }
    }
}
