
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class GetNotRequiredOverridesHandler : IRequestHandler<GetNotRequiredOverridesRequest, NotRequiredOverrideConfiguration>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly ILogger<GetNotRequiredOverridesHandler> _logger;

        public GetNotRequiredOverridesHandler(IApplyRepository applyRepository, ILogger<GetNotRequiredOverridesHandler> logger)
        {
            _applyRepository = applyRepository;
            _logger = logger;
        }

        public async Task<NotRequiredOverrideConfiguration> Handle(GetNotRequiredOverridesRequest request, CancellationToken cancellationToken)
        {
            var configuration = await _applyRepository.GetNotRequiredOverrides(request.ApplicationId);
            return await Task.FromResult(configuration);
        }
    }
}
