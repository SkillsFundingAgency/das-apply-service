﻿using MediatR;
using SFA.DAS.ApplyService.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.AllowedProviders
{
    public class RemoveAllowedProviderHandler : IRequestHandler<RemoveAllowedProviderRequest, bool>
    {
        private readonly IAllowedProvidersRepository _repository;

        public RemoveAllowedProviderHandler(IAllowedProvidersRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(RemoveAllowedProviderRequest request, CancellationToken cancellationToken)
        {
            return await _repository.RemoveFromAllowedProvidersList(request.Ukprn);
        }
    }
}
