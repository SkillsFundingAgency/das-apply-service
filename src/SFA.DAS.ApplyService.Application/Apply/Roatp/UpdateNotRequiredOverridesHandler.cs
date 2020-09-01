using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class UpdateNotRequiredOverridesHandler : IRequestHandler<UpdateNotRequiredOverridesRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly ILogger<UpdateNotRequiredOverridesHandler> _logger;

        public UpdateNotRequiredOverridesHandler(IApplyRepository applyRepository, ILogger<UpdateNotRequiredOverridesHandler> logger)
        {
            _applyRepository = applyRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateNotRequiredOverridesRequest request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Saving not required overrides in api handler for applicationid [{request.ApplicationId}], count {request.NotRequiredOverrides?.NotRequiredOverrides.Count()}");
            var result = await _applyRepository.SaveNotRequiredOverrides(request.ApplicationId, request.NotRequiredOverrides);
            _logger.LogDebug($"result of saving required overrides in api handler for applicationid [{request.ApplicationId}], result {result}");

            return result;
        }
    }
}
