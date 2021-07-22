using MediatR;
using SFA.DAS.ApplyService.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.AllowedProviders
{
    public class AddAllowedProviderHandler : IRequestHandler<AddAllowedProviderRequest, bool>
    {
        private readonly IAllowedProvidersRepository _repository;

        public AddAllowedProviderHandler(IAllowedProvidersRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(AddAllowedProviderRequest request, CancellationToken cancellationToken)
        {
            return await _repository.AddToAllowedProvidersList(request.Ukprn, request.StartDateTime, request.EndDateTime);
        }
    }
}
