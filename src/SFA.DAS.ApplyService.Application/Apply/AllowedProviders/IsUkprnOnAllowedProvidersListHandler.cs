using MediatR;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.AllowedProviders
{
    public class IsUkprnOnAllowedProvidersListHandler : IRequestHandler<IsUkprnOnAllowedProvidersListRequest, bool>
    {
        private IAllowedProvidersRepository _repository;

        public IsUkprnOnAllowedProvidersListHandler(IAllowedProvidersRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(IsUkprnOnAllowedProvidersListRequest request, CancellationToken cancellationToken)
        {
            return await _repository.IsUkprnOnAllowedProvidersList(request.UKPRN);
        }
    }
}
