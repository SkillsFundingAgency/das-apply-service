using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.AllowedProviders;
using SFA.DAS.ApplyService.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.AllowedProviders
{
    public class GetAllowedProvidersListHandler : IRequestHandler<GetAllowedProvidersListRequest, List<AllowedProvider>>
    {
        private IAllowedProvidersRepository _repository;

        public GetAllowedProvidersListHandler(IAllowedProvidersRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AllowedProvider>> Handle(GetAllowedProvidersListRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllowedProvidersList(request.SortColumn, request.SortOrder);
        }
    }
}
