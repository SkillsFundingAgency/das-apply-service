using MediatR;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.AllowedProviders
{
    public class CanUkprnStartApplicationRequestHandler : IRequestHandler<CanUkprnStartApplicationRequest, bool>
    {
        private readonly IAllowedProvidersRepository _repository;

        public CanUkprnStartApplicationRequestHandler(IAllowedProvidersRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(CanUkprnStartApplicationRequest request, CancellationToken cancellationToken)
        {
            return await _repository.CanUkprnStartApplication(request.UKPRN);
        }
    }
}
