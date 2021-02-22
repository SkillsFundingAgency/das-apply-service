using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.WhitelistedProviders
{
    public class CheckIsUkprnWhitelistedHandler : IRequestHandler<CheckIsUkprnWhitelistedRequest, bool>
    {
        private IApplyRepository _repository;
        private ILogger<CheckIsUkprnWhitelistedHandler> _logger;

        public CheckIsUkprnWhitelistedHandler(IApplyRepository repository, ILogger<CheckIsUkprnWhitelistedHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(CheckIsUkprnWhitelistedRequest request, CancellationToken cancellationToken)
        {
            var isUkprnWhitelisted = await _repository.IsUkprnWhitelisted(request.UKPRN);

            return await Task.FromResult(isUkprnWhitelisted);
        }
    }
}
